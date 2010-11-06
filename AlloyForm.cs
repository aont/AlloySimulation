using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aont
{
    public partial class AlloyForm : Form
    {
        int N, M;


        public const int Pixel = 2;

        int[,] SpinStates;

        Bitmap bitmap;
        static Random random = new Random();
        public AlloyForm(int N, int M)
        {
            InitializeComponent();

            this.SuspendLayout();

            this.N = N;
            this.M = M;
            this.bitmap = new Bitmap(N, M);
            //this.pictureBox1.Size = new Size(N * Pixel, M * Pixel);
            this.SpinStates = new int[N, M];
            for (int n = 0; n < N; n++)
            {
                for (int m = 0; m < M; m++)
                {
                    SetSpin(n, m, ((n + m) % 2) * 2 - 1);
                }
            }
            this.pictureBox1.Image = this.bitmap;


            this.ResumeLayout(false);
            this.PerformLayout();

            this.timer1.Enabled = true;
        }


        public double J = -1.5e-21;
        public double T = 200;
        const double Kb = 1.4e-23;
        public void Advance(int n1, int m1, int n2, int m2)
        {
            int Spin_nm = SpinStates[n1, m1];
            int Spin_nm_ = SpinStates[n2, m2];
            if (Spin_nm == Spin_nm_)
                return;
            else
            {
                //実は間違っている！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
                double P = GetEnergyFlip(n1, m1) + GetEnergyFlip(n2, m2);
                if (P < 0)
                {
                    SetSpin(n1, m1, -Spin_nm);
                    SetSpin(n2, m2, -Spin_nm_);
                }
                else
                {
                    P = Math.Exp(-P / (Kb * T));
                    if (P > random.NextDouble())
                    {
                        SetSpin(n1, m1, -Spin_nm);
                        SetSpin(n2, m2, -Spin_nm_);
                    }
                }
            }
        }
        double GetEnergyFlip(int n, int m)
        {
            int Spin_nm = SpinStates[n, m];
            double P = 0;
            P -= SpinStates[getPeriodic(n - 1, N), m];
            P -= SpinStates[getPeriodic(n + 1, N), m];
            P -= SpinStates[n, getPeriodic(m - 1, M)];
            P -= SpinStates[n, getPeriodic(m + 1, M)];
            return (J * P ) * -2 * Spin_nm;
        }
        int getPeriodic(int x, int X)
        {
            while (x < 0)
            {
                x += X;
            } while (x >= X)
            {
                x -= X;
            }
            return x;
        }
        void SetSpin(int n, int m, int States)
        {
            SpinStates[n, m] = States;
            switch (States)
            {
                case 1:
                    this.bitmap.SetPixel(m, n, Color.Black);
                    break;
                case -1:
                    this.bitmap.SetPixel(m, n, Color.White);
                    break;
                default:
                    throw new Exception();
            }
            //this.pictureBox1.Invalidate();
        }

        int Step = 5;
        public void Advance()
        {
            int n1 = random.Next(N);
            int m1 = random.Next(M);
            int n2, m2;
            if (false)
            {
                n2 = random.Next(N);
                m2 = random.Next(M);
            }
            else if (true)
            {
                n2 = getPeriodic(n1 + random.Next(2*Step+1)  - Step, N);
                m2 = getPeriodic(m1 + random.Next(2*Step+1)  - Step, M);
            }
            Advance(n1, m1, n2, m2);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i * i < 16 * M * N; i++)
            {
                this.Advance();
            }
            this.pictureBox1.Invalidate();
            int averagespin = 0;
            for (int n = 0; n < N; n++)
            {
                for (int m = 0; m < M; m++)
                {
                    if ((n + m) % 2 != 0)
                        averagespin += 2 * SpinStates[n, m];
                }
            }

            this.AverageSpintoolStripLabel.Text = ((double)averagespin / (N * M)).ToString();
        }


        private SettingsForm settingform;
        private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingform == null || settingform.IsDisposed)
            {
                settingform = new SettingsForm(this);
            }
            settingform.Show();

        }

    }
}