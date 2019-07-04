using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public enum SurveyType
    {
        FeedBack=0,
        Quiz=1
    }

    public enum SurveyAllowAccess
    {
        Aanonymous = 0,
        SpecificUsers = 1
    }
   public enum SurveyItemType
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

        [Required,Display(Name ="Title")]
        public string Title { get; set; }

        [Display(Name ="Description"),DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ScaffoldColumn(false),Display(Name ="Publish Date")]
        public DateTime PublishDate { get; set; }

        public List<SurveyItem> SurveyItems { get; set; }

        [Display(Name = "Allow Access"), Range(0, 1)]
        public byte AllowAccess { get; set; } = 0;

        [Display(Name = "Users Allowed Access")]
        public List<string> UsersAllowedAccess { get; set; }

        [Display(Name = "Type"), Range(0, 1)]
        public byte SurveyType { get; set; } = 0;

        [Display(Name = "Quiz Time By minutes")]
        public int? SurveyTime { get; set; }

        [Display(Name ="Color")]
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