using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Dodge
{
    class AirShip
    {
        private const int clientSizeX = 500;
        private const int clientSizeY = 500;
        private Bitmap airShip = Properties.Resources.airship;


        public float a_Width { get; set; }
        public float a_Height { get; set; }

        public AirShip()
        {
            a_Width = 225;
            a_Height = 225;
        }

        public void draw(Graphics g)
        {
            g.DrawImage(airShip, a_Width, a_Height);
        }

    }
}
