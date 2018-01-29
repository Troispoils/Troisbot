using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TroisBot.Units;

namespace TroisBot.Radar
{
    class ManageGraph
    {
        /*//Information
        Bitmap bmp = new Bitmap(300, 300); // déclare une image bitmap
        Graphics g = Graphics.FromImage(bmp); // déclare un objet graphics attaché au bitmap

        g.DrawLine(new Pen(Color.Black), 0, 0, 299, 299); // dessine une ligne du point (0, 0) au point (299, 299)
        g.DrawEllipse(new Pen(Color.Red), 25, 0, 250, 300); // dessine une ellipse au point (25, 0) de grandeur (250x300)
        g.DrawArc(new Pen(Color.Green), 50, 50, 100, 100, 0, 90); // dessine un arc au point(50, 50) de grandeur (100x100) et de l'angle 0 à 90
 
        g.Dispose(); // libère l'objet graphics
        */

        float pointX = 0;
        float pointY = 0;

        Bitmap newmap = new Bitmap(260, 260);

        public ManageGraph()
        {

        }

        private float RadianToDegree(float Rotation)
        {
            return (float)(Rotation * (180 / Math.PI));
        }

        private void calculeMap(float playerX, float objectX, float playerY, float objectY)
        {
            pointX = ((playerX - objectX) * (float)2.5 + 260) / 2;
            pointY = ((playerY - objectY) * (float)2.5 + 260) / 2;
        }

        public Bitmap TracePlayerInMap(Bitmap map, Player player, Player curent, Color color, bool affname = false)
        {
            Graphics g = Graphics.FromImage(map);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            float Rotation = RadianToDegree(curent.Rotation);
            calculeMap(player.XPos, curent.XPos, player.YPos, curent.YPos);

            g.ResetTransform();
            g.TranslateTransform(-pointX, -pointY, System.Drawing.Drawing2D.MatrixOrder.Append);
            try
            {
                g.RotateTransform(-Rotation, System.Drawing.Drawing2D.MatrixOrder.Append);
            }
            catch (ArgumentException)
            {
            }
            g.TranslateTransform(pointX, pointY, System.Drawing.Drawing2D.MatrixOrder.Append);
            try
            {
                g.DrawEllipse(new Pen(color, 1.5F), pointX-4, pointY-4, 8, 8);
                g.DrawLine(new Pen(color, 1.5F), pointX, pointY, pointX, pointY - 5);
                if (affname)
                {
                    DrawText(map, curent.Name, Convert.ToInt32(pointX) - Convert.ToInt32((curent.Name.Length) * 2.5), Convert.ToInt32(pointY) + 8);
                }
            }
            catch (Exception ex)
            {

            }

            g.Dispose();

            return map;
        }

        private Bitmap DrawText(Bitmap img, String TextToDraw, int XPos, int YPos)
        {
            Graphics G = Graphics.FromImage(img);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Font DrawFont = new Font("Arial", 8);

            G.DrawString(TextToDraw, DrawFont, Brushes.Black, new Point(XPos, YPos));

            G.Dispose();
            DrawFont.Dispose();

            return img;
        }
    }
}
