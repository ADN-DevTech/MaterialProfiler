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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Inventor;
using Autodesk.ADN.InvUtility.CommandUtils;
using Autodesk.ADN.InvUtility.InventorUtils;

namespace MaterialProfiler
{
    [AdnCommand]
    public class MaterialProfilerCmd : AdnButtonCommandBase
    {
        ApplicationAddInSite _addInSiteObject;

        public MaterialProfilerCmd(ApplicationAddInSite addInSiteObject) :
            base(addInSiteObject.Application)
        {
            _addInSiteObject = addInSiteObject;
        }

        public override string DisplayName
        {
            get
            {
                return "Material\nProfiler";
            }
        }

        public override string InternalName
        {
            get
            {
                return "Autodesk.MaterialProfiler.MaterialProfilerCmd";
            }
        }

        public override CommandTypesEnum Classification
        {
            get
            {
                return CommandTypesEnum.kEditMaskCmdType;
            }
        }

        public override string ClientId
        {
            get
            {
                Type t = typeof(StandardAddInServer);
                return t.GUID.ToString("B");
            }
        }

        public override string Description
        {
            get
            {
                return "Displays Material Profiler control";
            }
        }

        public override string ToolTipText
        {
            get
            {
                return "Displays Material Profiler control";
            }
        }

        public override ButtonDisplayEnum ButtonDisplay
        {
            get
            {
                return ButtonDisplayEnum.kDisplayTextInLearningMode;
            }
        }

        public override string StandardIconName
        {
            get
            {
                return "MaterialProfiler.resources.profiler.16x16.ico";
            }
        }

        public override string LargeIconName
        {
            get
            {
                return "MaterialProfiler.resources.profiler.32x32.ico";
            }
        }

        protected override void OnExecute(NameValueMap context)
        {
            ProfilerDockableWnd.MakeVisible(
                _addInSiteObject, 
                DockingStateEnum.kDockLeft);

            Terminate();
        }

        protected override void OnHelp(NameValueMap context)
        {

        }
    }
}
