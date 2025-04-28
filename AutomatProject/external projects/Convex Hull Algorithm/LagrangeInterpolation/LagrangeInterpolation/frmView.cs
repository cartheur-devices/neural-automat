//*************************************************
//Author: Fady Aladdin C# , C++  Software Developer
//Cairo ,Egypt 
//*************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LagrangeInterpolation
{
    public partial class frmView : Form
    {

        #region Class Constructor
        public frmView()
        {
            InitializeComponent();
        }
        #endregion

        #region Structurs
        private struct Operators
        {
            public List<float> Operatos;

        }

        private struct LineC
        {
            public PointF sPoint;
            public PointF ePoint;
        }
        #endregion

        #region Globals
        private List<PointF> points = new List<PointF>();
        Graphics g;
        //Define a List of Operators (to hold a list of Lagrange operators in each od it's elements)
        List<Operators> OpList;//= new List<Operators>();
        //A list of X coordinates which we will compute the Lagrange operators according to and they will be
        //the X coordinates of the fitted curve.
        //A list of Y coordinates which is computed according to Lagrange operators and they will be 
        //the Y coordiantes of the fitted curve.
        List<float> Xs;//= new List<float>();
        List<float> Ys;//= new List<float>();
        bool curveRendered = false;
        List<LineC> lineList = new List<LineC>();
        List<PointF> intersectedPoints = new List<PointF>();


        #endregion

        #region Event Handlers

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            g = pictureBox.CreateGraphics();
            PointF p = new PointF(e.X, e.Y);
            points.Add(p);
            Brush b;
            b = Brushes.Yellow;
            PointF t = new PointF(p.X - 8, p.Y - 25);
            g.DrawString(".", new Font("Bold", 20), b, t);
            renderCircules(points);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Text = e.X.ToString() + "," + e.Y.ToString() + "  Lagrange Interpolation Curve Fitting C# Phady McPhreaker";
            if (chkGrid.Checked)
                renderGrid();
            if (curveRendered)
                renderCurve();

        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (chkGrid.Checked)
                renderGrid();
            if (curveRendered)
                renderCurve();
        }

        private void btnFit_Click(object sender, EventArgs e)
        {
            btnFit.Enabled = false;
            fitLagrange(points);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            points.Clear();
            curveRendered = false;
            if (chkGrid.Checked)
                renderGrid();
            btnFit.Enabled = true;
        }

        private void chkGrid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGrid.Checked) { renderGrid(); }
            else { g.Clear(Color.Black); }
            if (curveRendered) renderCurve();
        }

        private void frmView_Load(object sender, EventArgs e)
        {
            if (chkGrid.Checked)
            {
                renderGrid();
            }
        }

        #endregion

        #region Functions

        private void fitLagrange(List<PointF> pointList)
        {
            OpList = new List<Operators>();
            Xs = new List<float>();
            Ys = new List<float>();

            try
            {
                if (pointList.Count > 0)
                {
                    //compute lagrange operator for each X coordinate
                    for (int x = 1; x < 2000; x++)
                    {
                        //list of float to hold the Lagrange operators
                        List<float> L = new List<float>();
                        //Init the list with 1's
                        for (int i = 0; i < pointList.Count; i++)
                        {
                            L.Add(1);
                        }
                        for (int i = 0; i < L.Count; i++)
                        {
                            for (int k = 0; k < pointList.Count; k++)
                            {
                                if (i != k)
                                 L[i] *= (float)(x - pointList[k].X) / (pointList[i].X - pointList[k].X);
                            }
                        }
                        Operators o = new Operators();
                        o.Operatos = L;
                        OpList.Add(o);
                        Xs.Add(x);

                    }

                    //Computing the Polynomial P(x) which is y in our curve
                    foreach (Operators O in OpList)
                    {
                        float y = 0;
                        for (int i = 0; i < pointList.Count; i++)
                        {
                            y += O.Operatos[i] * pointList[i].Y;
                        }

                        Ys.Add(y);
                    }

                    //Drawing the curve in the simplest way
                    renderCurve();
                    
                }
                else
                {
                    if (MessageBox.Show("Pick some points, asshole.", "Lagrange curve fitting", MessageBoxButtons.OK,MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        btnFit.Enabled = true;
                        if (chkGrid.Checked)
                        renderGrid();
                    }
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        private void renderCircules(List<PointF> centerPoints)
        {
            foreach (PointF p in centerPoints)
            {
                g.DrawEllipse(new Pen(Color.Lime), p.X -6, p.Y - 6, 10, 10);
            }
            Brush b;
            b = Brushes.Yellow;
            foreach (PointF p in centerPoints)
            {
                PointF t = new PointF(p.X - 8, p.Y - 25);
                g.DrawString(".", new Font("Bold", 20), b, t);
            }
            
        }

        private void renderCurve()
        {
            List<PointF> Curve = new List<PointF>();
            for (int i = 0; i < Xs.Count; i++)
            {
                PointF p = new PointF(Xs[i], Ys[i]);
                Curve.Add(p);

            }
            int index = Curve.Count;
            PointF[] CurveArray = new PointF[index];
            CurveArray = Curve.ToArray();
            g.DrawLines(new Pen(Color.Fuchsia), CurveArray);
            renderCircules(points);
            //points.Clear();
            curveRendered = true;
        }

        private void renderGrid()
        {
            
            g = pictureBox.CreateGraphics();
            Color color = Color.DarkGreen;
            for (int x = 0; x <= pictureBox.Width; x = x + 20)
            {
                PointF p1 = new PointF(x, 0);
                PointF p2 = new PointF(x, Height);
                g.DrawLine(new Pen(color), p1, p2);
                LineC l = new LineC();
                l.sPoint = p1;
                l.ePoint = p2;
                lineList.Add(l);
            }

            for (int y = 0; y <= pictureBox.Height; y = y + 20)
            {
                PointF p1 = new PointF(0, y);
                PointF p2 = new PointF(Width, y);
                g.DrawLine(new Pen(color), p1, p2);
                LineC l = new LineC();
                l.sPoint = p1;
                l.ePoint = p2;
                lineList.Add(l);
            }


        }

        private void getLinesIntersections()
        {

            for (int i = 0; i < lineList.Count; i++)
            {

                for (int j = 0; j < lineList.Count; j++)
                {
                    if (i != j)
                    {
                        PointF p = new PointF();
                        //p = Intersection(lineList[i].sPoint, lineList[i].ePoint, lineList[j].sPoint, lineList[j].ePoint);
                        if (!intersectedPoints.Contains(p))
                        { intersectedPoints.Add(p); }
                    }
                }
            
            }

        }

        #endregion

    }
}