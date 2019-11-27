using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetMeter
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            var d = Process.GetProcesses();
            foreach (var s in d)
            {
                checkedListBox1.Items.Add(s.ProcessName, false);
            }
            foreach (var s in Frm.form.data.kill)
            {
                checkedListBox2.Items.Add(s.Key, s.Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    if (!Frm.form.data.kill.ContainsKey(checkedListBox1.Items[i].ToString()))
                        Frm.form.data.kill.Add(checkedListBox1.Items[i].ToString(), true);
                }
            }
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                if (!checkedListBox2.GetItemChecked(i))
                {
                    if (Frm.form.data.kill.ContainsKey(checkedListBox2.Items[i].ToString()))
                        Frm.form.data.kill.Remove(checkedListBox2.Items[i].ToString());
                }
            }
            this.Dispose();
        }
    }
}
