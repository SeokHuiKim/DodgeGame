using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dodge
{
    class Stone
    {
        private const int clientSizeX = 500;
        private const int clientSizeY = 500;

        private Bitmap stone = Properties.Resources.stone;
        private Random rand;
        private int[] num_rand = { 0, 485 };

        public int num_stone { get; set; }
        public int[] stoneX { get; set; }
        public int[] stoneY { get; set; }
        public List<Bitmap> _stone { get; set; }

        public Stone()
        {
            rand = new Random();
            num_stone = 30;
            _stone = new List<Bitmap>(num_stone);
            stoneX = new int[num_stone];
            stoneY = new int[num_stone];

            location();
        }

        public void location()
        {
            for (int i = 0; i < num_stone; i++)
            {
                stoneX[i] = rand.Next(0, clientSizeX);
                stoneY[i] = num_rand[rand.Next(0, num_rand.Length)];
                if (i % 2 == 0)
                {
                    stoneX[i] = num_rand[rand.Next(0, num_rand.Length)];
                    stoneY[i] = rand.Next(0, clientSizeY);
                }
                _stone.Add(stone);
            }
        }

        public void draw(Graphics g)
        {
            for (int i = 0; i < num_stone; i++)
            {
                g.DrawImage(_stone[i], stoneX[i], stoneY[i]);
            }
        }

    }
}

