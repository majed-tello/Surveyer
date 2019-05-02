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
    enum SurveyItemType
    {
        ShortAnswer=0,
        Pargrph=1,
        SingleChoice=2,
        MultipleChoice=3,
        Rating=4,
        FileUpload=5,
        Date=6,
        Time=7
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
        public byte AllowAccess { get; set; } = 0;

        [Display(Name = "Users Allowed Access")]
        public List<string> UsersAllowedAccess { get; set; }

        [Display(Name = "Survey Type"), Range(0, 1)]
        public byte SurveyType { get; set; } = 0;

        [Display(Name ="Survey Color")]
        public Color SurveyColor { get; set; }

        [ScaffoldColumn(false), Display(Name = "Publisher")]
        public string UserId { get; set; }

        public Survey()
        {
            this.Id = Guid.NewGuid().ToString();
            this.PublishDate = DateTime.Now;
            this.SurveyItems = new List<SurveyItem>();
        }
    }
}