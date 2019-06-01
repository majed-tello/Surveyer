using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class Notefication
    {
        public string UserId { get; set; }

        public string Content { get; set; }

        public string Link { get; set; }

        public DateTime Date { get; set; }

        public bool IsReaded { get; set; }

        public Notefication()
        {
            this.Date = DateTime.Now;
            this.IsReaded = false;
        }

    }
}