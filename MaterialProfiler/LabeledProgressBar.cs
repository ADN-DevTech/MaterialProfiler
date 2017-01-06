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
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    public class LabeledProgressBar : ProgressBar
    {
        private string labelText;

        public string LabelText
        {
            get { return labelText; }
            set { labelText = value; }
        }

        public LabeledProgressBar()
            : base()
        {

        }

        public LabeledProgressBar(double value)
            : base()
        {
            LabelText = value.ToString("F2") + "%";

            Value = (int)value;

            Minimum = 0;
            Maximum = 100;
            Style = ProgressBarStyle.Continuous;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e) 
        {
            base.OnPaint(e);

            using (Graphics gr = this.CreateGraphics())
            {
                string str = LabelText;

                //LinearGradientBrush brBG = new LinearGradientBrush(
                //    e.ClipRectangle,
                //    Color.GreenYellow,
                //    Color.Green,
                //    LinearGradientMode.Horizontal);

                SolidBrush brBG = new SolidBrush(Color.Turquoise);

                e.Graphics.FillRectangle(brBG,
                    e.ClipRectangle.X,
                    e.ClipRectangle.Y,
                    e.ClipRectangle.Width * this.Value / this.Maximum,
                    e.ClipRectangle.Height);

                //Font = new Font(Font.Name, Font.Size, FontStyle.Bold);

                gr.DrawString(str, 
                    Font, 
                    Brushes.Black,
                    new PointF(this.Width / 2 - (gr.MeasureString(str, SystemFonts.DefaultFont).Width / 2.0F),
                    this.Height / 2 - (gr.MeasureString(str, SystemFonts.DefaultFont).Height / 2.0F)));
            }
        }
    } 


}
