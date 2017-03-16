using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HqClient
{
    public partial class Form1 : Form
    {
        public Form1(String name)
        {
            InitializeComponent();
            label1.Text = "欢迎你：" + name;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
