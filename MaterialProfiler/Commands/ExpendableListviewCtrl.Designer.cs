namespace MaterialProfiler
{
    partial class ExpendableListviewCtrl
    {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node8");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node5", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Assembly", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode8});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpendableListviewCtrl));
            this.panelMain = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._treeView = new System.Windows.Forms.TreeView();
            this.TreeImages = new System.Windows.Forms.ImageList(this.components);
            this._listViewEx = new ListViewEmbeddedControls.ListViewEx();
            this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerDensity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerVolume = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerMass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPercentage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuExcelExport = new System.Windows.Forms.ToolStripMenuItem();
            this._TreeContextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuItemExpand = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItemCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this._contextMenu.SuspendLayout();
            this._TreeContextmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.splitContainer1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(794, 488);
            this.panelMain.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._listViewEx);
            this.splitContainer1.Size = new System.Drawing.Size(794, 488);
            this.splitContainer1.SplitterDistance = 126;
            this.splitContainer1.TabIndex = 6;
            // 
            // _treeView
            // 
            this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeView.HideSelection = false;
            this._treeView.ImageIndex = 0;
            this._treeView.ImageList = this.TreeImages;
            this._treeView.ItemHeight = 20;
            this._treeView.Location = new System.Drawing.Point(0, 0);
            this._treeView.Name = "_treeView";
            treeNode1.Name = "Node2";
            treeNode1.Text = "Node2";
            treeNode2.Name = "Node3";
            treeNode2.Text = "Node3";
            treeNode3.Name = "Node4";
            treeNode3.Text = "Node4";
            treeNode4.Name = "Node1";
            treeNode4.Text = "Node1";
            treeNode5.Name = "Node6";
            treeNode5.Text = "Node6";
            treeNode6.Name = "Node7";
            treeNode6.Text = "Node7";
            treeNode7.Name = "Node8";
            treeNode7.Text = "Node8";
            treeNode8.Name = "Node5";
            treeNode8.Text = "Node5";
            treeNode9.Name = "Node0";
            treeNode9.Text = "Assembly";
            this._treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this._treeView.SelectedImageIndex = 0;
            this._treeView.Size = new System.Drawing.Size(126, 488);
            this._treeView.TabIndex = 5;
            this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Handle_TreeNode_Selected);
            this._treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Handle_TreeNode_MouseClick);
            // 
            // TreeImages
            // 
            this.TreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeImages.ImageStream")));
            this.TreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "folderClose3.ico");
            this.TreeImages.Images.SetKeyName(1, "folderOpen3.ico");
            this.TreeImages.Images.SetKeyName(2, "INV_Assembly_Doc_Active_Browser_16x16.ico");
            this.TreeImages.Images.SetKeyName(3, "INV_Model_Solidbody_Active_Browser_16x16.ico");
            // 
            // _listViewEx
            // 
            this._listViewEx.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.headerDensity,
            this.headerVolume,
            this.headerMass,
            this.headerPercentage});
            this._listViewEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listViewEx.FullRowSelect = true;
            this._listViewEx.GridLines = true;
            this._listViewEx.HideSelection = false;
            this._listViewEx.Location = new System.Drawing.Point(0, 0);
            this._listViewEx.MultiSelect = false;
            this._listViewEx.Name = "_listViewEx";
            this._listViewEx.ShowGroups = false;
            this._listViewEx.Size = new System.Drawing.Size(664, 488);
            this._listViewEx.SmallImageList = this.imageList1;
            this._listViewEx.TabIndex = 5;
            this._listViewEx.UseCompatibleStateImageBehavior = false;
            this._listViewEx.View = System.Windows.Forms.View.Details;
            this._listViewEx.SelectedIndexChanged += new System.EventHandler(this.Handle_ListView_SelectedIndexChanged);
            this._listViewEx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Handle_ListView_MouseUp);
            // 
            // headerName
            // 
            this.headerName.Text = "Material";
            this.headerName.Width = 69;
            // 
            // headerDensity
            // 
            this.headerDensity.Text = "Density";
            this.headerDensity.Width = 76;
            // 
            // headerVolume
            // 
            this.headerVolume.Text = "Volume";
            this.headerVolume.Width = 76;
            // 
            // headerMass
            // 
            this.headerMass.Text = "Mass";
            this.headerMass.Width = 80;
            // 
            // headerPercentage
            // 
            this.headerPercentage.Text = "Percentage (of Total Mass)";
            this.headerPercentage.Width = 359;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "lvheight.PNG");
            // 
            // _contextMenu
            // 
            this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuExcelExport});
            this._contextMenu.Name = "_contextMenu";
            this._contextMenu.Size = new System.Drawing.Size(172, 26);
            // 
            // _menuExcelExport
            // 
            this._menuExcelExport.Name = "_menuExcelExport";
            this._menuExcelExport.Size = new System.Drawing.Size(171, 22);
            this._menuExcelExport.Text = "Export List to Excel";
            // 
            // _TreeContextmenu
            // 
            this._TreeContextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuItemExpand,
            this._menuItemCollapse});
            this._TreeContextmenu.Name = "_TreeContextmenu";
            this._TreeContextmenu.Size = new System.Drawing.Size(137, 48);
            // 
            // _menuItemExpand
            // 
            this._menuItemExpand.Name = "_menuItemExpand";
            this._menuItemExpand.Size = new System.Drawing.Size(136, 22);
            this._menuItemExpand.Text = "Expand All";
            // 
            // _menuItemCollapse
            // 
            this._menuItemCollapse.Name = "_menuItemCollapse";
            this._menuItemCollapse.Size = new System.Drawing.Size(136, 22);
            this._menuItemCollapse.Text = "Collapse All";
            // 
            // ExpendableListviewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ExpendableListviewCtrl";
            this.Size = new System.Drawing.Size(794, 488);
            this.panelMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this._contextMenu.ResumeLayout(false);
            this._TreeContextmenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView _treeView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList TreeImages;
        private System.Windows.Forms.ContextMenuStrip _contextMenu;
        private System.Windows.Forms.ToolStripMenuItem _menuExcelExport;
        private ListViewEmbeddedControls.ListViewEx _listViewEx;
        private System.Windows.Forms.ColumnHeader headerName;
        private System.Windows.Forms.ColumnHeader headerDensity;
        private System.Windows.Forms.ColumnHeader headerVolume;
        private System.Windows.Forms.ColumnHeader headerMass;
        private System.Windows.Forms.ColumnHeader headerPercentage;
        private System.Windows.Forms.ContextMenuStrip _TreeContextmenu;
        private System.Windows.Forms.ToolStripMenuItem _menuItemExpand;
        private System.Windows.Forms.ToolStripMenuItem _menuItemCollapse;

    }
}
