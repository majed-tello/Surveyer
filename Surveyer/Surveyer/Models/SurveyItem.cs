using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class SurveyItem
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Range(0,7)]
        public byte Type { get; set; }

        public string Text { get; set; }

        public bool Required { get; set; }

        public List<string> Answers { get; set; }

        public SurveyItem(byte type,string text, bool required, List<string> Answers)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Type = type;
            this.Text = text;
            this.Required = required;
            this.Answers = Answers;
        }
    }
}