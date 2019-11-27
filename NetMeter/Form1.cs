using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NetMeter
{
    public partial class Form1 : Form
    {
        public static readonly string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public Data data;
        bool isF;
        public Form1()
        {
            InitializeComponent();

            AddStartup(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString() + ".exe", Application.ExecutablePath.ToString());

            CM.MenuItems.Add("Change Device", new EventHandler(C_Device_Click));
            CM.MenuItems.Add("Clear Usage", new EventHandler(C_Usage_Click));
            CM.MenuItems.Add("Process Killer", new EventHandler(Killer_Click));
            CM.MenuItems.Add("Exit", new EventHandler(Exit_Click));
            // CM.MenuItems.Add(new MenuItem{d)

            if (!Directory.Exists(BasePath + DB.DBPath)) Directory.CreateDirectory(BasePath + DB.DBPath);
            if (!File.Exists(BasePath + DB.DBPath + "DB.db")) isF = true;
            else isF = false;
            data = (Data)new Data() { DBName = "DB", FileName = BasePath + DB.DBPath + "DB.db" }.LoadBinary(isF);

            TxtTotal.ContextMenu = CM;
        }

        private void Killer_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.ShowDialog();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void C_Usage_Click(object sender, EventArgs e)
        {
            data.used = 0;
            TxtTotal.Text = "0 B";
        }

        private void C_Device_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        public NetworkUsage nu = new NetworkUsage();
        ContextMenu CM = new ContextMenu();

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TxtTotal.MouseDown += new MouseEventHandler(AppFormBase_MouseDown);
            this.TxtTotal.MouseMove += new MouseEventHandler(AppFormBase_MouseMove);
            this.TxtTotal.MouseUp += new MouseEventHandler(AppFormBase_MouseUp);

            nu.Initialize(1000);
            nu.SelectDevice((uint)data.devi);
            nu.Data_Used_Timer += Dused;
            this.Location = data.point;
        }

        private void Dused(NetworkUsage.DataUsedArgs args)
        {
            TxtTotal.Text = GetStr(data.used + nu.GetTotalUsage());
            TxtUse.Text = GetStr(args.Received + args.Sent);
            // Console.WriteLine($"Received : {args.Received / 1024}KB\t\t\tSent : {args.Sent / 1024}KB\t\t\t{nu.GetTotalUsage() / 1024 / 1024}MB");
        }

        string GetStr(long data)
        {
            float GB = (float)data / 1024 / 1024 / 1024;
            float MB = (float)data / 1024 / 1024;
            float KB = (float)data / 1024;
            if (GB > 1)
            {
                return GB.ToString("00.00") + " GB";
            }
            else if (MB > 1)
            {
                return MB.ToString("00.00") + " MB";
            }
            else if (KB > 1)
            {
                return KB.ToString("00.00") + " KB";
            }
            return data.ToString() + " B";
        }

        void AppFormBase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }

        void AppFormBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPoint == Point.Empty)
            {
                return;
            }
            Point location = new Point(
                this.Left + e.X - downPoint.X,
                this.Top + e.Y - downPoint.Y);
            this.Location = location;
        }

        void AppFormBase_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }

        public Point downPoint = Point.Empty;

        public static void AddStartup(string appName, string path)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (!key.GetValueNames().ToList().Contains(appName))
                    key.SetValue(appName, "\"" + path + "\"");
            }
        }

        public static void RemoveStartup(string appName)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (key.GetValueNames().ToList().Contains(appName))
                    key.DeleteValue(appName, false);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            data.point = this.Location;
            data.used = nu.GetTotalUsage();
            data.ForceSave();
        }

        private void TxtTotal_SizeChanged(object sender, EventArgs e)
        {
            TxtUse.Location = new Point(TxtTotal.Location.X + TxtTotal.Size.Width + 5, TxtTotal.Location.Y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var d = Process.GetProcesses();
            foreach (var s in d)
            {
                if (data.kill.ContainsKey(s.ProcessName))
                {
                    try
                    {
                        s.Kill();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }

}
