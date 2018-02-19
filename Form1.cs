using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

// http://csharphelper.com/blog/2016/07/animate-a-piston-driving-a-wheel-in-c/
namespace howto_piston
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }

        // Geometry.
        private float Ax = 10;
        private float Cx;
        private float Cy;
        private float connectingRodLength = 100;
        private float CylinderLength = 60;
        private float CrankRadius = 30;
        private float X, Xmin, Xmax, Dx = 6;
        private const float PistonLength = 30;
        private const float PistonRadius = 30;
        private const float ArmLength = 20; // bottom of piston
        private const float ArmRadius = 10; // bottom of piston
        private const float PistonCylinderGap = 4;
        private const int NumCylinders = 5;
        private Double[] velocity = new double[NumCylinders];
        private Font font = new Font("courier", 10);
        private Label[] labels = new Label[NumCylinders];

        /// <summary>
        /// angle between horizontal and line connecting center of crank to crank pin (radians)
        /// </summary>
        private double alphaRad = 0.0;

        // For displaying the current image.
        private Bitmap Picture = null;
        private Graphics Gr;

        // Find the points where the two circles intersect.
        private int FindCircleCircleIntersections(
            float cx0, float cy0, float radius0,
            float cx1, float cy1, float radius1,
            out PointF intersection1, out PointF intersection2)
        {
            // Find the distance between the centers.
            float dx = cx0 - cx1;
            float dy = cy0 - cy1;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            // See how many solutions there are.
            if (dist > radius0 + radius1)
            {
                // No solutions, the circles are too far apart.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else if (dist < Math.Abs(radius0 - radius1))
            {
                // No solutions, one circle contains the other.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else if ((dist == 0) && (radius0 == radius1))
            {
                // No solutions, the circles coincide.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else
            {
                // Find a and h.
                double a = (radius0 * radius0 -
                    radius1 * radius1 + dist * dist) / (2 * dist);
                double h = Math.Sqrt(radius0 * radius0 - a * a);

                // Find P2.
                double cx2 = cx0 + a * (cx1 - cx0) / dist;
                double cy2 = cy0 + a * (cy1 - cy0) / dist;

                // Get the points P3.
                intersection1 = new PointF(
                    (float)(cx2 + h * (cy1 - cy0) / dist),
                    (float)(cy2 - h * (cx1 - cx0) / dist));
                intersection2 = new PointF(
                    (float)(cx2 - h * (cy1 - cy0) / dist),
                    (float)(cy2 + h * (cx1 - cx0) / dist));

                // See if we have 1 or 2 solutions.
                if (dist == radius0 + radius1) return 1;
                return 2;
            }
        }

        // Move the piston.
        private void tmrMovePiston_Tick(object sender, EventArgs e)
        {
            X += Dx;

            if ((X < Xmin) || (X > Xmax))
            {
                Dx = -Dx;
                X += 2 * Dx;
            }

            DrawSystem();
            picCanvas.Refresh();
        }

        // Draw everything.
        private void DrawSystem()
        {
            Gr.Clear(this.BackColor);
            Gr.SmoothingMode = SmoothingMode.AntiAlias;

            GraphicsState initial_state = Gr.Save(); // like opengl push

            // translate to center
            Gr.TranslateTransform(Cx, Cy);

            // draw origin
            Gr.DrawLine(Pens.Red, new Point(-4, 0), new Point(+4, 0));
            Gr.DrawLine(Pens.Red, new Point(0, -4), new Point(0, +4));

            using (Pen custom_pen = new Pen(Color.Blue, 2))
            {
                // Draw the crankshaft
                RectangleF crankRect = new RectangleF(-CrankRadius, -CrankRadius, 2 * CrankRadius, 2 * CrankRadius);
                custom_pen.Color = Color.Blue;
                custom_pen.Width = 1;
                Gr.DrawEllipse(custom_pen, crankRect);
                custom_pen.Width = 2;

                DrawCylinders(custom_pen);

                DrawPiston(custom_pen, 1, 0.0, null, null);

                //
                // cylinder 1 (-90 degrees)
                //
                // Find the ends of the linkage.
                float linkage_x1 = X + PistonLength + ArmLength; // rod's first point - lies on end of the piston
                float linkage_y1 = 0.0f;// Cy;
                PointF pt1, pt2;
                FindCircleCircleIntersections(linkage_x1, linkage_y1, connectingRodLength, Cx, 0.0f, CrankRadius, out pt1, out pt2);
                float linkage_x2, linkage_y2; // rod's second end point - lies on wheel / crankshaft
                if (Dx > 0)
                {
                    linkage_x2 = pt1.X;
                    linkage_y2 = pt1.Y;
                }
                else
                {
                    linkage_x2 = pt2.X;
                    linkage_y2 = pt2.Y;
                }
               
                GraphicsState initial_state3 = Gr.Save(); // like opengl push
                if (Double.IsNaN(linkage_x2)) { linkage_x2 = 170.0f; } //todo
                if (Double.IsNaN(linkage_y2)) { linkage_y2 = 0.0f; }
                DrawLinkage(custom_pen, linkage_x1, linkage_x2, linkage_y1, linkage_y2);
                alphaRad = computeAlpha(linkage_x2, linkage_y2);
                double crankAngleRad = - alphaRad;
                velocity[0] = PistonVelocity(connectingRodLength, CrankRadius, crankAngleRad);

                DrawCrank(custom_pen, linkage_x2, linkage_y2);

                //
                // Cylinders 2 through NumCylinders
                //
                float cylinderAngleDeg = 0.0f;
                int ndx = 0;
                for (ndx = 1, cylinderAngleDeg = 360.0f / NumCylinders; cylinderAngleDeg < 360.0f; ndx++, cylinderAngleDeg += 360.0f / NumCylinders)
                {
                    DrawAngledPiston(custom_pen, ndx + 1, cylinderAngleDeg, linkage_x2, linkage_y2, out velocity[ndx]);
                }
                String msg3 = String.Format("vel = {0,4:f1} {1,4:f1} {2,4:f1} {3,4:f1} {4,4:f1}", // fieldwidth:precision
                     velocity[0], velocity[1], velocity[2], velocity[3], velocity[4]);
                Debug.WriteLine(msg3);

                DisplayVelocities();
            }
            Gr.Restore(initial_state); // like opengl pop 
        }

        private void DrawCylinders(Pen custom_pen)
        {
            for (float cylinderAngleDeg = 0.0f; cylinderAngleDeg < 360.0f; cylinderAngleDeg += 360.0f / NumCylinders)
            {
                GraphicsState initial_state = Gr.Save();
                Gr.RotateTransform(cylinderAngleDeg);

                float cylinder_length = CylinderLength + PistonLength + 2 * PistonCylinderGap;
                float cylinder_radius = PistonRadius + 2 * PistonCylinderGap;
                Gr.TranslateTransform(-(PistonLength + connectingRodLength + 2.0f * CrankRadius), 0.0f); // note no Ax here

                PointF[] cylinder_lines = new PointF[]
                {
                    new PointF(cylinder_length, -cylinder_radius / 2 + PistonCylinderGap),
                    new PointF(cylinder_length, -cylinder_radius / 2),
                    new PointF(0.0f, -cylinder_radius / 2),
                    new PointF(0.0f, +cylinder_radius / 2),
                    new PointF(cylinder_length, cylinder_radius / 2),
                    new PointF(cylinder_length, cylinder_radius / 2 - PistonCylinderGap),
                };
                Gr.DrawLines(custom_pen, cylinder_lines);
                Gr.Restore(initial_state);
            }
        }

        private void DrawPiston(Pen custom_pen, int pistonNum, Double cylinderAngleDeg, float? linkage_x1, float? linkage_y1)
        {
            GraphicsState initial_state = Gr.Save();
            Gr.RotateTransform((float)cylinderAngleDeg);

            // if cylinder #1
            if (!linkage_x1.HasValue)
            {
                Gr.TranslateTransform(-(Ax + PistonLength + connectingRodLength + 2.0f * CrankRadius), 0.0f);
                GraphicsPath piston_path = new GraphicsPath();
                PointF[] piston_lines = new PointF[]
                {
                    new PointF(X + PistonLength,  - PistonRadius / 2),
                    new PointF(X,  - PistonRadius / 2),
                    new PointF(X,  + PistonRadius / 2),
                    new PointF(X + PistonLength,  + PistonRadius / 2),
                    new PointF(X + PistonLength,  + ArmRadius / 2),
                    new PointF(X + PistonLength + ArmLength,  + ArmRadius / 2),
                    new PointF(X + PistonLength + ArmLength,  - ArmRadius / 2),
                    new PointF(X + PistonLength,  - ArmRadius / 2),
                };
                piston_path.AddPolygon(piston_lines);
                Gr.FillPath(Brushes.LightGreen, piston_path);
                custom_pen.Color = Color.Green;
                Gr.DrawPath(custom_pen, piston_path);

                // piston rings
                Gr.DrawLine(custom_pen, X + PistonCylinderGap, -PistonRadius / 2, X + PistonCylinderGap, +PistonRadius / 2);
                Gr.DrawLine(custom_pen, X + 2 * PistonCylinderGap, -PistonRadius / 2, X + 2 * PistonCylinderGap, +PistonRadius / 2);

                Gr.DrawString(String.Format("{0}", 1), font, Brushes.Black, new PointF(X + 10, (-PistonRadius / 2) + 6));
            }
            else
            {
                Double cylinderAngleRad = DegreeToRadian(cylinderAngleDeg);

                // calculate how far along X axis to draw piston
                float xc = +(Cx - linkage_x1.Value);
                float yc = -linkage_y1.Value;
                Double len = Math.Sqrt((xc * xc) + (yc * yc));
                len += (PistonLength + ArmLength + 3);
                Gr.TranslateTransform(-(float)len, 0.0f);

                GraphicsPath piston_path = new GraphicsPath();
                PointF[] piston_lines = new PointF[]
                {
                    new PointF(PistonLength,  - PistonRadius / 2),
                    new PointF(0.0f, -PistonRadius / 2),
                    new PointF(0.0f, +PistonRadius / 2),
                    new PointF(PistonLength,  + PistonRadius / 2),
                    new PointF(PistonLength,  + ArmRadius / 2),
                    new PointF(PistonLength + ArmLength,  + ArmRadius / 2),
                    new PointF(PistonLength + ArmLength,  - ArmRadius / 2),
                    new PointF(PistonLength,  - ArmRadius / 2),
                };
                piston_path.AddPolygon(piston_lines);
                Gr.FillPath(Brushes.LightGreen, piston_path);
                custom_pen.Color = Color.Green;
                Gr.DrawPath(custom_pen, piston_path);

                // piston rings
                Gr.DrawLine(custom_pen, PistonCylinderGap, -PistonRadius / 2, PistonCylinderGap, +PistonRadius / 2);
                Gr.DrawLine(custom_pen, 2 * PistonCylinderGap, -PistonRadius / 2, 2 * PistonCylinderGap, +PistonRadius / 2);

                Gr.DrawString(String.Format("{0}", pistonNum), font, Brushes.Black, new PointF(10, (-PistonRadius / 2) + 6));
            }

            Gr.Restore(initial_state);
        }

        private void DrawAngledPiston(Pen custom_pen, int pistonNumber, Double cylinderAngleDeg, float linkage_x2, float linkage_y2, out double vel)
        {
            Double cylinderAngleRad = DegreeToRadian(cylinderAngleDeg);

            // determine the piston pin position based on where the crankshaft is
            alphaRad = computeAlpha(linkage_x2, linkage_y2);
            double crankAngleRad = cylinderAngleRad - alphaRad;
            double position = PistonPinPosition(connectingRodLength, CrankRadius, crankAngleRad);
            vel = PistonVelocity(connectingRodLength, CrankRadius, crankAngleRad);

            float x1 = (float)(Math.Cos(cylinderAngleRad) * position);
            float y1 = (float)(Math.Sin(cylinderAngleRad) * position);
            float linkage_x1Cyl2 = Cx - x1;
            float linkage_y1Cyl2 = -y1;

            DrawPiston(custom_pen, pistonNumber, cylinderAngleDeg, linkage_x1Cyl2, linkage_y1Cyl2);
            DrawLinkage(custom_pen, linkage_x1Cyl2, linkage_x2, linkage_y1Cyl2, linkage_y2);
        }

        // utility method to factor out common code
        private Double computeAlpha(float linkage_x2, float linkage_y2)
        {
            float xc = +(Cx - linkage_x2);
            float yc = -linkage_y2;
            Double alpha = Math.Atan2(yc, xc);
            if (Double.IsNaN(alpha))
            {
                alpha = 0.0;
            }
            return alpha;
        }

    private void DrawLinkage(Pen custom_pen, float linkage_x1, float linkage_x2, float linkage_y1, float linkage_y2)
        {
            GraphicsState initial_state = Gr.Save();
            Gr.TranslateTransform(-(Ax + PistonLength + connectingRodLength + 2.0f * CrankRadius), 0.0f);

            custom_pen.Color = Color.Green;
            custom_pen.Width = 5;
            Gr.DrawLine(custom_pen, linkage_x1, linkage_y1, linkage_x2, linkage_y2);
            custom_pen.Color = Color.LightGreen;
            custom_pen.Width = 2;
            Gr.DrawLine(custom_pen, linkage_x1, linkage_y1, linkage_x2, linkage_y2);

            // Draw joints.
            Gr.FillEllipse(Brushes.Green, linkage_x1 - 4, linkage_y1 - 4, 8, 8);
            Gr.FillEllipse(Brushes.Green, linkage_x2 - 4, linkage_y2 - 4, 8, 8);

            Gr.Restore(initial_state);
        }

        private void DrawCrank(Pen custom_pen, float linkage_x2, float linkage_y2)
        {
            GraphicsState initial_state = Gr.Save();
            Gr.TranslateTransform(-(Ax + CrankRadius + connectingRodLength + 2.0f * CrankRadius), 0.0f);
            custom_pen.Color = Color.Gray;
            custom_pen.Width = 2;
            Gr.DrawLine(custom_pen, Cx, 0.0f, linkage_x2, linkage_y2);
            Gr.Restore(initial_state);
        }

        private void DisplayVelocities()
        {
            for (int piston = 0; piston < NumCylinders; piston++)
            {
                labels[piston].Text = String.Format("Piston {0} Velocity: {1,4:f1}", (piston + 1), velocity[piston]);
            }
        }

        // Start or stop the timer.
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Start")
            {
                btnStartStop.Text = "Stop";
                init();
                tmrMovePiston.Enabled = true;
            }
            else
            {
                btnStartStop.Text = "Start";
                tmrMovePiston.Enabled = false;
            }
        }

        private void init()
        {
            labels[0] = this.lblVelocity1;
            labels[1] = this.lblVelocity2;
            labels[2] = this.lblVelocity3;
            labels[3] = this.lblVelocity4;
            labels[4] = this.lblVelocity5;

            Cx = picCanvas.ClientSize.Width / 2;
            Cy = picCanvas.ClientSize.Height / 2;

            scrTimer.Value = tmrMovePiston.Interval;
            txtScrollTimer.Text = scrTimer.Value.ToString();

            Xmin = Ax + PistonCylinderGap;
            Xmax = Xmin + CylinderLength;
            X = Xmin;

            Picture = new Bitmap(
                picCanvas.ClientSize.Width,
                picCanvas.ClientSize.Height);
            Gr = Graphics.FromImage(Picture);
            picCanvas.Image = Picture;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                btnPause.Text = "Resume";
                tmrMovePiston.Enabled = false;
            }
            else
            {
                btnPause.Text = "Pause";
                tmrMovePiston.Enabled = true;
            }
        }

        private void scrTimer_Scroll(object sender, ScrollEventArgs e)
        {
            tmrMovePiston.Interval = scrTimer.Value;
            txtScrollTimer.Text = scrTimer.Value.ToString();
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private static double RadianToDegree(double angle)
        {
            return 180.0 * angle / Math.PI;
        }

        // returns distance from crank center to piston pin
        // https://en.wikipedia.org/wiki/Piston_motion_equations
        private double PistonPinPosition(double rodLength, double crankRadius, double crankAngleRad)
        {
            double c = crankRadius * Math.Cos(crankAngleRad);
            double s = crankRadius * Math.Sin(crankAngleRad);
            double z = (rodLength * rodLength) - (s * s);
            double pos = c + Math.Sqrt(z);
            return pos;
        }

        // returns piston velocity
        // https://en.wikipedia.org/wiki/Piston_motion_equations
        private double PistonVelocity(double rodLength, double crankRadius, double crankAngleRad)
        {
            double c = crankRadius * Math.Cos(crankAngleRad);
            double s = crankRadius * Math.Sin(crankAngleRad);
            double z = (rodLength * rodLength) - (s * s);
            double vel = -s - ((crankRadius* crankRadius) * c * s) / (Math.Sqrt(z));
            return vel;
        }
    }
}
