using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class SingleChoiceItem : SurveyItem
    {
        public List<Chice> Chices { get; set; }
    }
}