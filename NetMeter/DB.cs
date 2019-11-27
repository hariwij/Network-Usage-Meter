using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMeter
{
    [Serializable]
    public abstract class DB
    {
        public const string DBPath = "\\HariWij\\NetMeter\\DB\\";

        public string DBName;
        public ulong LastUpdate = 10U;
        public ulong LastSave = 10U;

        [NonSerialized]
        public static bool IsBusy = false;
        [NonSerialized]
        public string FileName;

        public abstract void CreateNewDatabase();

        public void SaveBinary()
        {
            while (IsBusy)
                System.Threading.Thread.Sleep(100);

            if (LastUpdate == LastSave)
                return;

            IsBusy = true;



            ulong changes = LastUpdate - LastSave;
            LastSave = LastUpdate;


            System.IO.FileStream fs = null;
            try
            {
                fs = new System.IO.FileStream(FileName, System.IO.FileMode.OpenOrCreate);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(fs, this);

                Log($"@DB Save succesful. {changes} changes in {DBName}");

                fs.Close();
            }
            catch (Exception ex)
            {
                LogErr(ex, $"@DB Failed to save {DBName}");

                try
                {
                    fs.Close();
                }
                catch (Exception)
                {
                }
            }

            IsBusy = false;
        }

        public DB LoadBinary(bool CreateNewIfNotFound = false)
        {
            while (IsBusy)
                System.Threading.Thread.Sleep(100);

            IsBusy = true;

            if (LastUpdate != LastSave) { IsBusy = false; SaveBinary(); }


            System.IO.FileStream FS = null;
            try
            {
                if (!File.Exists(FileName))
                {
                    Log($"Database {DBName} in {FileName} not found.");

                    if (CreateNewIfNotFound)
                    {
                        LastUpdate++;
                        CreateNewDatabase();
                        IsBusy = false;
                        SaveBinary();
                        IsBusy = true;
                        Log("@DB saved new empty DataBase");
                    }
                    else
                    {
                        IsBusy = false;
                        return null;
                    }
                }

                Log($"@DB {DBName} File opening...");

                FS = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                DB newDB = (DB)bf.Deserialize(FS);
                FS.Close();

                IsBusy = false;


                Log($"@DB {DBName} loading success.");
                newDB.FileName = FileName;

                return newDB;
            }
            catch (Exception ex)
            {
                LogErr(ex, $"@DB Failed to save {DBName}");

                try
                {
                    FS.Close();
                }
                catch (Exception)
                {
                }
            }

            IsBusy = false;
            return null;
        }

        public void ForceSave()
        {
            LastUpdate++;
            SaveBinary();
        }
        public DB DiscardChanges() { LastUpdate = LastSave; return LoadBinary(); }
        public void SetDataMember(string name, dynamic val)
        {
            // GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField, Type.DefaultBinder, this, val);

            GetType().GetField(name).SetValue(this, val);
        }
        public dynamic GetDataMember(string name)
        {
            return GetType().GetField(name).GetValue(this);
        }

        void Log(string s)
        {
            Console.WriteLine(s);
        }
        void LogErr(Exception e, string s)
        {
            Console.WriteLine(s);
        }
    }

    [Serializable]
    public class Data : DB
    {
        public Dictionary<string, bool> kill;
        public long used;
        public long sent;
        public long receive;
        public uint devi;
        public Point point;
        public override void CreateNewDatabase()
        {
            kill = new Dictionary<string, bool>();
            used = 0;
            sent = 0;
            receive = 0;
            devi = 0;
            point = new Point(10, 10);
        }
    }
}