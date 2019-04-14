using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public abstract class SurveyItem
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public string Text { get; set; }

        public bool Required { get; set; }

        public SurveyItem()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}