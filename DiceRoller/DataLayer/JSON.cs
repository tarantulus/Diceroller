using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;

namespace DiceRoller.DataLayer
{
    public class JsonStore : DataStore
    {

        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public JsonStore(string name)
        {

            string root = AppDomain.CurrentDomain.BaseDirectory;
            this.path = string.Format("{0}{1}.txt", root, name);
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

        public object GetData(Type type)
        {

            return this.serializer.Deserialize(path, type);
        }

        public void Update(object data)
        {
            string json = this.serializer.Serialize(data);
            StreamWriter file = new StreamWriter(path, true);
            file.WriteLine(json);
            file.Close();
        }

        public bool Delete()
        {
            return true;
        }
    }
}