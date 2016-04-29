using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testPic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.ImageLocation = "http://f.hiphotos.baidu.com/image/pic/item/1b4c510fd9f9d72a75301d09d62a2834349bbb49.jpg";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.pictureBox1.ImageLocation = "http://192.168.1.110:8084/ROOT/images/wolf.jpg";
        }
    }
}
