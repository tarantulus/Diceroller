using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace DiceRoller.DataLayer
{
    public class JsonStore : DataStore
    {

        public JsonStore(string name)
        {

            string root = AppDomain.CurrentDomain.BaseDirectory;
            this.path = string.Format("{0}{1}.txt", root, name);
            if (!Exists())
            {
                Create();
            }
        }

        public string path { get; set; }

        public bool Exists()
        {
            return File.Exists(path);
        }

        public void Create()
        {
            StreamWriter file = new StreamWriter(path);
            file.Close();
        }

        public object GetData<T>()
        {
            StreamReader file = new StreamReader(path);
            string text = file.ReadToEnd();
            file.Close();
            return JsonConvert.DeserializeObject<T>(text);
            
        }

        public void Update<T>(object data)
        {
            
            string json = JsonConvert.SerializeObject(data);
            StreamWriter file = new StreamWriter(path, false);
            file.WriteLine(json);
            file.Close();
        }

        public bool Delete()
        {
            return true;
        }
    }
}