using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 俄罗斯方块
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Russia MyRussia = new Russia();
        Russia TemRussia = new Russia();
        public static int CakeNO = 0;
        public static bool become = false;
        public static bool isbegin = false;
        public bool ispause = true;
        public Timer timer = new Timer();

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
