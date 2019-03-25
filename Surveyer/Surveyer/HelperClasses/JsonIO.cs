using Surveyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.HelperClasses
{
    public class JsonIO
    {
        public JsonDataType<User> Users { get; set; }
        public JsonIO()
        {
            Users = new JsonDataType<User>("UserDataFile.json");
        }
    }
}