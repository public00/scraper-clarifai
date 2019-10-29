using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper.DTO
{
    public class Result
    {
        public Result(Result result)
        {
            this.Area = result.Area;
            this.Description = result.Description;
            this.Location = result.Location;
            this.Title = result.Title;
            this.PropertyDescriptionTitle = result.PropertyDescriptionTitle;
            this.Pictures = result.Pictures;
        }

        public Result() { }

        public string Id { get; set; }

        public string Title { get; set; }

        public string PropertyDescriptionTitle { get; set; }

        public string Description { get; set; }

        public List<string> Pictures { get; set; }

        public string Location { get; set; }

        public string Area { get; set; }

    }
}
