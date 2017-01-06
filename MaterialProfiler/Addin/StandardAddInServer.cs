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
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

using Inventor;
using Microsoft.Win32;

using Autodesk.ADN.InvUtility.InventorUtils;
using Autodesk.ADN.InvUtility.CommandUtils;
using Autodesk.ADN.InvUtility.RibbonUtils;

namespace MaterialProfiler
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("59f8f264-156e-4e54-b9c1-57a18ac60a96"), 
    System.Runtime.InteropServices.ComVisible(true)]
    public class StandardAddInServer : Autodesk.ADN.InvUtility.AddIn.AdnAddInServer
    {
        public override void Activate(ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            base.Activate(addInSiteObject, firstTime);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override string RibbonResource
        {
            get
            {
                return "MaterialProfiler.resources.ribbons.xml";
            }
        }
    }
}
