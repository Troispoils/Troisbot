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

        float localPlayerX = 0;
        float localPlayerY = 0;

        Bitmap newmap = new Bitmap(260, 260);

        public ManageGraph()
        {

        }

        public void majGraphxy(Player player)
        {
            localPlayerX = player.XPos;
            localPlayerY = player.YPos;
        }

        public Bitmap TracePlayerInMap(Bitmap map, Player player, Player curent)
        {
            majGraphxy(player);

            Graphics g = Graphics.FromImage(map);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (curent.Type == 3)
            {
                if(!curent.targeted)
                    g.DrawEllipse(new Pen(Color.Red, 2F), ((player.XPos - curent.XPos) * 2 + 260) / 2, ((player.YPos - curent.YPos) * 2 + 260) / 2, 2, 2);
                else
                    g.DrawEllipse(new Pen(Color.Pink, 2F), ((player.XPos - curent.XPos) * 2 + 260) / 2, ((player.YPos - curent.YPos) * 2 + 260) / 2, 2, 2);
            }
            else if (curent.Type == 4)
            {
                if (curent.Name == player.Name)
                    g.DrawEllipse(new Pen(Color.Blue, 2F), ((player.XPos - curent.XPos) * 2 + 260) / 2, ((player.YPos - curent.YPos) * 2 + 260) / 2, 2, 2);
                else
                    g.DrawEllipse(new Pen(Color.Green, 2F), ((player.XPos - curent.XPos) * 2 + 260) / 2, ((player.YPos - curent.YPos) * 2 + 260) / 2, 2, 2);
            }

            g.Dispose();

            return map;
        }
    }
}
