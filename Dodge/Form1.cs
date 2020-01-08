using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dodge
{
    public partial class Form1 : Form
    {
        private enum PlayerState { MOVE_LEFT, MOVE_RIGHT, MOVE_UP, MOVE_DOWN, PAUSE }
        private enum State { READY, START, END }
        private Bitmap gameover = Properties.Resources.gameover;
        private AirShip airship;
        private Stone stone;
        private float playerSpeedX, playerSpeedY;
        private State state;
        private Random rand;
        private int[] stoneSpeedX, stoneSpeedY;
        private PlayerState playerState;

        public int min { get; set; }
        public int sec { get; set; }

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            panel1.Visible = false;
            watch.Visible = false;
            endText.Visible = false;
            start_button.Visible = true;
            ClientSize = new Size(500, 500);
            KeyPreview = true;
            airship = new AirShip();
            stone = new Stone();
            rand = new Random();
            state = State.READY;
            playerState = PlayerState.PAUSE;
            playerSpeedX = playerSpeedY = 0.5f;
            stoneSpeedX = new int[stone.num_stone];
            stoneSpeedY = new int[stone.num_stone];

            stoneSpeed();

        }

        public void stoneSpeed()
        {
            for (int i = 0; i < stone.num_stone; i++)
            {
                stoneSpeedX[i] = rand.Next(-10, 10);
                stoneSpeedY[i] = rand.Next(-10, 10);

                if (stoneSpeedX[i] == 0)
                    stoneSpeedX[i] = 1;
                else if (stoneSpeedY[i] == 0)
                    stoneSpeedY[i] = 1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (state == State.START)
            {
                for (int i = 0; i < stone.num_stone; i++)
                {
                    stone.stoneX[i] += stoneSpeedX[i];
                    stone.stoneY[i] += stoneSpeedY[i];

                    //돌이 벽과 충돌
                    if (stone.stoneX[i] < 0)
                        stoneSpeedX[i] = -stoneSpeedX[i];
                    else if (stone.stoneX[i] > ClientSize.Width - 10)
                        stoneSpeedX[i] = -stoneSpeedX[i];
                    if (stone.stoneY[i] < 0)
                        stoneSpeedY[i] = -stoneSpeedY[i];
                    else if (stone.stoneY[i] > ClientSize.Height - 10)
                        stoneSpeedY[i] = -stoneSpeedY[i];

                    // 우주선이 Form1밖으로 벗어나지 못하게
                    if (airship.a_Width < 0)
                        airship.a_Width = 0;
                    else if (airship.a_Width > ClientSize.Width - 25)
                        airship.a_Width = ClientSize.Width - 25;
                    if (airship.a_Height < 0)
                        airship.a_Height = 0;
                    else if (airship.a_Height > ClientSize.Height - 25)
                        airship.a_Height = ClientSize.Height - 25;

                    // 키 입력에 따른 우주선 움직임
                    if (playerState == PlayerState.MOVE_UP)
                        airship.a_Height -= playerSpeedY;
                    else if (playerState == PlayerState.MOVE_DOWN)
                        airship.a_Height += playerSpeedY;
                    else if (playerState == PlayerState.MOVE_LEFT)
                        airship.a_Width -= playerSpeedX;
                    else if (playerState == PlayerState.MOVE_RIGHT)
                        airship.a_Width += playerSpeedX;


                    // 돌과 플레이어가 충돌
                    if (stone.stoneX[i] + 5 > airship.a_Width && stone.stoneX[i] < airship.a_Width + 25
                        && stone.stoneY[i] + 5 > airship.a_Height && stone.stoneY[i] < airship.a_Height + 25)
                    {
                        endText.Visible = true;
                        state = State.END;
                        timer1.Stop();
                        timer2.Stop();
                    }
                    panel1.Invalidate();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            min++;
            if (min > 59)
            {
                min = 0;
                sec++;
                if (sec > 59)
                    sec = 0;
            }
            watch.Text = sec + ":" + min;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (state == State.START || state == State.END)
            {
                airship.draw(g);
                stone.draw(g);
            }
            if (state == State.END)
                g.DrawImage(gameover, 100, 100);
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            state = State.START;
            start_button.Visible = false;
            watch.Visible = true;
            panel1.Visible = true;
            timer1.Start();
            timer1.Interval = 50;
            timer2.Start();
            timer2.Interval = 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (state == State.START)
            {
                if (e.KeyCode == Keys.Up)
                    playerState = PlayerState.MOVE_UP;
                else if (e.KeyCode == Keys.Down)
                    playerState = PlayerState.MOVE_DOWN;
                else if (e.KeyCode == Keys.Left)
                    playerState = PlayerState.MOVE_LEFT;
                else if (e.KeyCode == Keys.Right)
                    playerState = PlayerState.MOVE_RIGHT;
                panel1.Invalidate();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            playerState = PlayerState.PAUSE;
            if (state == State.END)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    panel1.Visible = false;
                    watch.Visible = false;
                    endText.Visible = false;
                    start_button.Visible = true;
                    stoneSpeed();
                    stone.location();
                    sec = min = 0;
                    airship.a_Width = airship.a_Height = ClientSize.Width / 2;
                }
            }
        }
    }
}
