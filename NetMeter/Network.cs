using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace NetMeter
{
    public class NetworkUsage
    {
        public NetworkUsage()
        {

        }

        private List<string> Names = new List<string>();
        private long S_sent;
        private long S_receive;

        private long P_sent;
        private long P_receive;

        private long T_sent;
        private long T_receive;

        private bool DChanged;
        private uint delay;
        private bool isInit;
        private uint device;

        Timer timer = new Timer();

        private NetworkInterface[] interfaces;

        public void SelectDevice(uint Index)
        {
            DChanged = true;
            S_sent = 0;
            S_receive = 0;
            P_sent = 0;
            P_receive = 0;
            T_sent = 0;
            T_receive = 0;
            device = Index;
        }

        public List<string> GetNames()
        {
            return Names;
        }
        public long GetTotalUsage()
        {
            return T_sent + T_receive;
        }
        public long GetTotalSent()
        {
            return T_sent;
        }
        public long GetTotalReceived()
        {
            return T_receive;
        }

        public bool Initialize(uint Time_ms)
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return false;
            else
            {
                isInit = true;
                delay = Time_ms;
                interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface ni in interfaces)
                {
                    Names.Add(ni.Description);
                }

                timer.Tick += new EventHandler(Timer_Tick);
                timer.Interval = (int)delay;
                timer.Enabled = true;
                timer.Start();

                return true;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            long tx = interfaces[(int)device].GetIPv4Statistics().BytesSent;
            long rx = interfaces[(int)device].GetIPv4Statistics().BytesReceived;
            if (tx < 0 || rx < 0) return;
            if (DChanged)
            {
                S_sent = tx;
                S_receive = rx;

                P_sent = tx;
                P_receive = rx;
                DChanged = false;
            }
            if (isInit)
            {
                if (P_sent < tx) // Data Sent
                {
                    var handler = Data_Sent;
                    handler?.Invoke(new DataSentArgs(tx - P_sent));
                }
                if (P_receive < rx) // Data Received
                {
                    var handler = Data_Received;
                    handler?.Invoke(new DataReceivedArgs(rx - P_receive));
                }
                if (P_sent + P_receive < tx + rx) // Data Used
                {
                    var handler = Data_Used;
                    handler?.Invoke(new DataUsedArgs(tx - P_sent, rx - P_receive));
                }
                var handlerT = Data_Used_Timer;
                handlerT?.Invoke(new DataUsedArgs(tx - P_sent, rx - P_receive));

                T_sent += tx - P_sent;
                T_receive += rx - P_receive;

                P_sent = tx;
                P_receive = rx;
            }
        }

        public event T_Tick Data_Used_Timer;
        public delegate void T_Tick(DataUsedArgs args);

        public event DataReceived Data_Received;
        public delegate void DataReceived(DataReceivedArgs args);
        public class DataReceivedArgs : EventArgs
        {
            public long Received;
            public DataReceivedArgs(long _Received)
            {
                Received = _Received;
            }
        }

        public event DataSent Data_Sent;
        public delegate void DataSent(DataSentArgs args);
        public class DataSentArgs : EventArgs
        {
            public long Sent;
            public DataSentArgs(long _Sent)
            {
                Sent = _Sent;
            }
        }

        public event DataUsed Data_Used;
        public delegate void DataUsed(DataUsedArgs args);
        public class DataUsedArgs : EventArgs
        {
            public long Sent;
            public long Received;
            public DataUsedArgs(long _Sent, long _Received)
            {
                Sent = _Sent;
                Received = _Received;
            }
        }
    }
}

