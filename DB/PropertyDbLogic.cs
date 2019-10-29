using Scraper.Entities;
using System;

namespace Scraper.DB
{
    public class PropertyDbLogic
    {
        public void SaveProperty(Property property) {

            using (var context = new ScraperContext())
            {
                context.Add(property);
                context.SaveChanges();
            }

        }
    }
}
