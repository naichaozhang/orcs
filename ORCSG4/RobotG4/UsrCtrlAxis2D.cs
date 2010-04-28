/////////////////////////////////////////////////////////////
//
//  Filename: UsrCtrlAxis2D.cs
//  Author:   Travis Feirtag
//  Date:     05/12/2006 11:30:36
//  CLR ver:  2.0.50727.42
//  Project:  Accelerometer01
//
//  Copyright � 2006 Feirtech Inc.  All rights reserved.
//�
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR F  ESS FOR A
//  PARTICULAR PURPOSE.
////////////////////////////////////////////////////////////

namespace Robot
{
    #region Using Statements
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Data;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class UsrCtrlAxis2D : UserControl
    {
        #region Fields
        private Pen axisPen = new Pen(Color.Red, 1.0f);
        private Pen objectPen = new Pen(Color.Green, 2.0f);
        private float fOriginX = 0;
        private float fOriginY = 0;
        private float fMinX = 70; // Close to experimental values
        private float fMaxX = 215;
        private float fMinY = 70; // Close to experimental values
        private float fMaxY = 215;
        private float fMinZ = 70; // Close to experimental values
        private float fMaxZ = 215;
        private float fAngle = 0;

        private PointF[] ptsObject = new PointF[5];
        private PointF[] ptsModObject = new PointF[5];

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public UsrCtrlAxis2D()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current angle
        /// </summary>
        public float Angle
        {
            get
            {
                return this.fAngle;
            }
        }
        #endregion

        #region Dispose Method
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
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UsrCtrlAxis2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UsrCtrlAxis2D";
            this.Size = new System.Drawing.Size(350, 350);
            this.ResumeLayout(false);

        }
        #endregion

        #region OnPaint Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paintEventArgs"></param>
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            // Clear background
            paintEventArgs.Graphics.Clear(this.BackColor);
            paintEventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw X & Y axes
            paintEventArgs.Graphics.DrawLine(this.axisPen, this.fOriginX, 0, this.fOriginX, this.ClientSize.Height);
            paintEventArgs.Graphics.DrawLine(this.axisPen, 0, this.fOriginY, this.ClientSize.Width, this.fOriginY);

            // Draw object
            paintEventArgs.Graphics.DrawLines(this.objectPen, this.ptsModObject);
        }
        #endregion

        #region OnSizeChanged Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        protected override void OnSizeChanged(EventArgs eventArgs)
        {
            float fSpaceX = 10;
            float fSpaceY = 10;

            this.fOriginX = this.ClientSize.Width / 2;
            this.fOriginY = this.ClientSize.Height / 2;

            this.ptsObject[0] = new PointF(this.fOriginX - fSpaceX, fSpaceY);
            this.ptsObject[1] = new PointF(this.fOriginX + fSpaceX, fSpaceY);
            this.ptsObject[2] = new PointF(this.fOriginX + fSpaceX, this.ClientSize.Height - fSpaceY);
            this.ptsObject[3] = new PointF(this.fOriginX - fSpaceX, this.ClientSize.Height - fSpaceY);
            this.ptsObject[4] = new PointF(this.fOriginX - fSpaceX, fSpaceY);

            base.OnSizeChanged(eventArgs);
        }
        #endregion

        #region SetCurrentValue Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fCurrentX"></param>
        public void SetCurrentValue(float fCurrent)
        {
            
            if(Robot1.radio == 1) ///XXXXX
            {
            if (fCurrent < fMinX)
                fMinX = fCurrent;

            if (fCurrent > this.fMaxX)
                fMaxX = fCurrent;

            // Copy original points
            for (int i = 0; i < 5; i++)
                this.ptsModObject[i] = this.ptsObject[i];

            // Do some calculations
            // Minimum value will be 0 degrees
            // Maximum value will be 180 degrees


            float fUnitX = 180 / (fMaxX - fMinX);
            this.fAngle = ((fCurrent - this.fMinX) * fUnitX);
            
            Robot1.fAnglex0 = fAngle;
            }

            if (Robot1.radio == 2) //YYYY
            {
                if (fCurrent < this.fMinY)
                    this.fMinY = fCurrent;

                if (fCurrent > this.fMaxY)
                    this.fMaxY = fCurrent;

                // Copy original points
                for (int i = 0; i < 5; i++)
                    this.ptsModObject[i] = this.ptsObject[i];

                // Do some calculations
                // Minimum value will be 0 degrees
                // Maximum value will be 180 degrees


                float fUnitY = 180 / (this.fMaxY - this.fMinY);
                this.fAngle = ((fCurrent - this.fMinY) * fUnitY);
                Robot1.fAngley0 = fAngle;
            }

            if (Robot1.radio == 3) ///Z
            {
                if (fCurrent < this.fMinZ)
                    this.fMinZ = fCurrent;

                if (fCurrent > this.fMaxZ)
                    this.fMaxZ = fCurrent;

                // Copy original points
                for (int i = 0; i < 5; i++)
                    this.ptsModObject[i] = this.ptsObject[i];

                // Do some calculations
                // Minimum value will be 0 degrees
                // Maximum value will be 180 degrees


                float fUnitZ = 180 / (this.fMaxZ - this.fMinZ);
                this.fAngle = ((fCurrent - this.fMinZ) * fUnitZ);
                Robot1.fAnglez0 = fAngle;
            }


            // Create a matrix and scale it.
            Matrix myMatrix = new Matrix();
            myMatrix.RotateAt(this.fAngle, new PointF(this.fOriginX, this.fOriginY));
            myMatrix.TransformPoints(this.ptsModObject);

            // Refresh
            Thread.Sleep(10);
            this.Refresh();
        }
        #endregion
    } // End of UsrCtrlAxis2D class
} // End of namespace