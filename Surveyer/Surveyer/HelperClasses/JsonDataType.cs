using Newtonsoft.Json;
using Surveyer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveyer.HelperClasses
{
    public class JsonDataType<T>
    {
        private string filename { get; set; }
        public JsonDataType(string filename)
        {
            this.filename = filename;
        }

        public List<T> GetData(Controller controller)
        {
            var jsonfile = new StreamReader(controller.Server.MapPath("~/JsonData/" + filename));
            var jsondata = jsonfile.ReadToEnd();
            jsonfile.Close();
            return JsonConvert.DeserializeObject<List<T>>(jsondata);
        }

        private void SaveData(Controller controller, List<T> Data)
        {
            var jsondata = JsonConvert.SerializeObject(Data);
            System.IO.File.WriteAllText(controller.Server.MapPath("~/JsonData/" + filename), string.Empty);
            var jsonfile = new StreamWriter(controller.Server.MapPath("~/JsonData/" + filename));
            jsonfile.WriteLine(jsondata);
            jsonfile.Close();
        }

        public void AddItem(Controller controlle, T Item)
        {
            var data = GetData(controlle);
            if (data == null)
                data = new List<T>();
            data.Add(Item);
            SaveData(controlle, data);
        }

        public void RemoveItem(Controller controlle, T Item)
        {
            var data = GetData(controlle);
            if (data == null)
                throw new Exception("the json file is olredy empty");
            data.Remove(Item);
            SaveData(controlle, data);
        }
    }
}