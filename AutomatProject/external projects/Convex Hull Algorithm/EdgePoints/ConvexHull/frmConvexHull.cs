/*
 About the author
 ****************
 * Phady McPhreaker 
 * B.Sc. Computer and Systems Engineering 
 * C# , C++ Software Engineer
 * MCTS in Developing Windows Base Application using .NET framewrok
 * ****************************************************************
 * e-mail :fady_aladdin@hotmail.com
 * location: Cairo
 * ****************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConvexHull
{
    public partial class frmConvexHull : Form
    {
        #region Form constructor
        public frmConvexHull()
        {
            InitializeComponent();
        }
        #endregion

        #region Globals
        Graphics graphics;
        List<SuperPoint> pointsList = new List<SuperPoint>();
        List<Segment> Segments = new List<Segment>();
        int pID = 0;
        List<List<SuperPoint>> renderingPoints = new List<List<SuperPoint>>();
        List<List<Segment>> renderingEdges = new List<List<Segment>>();
        List<PointF[]> renderingAreas = new List<PointF[]>();
        #endregion
        
        #region Event handlers
        private void frmConvexHull_MouseDown(object sender, MouseEventArgs e)
        {
            render();
            pID++;
            graphics = this.CreateGraphics();
            renderPoint(e.X, e.Y, Color.Lime);
            PointF p = new PointF(e.X, e.Y);
            SuperPoint sp = new SuperPoint(p, pID);
            pointsList.Add(sp);

        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            if (pointsList.Count > 0)
            {
                InitOrderdPoints();
                Compute();
                prepareNewConvexHull();

            }
            else
            {
                MessageBox.Show("Pick the hull points");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.Black);
            pointsList.Clear();
            Segments.Clear();
            renderingEdges.Clear();
            renderingPoints.Clear();
            renderingAreas.Clear();
        }

        private void frmConvexHull_Paint(object sender, PaintEventArgs e)
        {
            render();
        }

        #endregion

        #region Functions

        private void renderPoint(float x, float y, Color c)
        {
            Pen p = new Pen(c, 1.7f);
            graphics.DrawEllipse(p, x - 5, y - 5, 10, 10);
            graphics.DrawLine(p, x, y - 5, x, y - 5 - 2.5f);
            graphics.DrawLine(p, x - 5, y, x - 5 - 2.5f, y);
            graphics.DrawLine(p, x, y + 5, x, y + 5 + 2.5f);
            graphics.DrawLine(p, x + 5, y, x + 5 + 2.5f, y);
            graphics.FillEllipse(Brushes.Lime, x - 1.25f, y - 1.25f, 2.5f, 2.5f);
        }

        private void renderEdges(List<Segment> edges)
        {
            List<PointF> list = new List<PointF>();
            for (int i = 0; i < edges.Count; i++)
            {
                if (i == 0)
                    list.Add(edges[i].p);

                foreach (Segment s in edges)
                {
                    if (s.p == list[list.Count - 1])
                    {
                        list.Add(s.q);
                        break;
                    }
                }
            }

            Pen p = new Pen(Color.Fuchsia);
            if (checkBox1.Checked)
            {
                PointF[] Array = new PointF[list.Count];
                Array = list.ToArray();
                renderingAreas.Add(Array);

                graphics.FillPolygon(Brushes.MidnightBlue, Array);
                render();

                foreach (Segment opEdge in edges)
                {
                    graphics.DrawLine(p, opEdge.p, opEdge.q);
                }
            }

            else
            {
                foreach (Segment opEdge in edges)
                {
                    graphics.DrawLine(p, opEdge.p, opEdge.q);
                }
            }

            foreach (SuperPoint sp in pointsList)
            {
                renderPoint(sp.P.X, sp.P.Y, Color.Lime);
            }

        }

        private void render()
        {
            if (renderingEdges.Count > 0 || renderingPoints.Count > 0 || renderingAreas.Count > 0)
            {
                foreach (PointF[] pA in renderingAreas)
                {
                    graphics.FillPolygon(Brushes.MidnightBlue, pA);
                }

                foreach (List<SuperPoint> splist in renderingPoints)
                {
                    foreach (SuperPoint sp in splist)
                    {
                        renderPoint(sp.P.X, sp.P.Y, Color.Blue);

                    }
                }

                foreach (List<Segment> seglist in renderingEdges)
                {
                    foreach (Segment s in seglist)
                    {

                        graphics.DrawLine(new Pen(Color.LightSkyBlue), s.p, s.q);
                    }
                }
            }

            foreach (SuperPoint sp2 in pointsList)
            {
                renderPoint(sp2.P.X, sp2.P.Y, Color.Lime);
            }
        }

        private void prepareNewConvexHull()
        {
            pointsList.Clear();
            Segments.Clear();
        }

        private void Compute()
        {
            List<SuperPoint> ProcessingPoints = new List<SuperPoint>();
            int i = 0;
            int j = 0;
            for (i = 0; i < Segments.Count; )
            {
                //ProcessingPoints will be the points that are not in the current segment
                ProcessingPoints = new List<SuperPoint>(pointsList);

                for (j = 0; j < ProcessingPoints.Count; )
                {

                    if (Segments[i].contains(ProcessingPoints[j]))
                    {
                        ProcessingPoints.Remove(ProcessingPoints[j]);
                        j = 0;
                        continue;
                    }
                    j++;

                }

                if (!isEdge(ProcessingPoints, Segments[i]))
                {
                    Segments.Remove(Segments[i]);
                    i = 0;
                    continue;
                }
                else
                { i++; }
            }

            renderEdges(Segments);
            List<SuperPoint> superPointsList = new List<SuperPoint>(pointsList);
            List<Segment> segmentsList = new List<Segment>(Segments);
            renderingPoints.Add(superPointsList);
            renderingEdges.Add(segmentsList);

        }

        private bool isEdge(List<SuperPoint> processingPoints, Segment edge)
        {
            for (int k = 0; k < processingPoints.Count; k++)
            {
                if (isLeft(edge, processingPoints[k].P))
                {
                    return false;
                }
            }
            return true;
        }

        private bool isLeft(Segment segment, PointF r)
        {
            float D = 0;
            float px, py, qx, qy, rx, ry = 0;
            //The determinant
            // | 1 px py |
            // | 1 qx qy |
            // | 1 rx ry |
            //if the determinant result is positive then the point is left of the segment
            px = segment.p.X;
            py = segment.p.Y;
            qx = segment.q.X;
            qy = segment.q.Y;
            rx = r.X;
            ry = r.Y;

            D = ((qx * ry) - (qy * rx)) - (px * (ry - qy)) + (py * (rx - qx));

            if (D <= 0)
                return false;

            return true;
        }

        private void InitOrderdPoints()
        {
            //Initialize all possible segments from the picked points
            for (int i = 0; i < pointsList.Count; i++)
            {
                for (int j = 0; j < pointsList.Count; j++)
                {
                    if (i != j)
                    {
                        Segment op = new Segment();
                        PointF p1 = pointsList[i].P;
                        PointF p2 = pointsList[j].P;
                        op.p = p1;
                        op.q = p2;

                        Segments.Add(op);
                    }
                }
            }
        }

        #endregion
    }
}