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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

using Inventor;
using Autodesk.ADN.InvUtility.InventorUtils;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.IO;


namespace MaterialProfiler
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class ExpendableListviewCtrl : UserControl
    {
        internal class TreeNodeTag
        {
            public readonly List<ComponentOccurrence> Occurrences;
            public readonly ListViewItem ListViewItem;

            public TreeNodeTag(
                ComponentOccurrence occurrence, 
                ListViewItem listViewItem)
            {
                Occurrences = new List<ComponentOccurrence>();
                Occurrences.Add(occurrence);

                ListViewItem = listViewItem;
            }

            public TreeNodeTag(
               List<ComponentOccurrence> occurrences,
               ListViewItem listViewItem)
            {
                Occurrences = new List<ComponentOccurrence>();
                Occurrences.AddRange(occurrences);

                ListViewItem = listViewItem;
            }
        }

        internal class ListViewItemTag
        {
            public readonly List<ComponentOccurrence> Occurrences;
            public readonly TreeNode TreeNode;

            public ListViewItemTag(
                ComponentOccurrence occurrence,
                TreeNode treeNode)
            {
                Occurrences = new List<ComponentOccurrence>();
                Occurrences.Add(occurrence);

                TreeNode = treeNode;
            }

            public ListViewItemTag(
               List<ComponentOccurrence> occurrences,
               TreeNode treeNode)
            {
                Occurrences = new List<ComponentOccurrence>();
                Occurrences.AddRange(occurrences);

                TreeNode = treeNode;
            }
        }

        AssemblyDocument ActiveDocument
        {
            get
            {
                try
                {
                    AssemblyDocument document = AdnInventorUtilities.InvApplication.ActiveDocument
                        as AssemblyDocument;

                    return document;
                }
                catch
                {
                    return null;
                }
            }
        }

        bool _InventorSelectionEnabled = true;

        public ExpendableListviewCtrl()
        {
            InitializeComponent();

            _treeView.AfterExpand += new TreeViewEventHandler(Handle_TreeView_AfterExpand);
            _treeView.AfterCollapse += new TreeViewEventHandler(Handle_TreeView_AfterCollapse);

            _menuExcelExport.Click += new EventHandler(Handle_MenuExcelExport_Click);
            _menuItemExpand.Click += new EventHandler(Handle_MenuItemExpand_Click);
            _menuItemCollapse.Click += new EventHandler(Handle_MenuItemCollapse_Click);
        }

        private void AfterCollapseRecursive(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                TreeNodeTag tag = childNode.Tag as TreeNodeTag;
                tag.ListViewItem.Remove();

                AfterCollapseRecursive(childNode);
            }
        }

        private void Handle_TreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            AfterCollapseRecursive(e.Node);
        }

        private void AfterExpandRecursive(TreeNode node, ref int idx)
        {
            if (node != _treeView.Nodes[0])
            {
                TreeNodeTag nodeItem = node.Tag as TreeNodeTag;
                idx = nodeItem.ListViewItem.Index + 1;
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                TreeNodeTag tag = childNode.Tag as TreeNodeTag;

                _listViewEx.Items.Insert(idx++, tag.ListViewItem);

                if (childNode.IsExpanded)
                {
                    AfterExpandRecursive(childNode, ref idx);
                }
            }
        }

        private void Handle_TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            int idx = 0;

            AfterExpandRecursive(e.Node, ref idx);
        }

        public void RefreshContent()
        {
            _treeView.Nodes.Clear();

            _listViewEx.Items.Clear();

            if (ActiveDocument == null)
                return;

            ActiveDocument.DocumentEvents.OnChangeSelectSet += 
                new DocumentEventsSink_OnChangeSelectSetEventHandler(
                    DocumentEvents_OnChangeSelectSet);

            _listViewEx.Columns[1].Text = "Density (" + MaterialUtils.GetDocDensityUnits(ActiveDocument as Document) + ")";
            _listViewEx.Columns[2].Text = "Volume (" + MaterialUtils.GetDocVolumeUnits(ActiveDocument as Document) + ")";
            _listViewEx.Columns[3].Text = "Mass (" + MaterialUtils.GetDocMassUnits(ActiveDocument as Document) + ")";

            Dictionary<Material, MaterialUtils.MaterialGlobalInfos> materialInfos =
                MaterialUtils.GetMaterialInfos(ActiveDocument);

            if (materialInfos == null)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Unable to parse material information...",
                    "Error parsing materials",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                return;
            }

            TreeNode rootNode = _treeView.Nodes.Add("Assembly", ActiveDocument.DisplayName, 2, 2);
            
            foreach (Material material in materialInfos.Keys)
            {
                TreeNode materialNode = rootNode.Nodes.Add(material.Name);

                ListViewItem materialItem = _listViewEx.Items.Insert(
                    _listViewEx.Items.Count, 
                    material.Name);

                MaterialUtils.MaterialInfos globalInfos = materialInfos[material].GlobalInfos;

                ListViewItem.ListViewSubItem subItem;

                double density = MaterialUtils.GetDocDensity(ActiveDocument as Document, globalInfos.Density);

                subItem = materialItem.SubItems.Add(MaterialUtils.FormatVisible(density));
                subItem = materialItem.SubItems.Add(MaterialUtils.FormatVisible(globalInfos.Volume));
                subItem = materialItem.SubItems.Add(MaterialUtils.FormatVisible(globalInfos.Mass));
                subItem.Tag = globalInfos.DbMass;

                CustomControls.LabeledProgressBar ctrlGlobal =
                   new CustomControls.LabeledProgressBar(globalInfos.MassPercentage);

                _listViewEx.AddEmbeddedControl(ctrlGlobal, 4, materialItem.Index);

                List<ComponentOccurrence> occurrences = 
                    materialInfos[material].BreakDownInfos.Keys.ToList<ComponentOccurrence>();

                materialItem.Tag = new ListViewItemTag(occurrences, materialNode);
                materialNode.Tag = new TreeNodeTag(occurrences, materialItem);

                foreach (ComponentOccurrence occurrence in occurrences)
                {
                    TreeNode occNode = materialNode.Nodes.Add(occurrence.Name, occurrence.Name, 3, 3);

                    ListViewItem occItem = _listViewEx.Items.Insert(
                         _listViewEx.Items.Count,
                         material.Name + " [" + occurrence.Name + "]");

                    MaterialUtils.MaterialInfos breakDownInfos = 
                        materialInfos[material].BreakDownInfos[occurrence];

                    double dOcc = MaterialUtils.GetDocDensity(
                        ActiveDocument as Document, 
                        breakDownInfos.Density);

                    subItem = occItem.SubItems.Add(MaterialUtils.FormatVisible(dOcc));
                    subItem = occItem.SubItems.Add(MaterialUtils.FormatVisible(breakDownInfos.Volume));
                    subItem = occItem.SubItems.Add(MaterialUtils.FormatVisible(breakDownInfos.Mass));
                    subItem.Tag = breakDownInfos.DbMass;

                    CustomControls.LabeledProgressBar ctrlBreak =
                        new CustomControls.LabeledProgressBar(breakDownInfos.MassPercentage);

                    _listViewEx.AddEmbeddedControl(ctrlBreak, 4, occItem.Index);

                    occItem.Tag = new ListViewItemTag(occurrence, occNode);
                    occNode.Tag = new TreeNodeTag(occurrence, occItem);
                }
            }

            foreach (ListViewItem item in _listViewEx.Items)
            {
                item.Remove();
            }

            rootNode.Expand(); 
        }

        void DocumentEvents_OnChangeSelectSet(
            EventTimingEnum BeforeOrAfter, 
            NameValueMap Context, 
            out HandlingCodeEnum HandlingCode)
        {
            HandlingCode = HandlingCodeEnum.kEventNotHandled;

            if (!_InventorSelectionEnabled)
                return;

            try
            {
                if (BeforeOrAfter == EventTimingEnum.kAfter)
                {
                    if (ActiveDocument.SelectSet.Count != 0)
                    {
                        object obj = ActiveDocument.SelectSet[ActiveDocument.SelectSet.Count];

                        if (obj is ComponentOccurrence)
                        {
                            ComponentOccurrence occurrence = obj as ComponentOccurrence;

                            foreach (TreeNode childNode in _treeView.Nodes[0].Nodes)
                            {
                                foreach (TreeNode subChildNode in childNode.Nodes)
                                {
                                    TreeNodeTag tag = subChildNode.Tag as TreeNodeTag;

                                    if (tag.Occurrences.Contains(occurrence))
                                    {
                                        childNode.Expand();

                                        _treeView.SelectedNode = subChildNode;                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            
            }
        }

        private void Handle_ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_listViewEx.SelectedItems.Count == 0)
                return;

            ListViewItemTag tag = _listViewEx.SelectedItems[0].Tag as ListViewItemTag;

            if (tag != null)
            {
                _InventorSelectionEnabled = false;

                if(_treeView.SelectedNode != tag.TreeNode)
                    _treeView.SelectedNode = tag.TreeNode;
            }
        }

        private void Handle_TreeNode_Selected(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNodeTag tag = e.Node.Tag as TreeNodeTag;

                if (tag != null)
                {
                    _InventorSelectionEnabled = false;

                    tag.ListViewItem.Selected = true;

                    ActiveDocument.SelectSet.Clear();

                    foreach (ComponentOccurrence occurrence in tag.Occurrences)
                    {
                        ActiveDocument.SelectSet.Select(occurrence);
                    }

                    _InventorSelectionEnabled = true;
                }
            }
            catch
            { 
            
            }
        }

        private void ExcelExport() 
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "Excel Files (*.xls; *xlsx)|*.xlsx;*.xlsx";
                sfd.Title = "Export to Excel format";

                if (sfd.ShowDialog() == DialogResult.Cancel)
                    return;

                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add(1);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                ws.Cells[1, 1] = _listViewEx.Columns[0].Text;
                ws.Cells[1, 2] = _listViewEx.Columns[1].Text;
                ws.Cells[1, 3] = _listViewEx.Columns[2].Text;
                ws.Cells[1, 4] = _listViewEx.Columns[3].Text;
                ws.Cells[1, 5] = _listViewEx.Columns[4].Text;

                TreeNode rootNode = _treeView.Nodes[0];

                int rowIdx = 2;

                foreach (ListViewItem lvi in _listViewEx.Items)
                {
                    ListViewItemTag tag = lvi.Tag as ListViewItemTag;

                    if (tag.TreeNode.Parent == rootNode)
                    {
                        ++rowIdx;
                    }

                    int columIdx = 0;

                    foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                    {
                        ws.Cells[rowIdx, ++columIdx] = lvs.Text;
                    }

                    CustomControls.LabeledProgressBar ctrl =
                        _listViewEx.GetEmbeddedControl(columIdx, lvi.Index) as
                            CustomControls.LabeledProgressBar;

                    if (ctrl != null)
                        ws.Cells[rowIdx, ++columIdx] = ctrl.LabelText;

                    ++rowIdx;
                }

                object objMissing = System.Reflection.Missing.Value;

                wb.SaveAs(sfd.FileName,
                    objMissing, objMissing, objMissing, objMissing, objMissing,
                    Excel.XlSaveAsAccessMode.xlExclusive,
                    objMissing, objMissing, objMissing, objMissing, objMissing);

                wb.Close(objMissing, objMissing, objMissing);

                app.Quit();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error during Excel export: " + ex.Message,
                    "Excel export error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void Handle_MenuExcelExport_Click(object sender, EventArgs e)
        {
            ExcelExport();
        }

        private void Handle_ListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            _contextMenu.Show(Cursor.Position);
        }

        private TreeNode _clickedNode = null;

        private void Handle_TreeNode_MouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            _clickedNode = e.Node;

            _TreeContextmenu.Show(Cursor.Position);
        }

        void Handle_MenuItemCollapse_Click(object sender, EventArgs e)
        {
            if (_clickedNode != null)
            {
                _clickedNode.Collapse(false);
                _clickedNode = null;
            }
        }

        void Handle_MenuItemExpand_Click(object sender, EventArgs e)
        {
            if (_clickedNode != null)
            {
                _clickedNode.ExpandAll();
                _clickedNode = null;
            }
        }
    }
}
