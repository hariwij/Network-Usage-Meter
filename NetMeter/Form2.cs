using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetMeter
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            comboBox1.DataSource = Frm.form.nu.GetNames();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Frm.form.data.devi = (uint)comboBox1.SelectedIndex;
            Frm.form.data.used = 0;
            Frm.form.nu.SelectDevice(Frm.form.data.devi);
            this.Hide();
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Frm.form.data.devi = (uint)comboBox1.SelectedIndex;
                Frm.form.data.used = 0;
                Frm.form.nu.SelectDevice(Frm.form.data.devi);
                this.Hide();
            }
        }
    }
    public static class Frm
    {
        public static Form1 form;
    }
}
