using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetMeter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Frm.form = new Form1();
            Application.Run(Frm.form);
        }
    }
}
