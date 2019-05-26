using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class SurveyResult
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [ScaffoldColumn(false)]
        public string SurveyId { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        [ScaffoldColumn(false), Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        public List<SurveyItemResult> surveyItemResults { get; set; }

        public SurveyResult()
        {
            this.Id = Guid.NewGuid().ToString();
            this.PublishDate = DateTime.Now;
            this.surveyItemResults = new List<SurveyItemResult>();
        }
    }
}