using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Inventor;

namespace MaterialProfiler
{
	partial class ProfilerDockableWnd
	{
        private static ProfilerDockableWnd _instance = null;

        public static ProfilerDockableWnd Instance
        {
            get
            {
                return _instance;
            }
        }

        public static void CreateInstance(
            ApplicationAddInSite addInSiteObject,
            DockingStateEnum initialDockingState)
        {
            _instance = new ProfilerDockableWnd(
                addInSiteObject,
                initialDockingState);
        }

        #region Constructors
        /// <summary>Initialize the form in <see cref="DesignMode"/>. 
        /// Do not use this constructor from code.</summary>
        public ProfilerDockableWnd()
        {
            System.Diagnostics.Debug.Assert(DesignMode, "Default constructor should only be used in Design Mode.");
            InitializeComponent();
        }

        /// <summary>Initialize the dockable window.</summary>
        /// <param name="addInSite">The ApplicationAddInSite object supplied by Inventor.</param>
        public ProfilerDockableWnd(Inventor.ApplicationAddInSite addInSite)
            : this(addInSite, default(Inventor.DockingStateEnum))
        {
        }

        public ProfilerDockableWnd(
            Inventor.ApplicationAddInSite addInSite,
            Inventor.DockingStateEnum initialDockingState)
        {
            if (addInSite == null) // We can't build the dockable window without the add-in site object.
                throw new ArgumentNullException("addInSite");

            // Initialize the form with the designer code.
            InitializeComponent();

            // Make sure the components object is created. (The designer doesn't always create it.)
            if (components == null)
                components = new Container();

            // Create the DockableWindow using a managed wrapper and add it to the components collection.
            components.Add(
                new DockableWindowWrapper(addInSite, this, initialDockingState),
                typeof(DockableWindowWrapper).Name);
        }

        #endregion

        #region Generated Code for initializing the Dockable Window
        /// <summary>Gets the <see cref="DockableWindowWrapper"/> object.</summary>
        /// <exception cref="InvalidOperationException">components is null or the wrapper cannot be found.</exception>
        /// <exception cref="ObjectDisposedException">The object is already disposed.</exception>
        private DockableWindowWrapper GetWrapper()
        {
            DockableWindowWrapper wrapper = null;

            if (IsDisposed) // Check for disposed.
                throw new ObjectDisposedException(base.GetType().Name);

            // Try to find the wrapper in the components collection by name.
            if (components == null || null == (wrapper = components.Components[typeof(DockableWindowWrapper).Name]
                as DockableWindowWrapper))
                throw new InvalidOperationException(); // Wrapper not found.

            return wrapper;
        }

        /// <summary>Sets the visibility of the form.</summary>
        /// <param name="value">true to make the control visible; otherwise false.</param>
        /// This hook ensures the form gets attached to the DockableWindow properly.
        protected override sealed void SetVisibleCore(bool value)
        {
            // A handle is created the first time the form is shown; initialize the DockableWindow.
            // First time is when the handle is not created yet.
            if (value && !IsHandleCreated && !DesignMode)
            {
                // Show the form making the Inventor DockableWindow its owner.
                // This method will be called again inside Show(IWin32Window) but the handle will be created first.
                base.Show(GetWrapper());
            }
            else
            {
                // The Handle is already created so just pass through to the base implementation.
                base.SetVisibleCore(value);
            }
        }

        public new bool Visible
        {
            get { return GetWrapper().Visible; }
            set { GetWrapper().Visible = value; }
        }

        /// <summary>Gets or sets the docking state of the DockingWindow.</summary>
        public Inventor.DockingStateEnum DockingState
        {
            get
            {
                if (DesignMode)
                    return Inventor.DockingStateEnum.kFloat;
                return GetWrapper().DockingState;
            }
            set
            {
                if (!DesignMode)
                    GetWrapper().DockingState = value;
            }
        }

        private void OnVisibilityChanged(OnVisibilityChangedEventArgs e)
        {
            if (OnVisibilityChangedEvent != null)
                OnVisibilityChangedEvent(this, e);
        }

        public event OnVisibilityChangedHandler OnVisibilityChangedEvent = null;

        #region DockableWindowWrapper
        /// <summary>Wrapper for Inventor.DockableWindow.</summary>
        private sealed class DockableWindowWrapper : Component, IWin32Window
        {
            /// <summary>The wrapped Inventor.DockableWindow object.</summary>
            private Inventor.DockableWindow dockableWindow;

            private DockableWindowsEvents _dockableWindowsEvents;

            private ProfilerDockableWnd _form;

            /// <summary>Initializes the <see cref="DockableWindowWrapper"/>.</summary>
            /// <param name="addInSite">The ApplicationAddInSite object supplied by Inventor.</param>
            /// <param name="form">The managed form to add to the DockableWindow.</param>
            /// <param name="initialDockingState">The initial docking state of the DockableWindow 
            /// if it is created for the first time.</param>
            internal DockableWindowWrapper(
                Inventor.ApplicationAddInSite addInSite,
                ProfilerDockableWnd form,
                Inventor.DockingStateEnum initialDockingState)
            {
                System.Diagnostics.Debug.Assert(addInSite != null && form != null);

                _form = form;

                // Set up the parameters.
                string clientId = addInSite.Parent.ClientId;
                string internalName = _form.GetType().FullName + "." + form.Name;
                string title = _form.Text;

                // We don't want the border to show since the form will be docked inside the DockableWindow.
                _form.FormBorderStyle = FormBorderStyle.None;

                bool isCustomized = false;

                // Retrieve or create the dockable window.
                try
                {
                    dockableWindow = addInSite.Application.UserInterfaceManager.DockableWindows[internalName];

                    isCustomized = true;
                }
                catch
                {
                    dockableWindow = addInSite.Application.UserInterfaceManager.DockableWindows.Add(
                        clientId,
                        internalName,
                        title);
                }

                // Set the minimum size of the dockable window.
                System.Drawing.Size minimumSize = form.MinimumSize;
                if (!minimumSize.IsEmpty)
                    dockableWindow.SetMinimumSize(minimumSize.Height, minimumSize.Width);

                // Set the initial docking state of the DockableWindow if it is not remembered by Inventor.
                if (initialDockingState != default(Inventor.DockingStateEnum) && !isCustomized)
                    dockableWindow.DockingState = initialDockingState;

                // Set initial state to invisible.
                dockableWindow.Visible = false;

                // Listen for events.
                _form.HandleCreated += new EventHandler(OnHandleCreated);

                _form.VisibleChanged += new EventHandler(OnVisibleChanged);

                _dockableWindowsEvents =
                    addInSite.Application.UserInterfaceManager.DockableWindows.Events;
 
                _dockableWindowsEvents.OnShow += 
                    new DockableWindowsEventsSink_OnShowEventHandler(DockableWindowsEvents_OnShow);

                _dockableWindowsEvents.OnHide +=
                    new DockableWindowsEventsSink_OnHideEventHandler(DockableWindowsEvents_OnHide);
            }

            void DockableWindowsEvents_OnShow(
                DockableWindow DockableWindow, 
                EventTimingEnum BeforeOrAfter, 
                NameValueMap Context, 
                out HandlingCodeEnum HandlingCode)
            {
                HandlingCode = HandlingCodeEnum.kEventNotHandled;

                if (DockableWindow == dockableWindow)
                {
                    OnVisibilityChangedEventArgs arg = 
                        new OnVisibilityChangedEventArgs(
                            BeforeOrAfter,
                            true);

                    _form.OnVisibilityChanged(arg);

                    HandlingCode = arg.HandlingCode;
                }
            }

            void DockableWindowsEvents_OnHide(
                DockableWindow DockableWindow,
                EventTimingEnum BeforeOrAfter,
                NameValueMap Context,
                out HandlingCodeEnum HandlingCode)
            {
                HandlingCode = HandlingCodeEnum.kEventNotHandled;

                if (DockableWindow == dockableWindow)
                {
                    OnVisibilityChangedEventArgs arg =
                       new OnVisibilityChangedEventArgs(
                           BeforeOrAfter,
                           false);

                    _form.OnVisibilityChanged(arg);

                    HandlingCode = arg.HandlingCode;
                }
            }

            /// <summary>Gets or sets the docking state of the DockableWindow.</summary>
            internal Inventor.DockingStateEnum DockingState
            {
                get { return dockableWindow.DockingState; }
                set { dockableWindow.DockingState = value; }
            }

            internal bool Visible
            {
                get { return dockableWindow.Visible; }
                set { dockableWindow.Visible = value; }
            }

            // This event handler will be called when a handle is create for the managed form.
            void OnHandleCreated(object sender, EventArgs e)
            {
                // The form's handle was created so add it to the dockable window.
                dockableWindow.AddChild(((Form)sender).Handle);
            }

            // This event handler will be called when the form is shown or hidden.
            void OnVisibleChanged(object sender, EventArgs e)
            {
                // Set the Visible state of the Dockable Window to match the form.
                dockableWindow.Visible = ((Form)sender).Visible;
            }

            /// <summary>
            /// Releases unmanaged resources used by the <see cref="Component"/> and optionally
            /// releases the Inventor.DockableWindow.
            /// </summary>
            /// <param name="disposing">true to release the Dockable Window.</param>
            protected override void Dispose(bool disposing)
            {
                // Let the base do its clean-up.
                base.Dispose(disposing);

                if (disposing)
                {
                    // If disposing, we should release the dockable window.
                    Inventor.DockableWindow wnd = dockableWindow;
                    dockableWindow = null; // Clear the field so we can't release it twice.
                    if (wnd != null)
                    {
                        wnd.Visible = false;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wnd);
                    }
                }
            }

            #region IWin32Window Members

            /// <summary>Gets a handle for the dockable window.</summary>
            IntPtr IWin32Window.Handle
            {
                get { return new IntPtr(dockableWindow.HWND); }
            }

            #endregion
        }
        #endregion


        #endregion

        #region WPF Element

        private System.Windows.Forms.Integration.ElementHost _wpfHost = null;

        private void SetWpfControl(System.Windows.UIElement wpfControl)
        {
            _wpfHost = new System.Windows.Forms.Integration.ElementHost();
            _wpfHost.AutoSize = true;
            _wpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
            _wpfHost.Location = new System.Drawing.Point(0, 0);
            _wpfHost.Name = "WpfHost";
            _wpfHost.Size = new System.Drawing.Size(284, 262);
            _wpfHost.TabIndex = 0;
            _wpfHost.Text = "WpfElementHost";
            _wpfHost.Child = wpfControl;

            this.Controls.Add(_wpfHost);
        }

        public System.Windows.UIElement WpfControl
        {
            get
            {
                if(_wpfHost != null)
                    return _wpfHost.Child;

                return null;
            }
        }

        #endregion


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._expListviewCtrl = new MaterialProfiler.ExpendableListviewCtrl();
            this.SuspendLayout();
            // 
            // _expListviewCtrl
            // 
            this._expListviewCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._expListviewCtrl.Location = new System.Drawing.Point(0, 0);
            this._expListviewCtrl.Name = "_expListviewCtrl";
            this._expListviewCtrl.Size = new System.Drawing.Size(768, 531);
            this._expListviewCtrl.TabIndex = 0;
            // 
            // ProfilerDockableWnd
            // 
            this.ClientSize = new System.Drawing.Size(768, 531);
            this.ControlBox = false;
            this.Controls.Add(this._expListviewCtrl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfilerDockableWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Material Profiler";
            this.ResumeLayout(false);

		}

		#endregion

        private ExpendableListviewCtrl _expListviewCtrl;

        #region  OnVisibilityChanged Event code

        /////////////////////////////////////////////////////////////
        // Use: Exposes visibility change Event 
        //
        /////////////////////////////////////////////////////////////
        public class OnVisibilityChangedEventArgs : EventArgs
        {
            public EventTimingEnum BeforeOrAfter
            {
                private set;
                get;
            }

            public HandlingCodeEnum HandlingCode
            {
                set;
                get;
            }

            public bool Visible
            {
                private set;
                get;
            }

            public OnVisibilityChangedEventArgs(
                EventTimingEnum beforeOrAfter,
                bool visible)
            {
                HandlingCode = HandlingCodeEnum.kEventNotHandled;
                BeforeOrAfter = beforeOrAfter;
                Visible = visible;
            }
        }

        public delegate void OnVisibilityChangedHandler(
            object o,
            OnVisibilityChangedEventArgs e);

        #endregion
	}
}