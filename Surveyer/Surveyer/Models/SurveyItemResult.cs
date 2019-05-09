using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class SurveyItemResult
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Range(0, 7)]
        public byte Type { get; set; }

        public object Value { get; set; }
    }
}