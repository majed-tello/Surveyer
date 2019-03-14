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
        public List<T> GetData(Controller controller)
        {
            string filename = "UserDataFile.json";
            //if (typeof(T) is User)
            //    filename = "";
            var jsonfile = new StreamReader(controller.Server.MapPath("~/JsonData/" + filename));
            var jsondata = jsonfile.ReadToEnd();
            jsonfile.Close();
            return JsonConvert.DeserializeObject<List<T>>(jsondata);
        }

        public void SaveData(Controller controller, List<T> Data)
        {
            string filename = "";

            if (typeof(T) is User)
                filename = "";

            var jsondata = JsonConvert.SerializeObject(Data);
            System.IO.File.WriteAllText(controller.Server.MapPath("~/JsonData/" + filename), string.Empty);
            var jsonfile = new StreamWriter(controller.Server.MapPath("~/JsonData/" + filename));
            jsonfile.WriteLine(Data);
            jsonfile.Close();
        }

        public void AddItem(string path, T Item)
        {
            string filename = "UserDataFile.json";
            //if (typeof(T) is User)
            //    filename = "UserDataFile";

            var jsondata = JsonConvert.SerializeObject(Item);
            var jsonfile = new StreamWriter(path + filename,true);
            jsonfile.WriteLine(jsondata);
            jsonfile.Close();
        }
    }
}