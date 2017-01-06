////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved 
// Written by Philippe Leefsma 2012 - ADN/Developer Technical Services
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
////////////////////////////////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Inventor;

namespace MaterialProfiler
{
    public partial class ProfilerDockableWnd : Form
    {
        public static ProfilerDockableWnd MakeVisible(
            ApplicationAddInSite addInSiteObject,
            DockingStateEnum initialDockingState)
        {
            if (Instance == null)
            {
                CreateInstance(
                    addInSiteObject,
                    initialDockingState);
               
                Instance.Show();

                Instance.RefreshContent();
            }
            else
            {
                Instance.RefreshContent();
                Instance.Visible = true;
            }

		    return _instance;
        }

        public void RefreshContent()
        {
            _expListviewCtrl.RefreshContent();
        }
    }
}
