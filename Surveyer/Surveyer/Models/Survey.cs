using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    enum SurveyType
    {
        FeedBack=0,
        Quiz=1
    }

    enum SurveyAllowAccess
    {
        Public = 0,
        Private =1
    }

    public class Survey
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Display(Name ="Surey Title"),Required]
        public string Title { get; set; }

        [Display(Name ="Survey Description"),DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ScaffoldColumn(false),Display(Name ="Publish Date")]
        public DateTime PublishDate { get; set; }

        public List<SurveyItem> SurveyItems { get; set; }

        [Display(Name = "Allow Access"), Range(0, 1)]
        public decimal AllowAccess { get; set; } = 1;

        [Display(Name = "Users Allowed Access")]
        public List<string> UsersAllowedAccess { get; set; }

        [Display(Name = "Survey Type"), Range(0, 1)]
        public decimal SurveyType { get; set; } = 0;

        [Display(Name ="Survey Color")]
        public Color SurveyColor { get; set; }

        public Survey()
        {
            this.Id = Guid.NewGuid().ToString();
            this.PublishDate = DateTime.Now;
        }
    }
}