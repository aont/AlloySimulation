using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Aont
{
    public partial class SettingsForm : Form
    {
        AlloyForm isingform;
        public SettingsForm(AlloyForm isingform)
        {

            InitializeComponent();
            this.isingform = isingform;
            button2_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isingform.J = double.Parse(this.textBox1.Text);
            isingform.T = double.Parse(this.textBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = isingform.J.ToString();
            this.textBox3.Text = isingform.T.ToString();
        }

    }
}