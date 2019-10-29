using Newtonsoft.Json;
using Scraper.DB;
using Scraper.Entities;
using Scraper.Prediction;
using Scraper.ScraperLogic;
using System.Threading.Tasks;

namespace Scraper
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var url = string.Empty;
            var pagesNumber = 8;
            for (int i = 1; i < pagesNumber; i++)
            {
                if (i != 1)
                {
                    url = Constants.Url + string.Format("/{0}-pg", i);
                }

                PageScraper scrape = new PageScraper(url);
                scrape.Start();

                foreach (var item in scrape.resultItems)
                {
                    var propertyScrape = new PropertyScraper(item.Value.ToString());
                    propertyScrape.Start();

                    var prediction = new PredictionBase(propertyScrape.Result.Pictures);
                    var propertyPredictionVector = await prediction.GetPredictionsForProperty();

                    var property = new Property
                    {
                        Area = propertyScrape.Result.Area,
                        Description = propertyScrape.Result.Description,
                        PropertyDescriptionTitle = propertyScrape.Result.PropertyDescriptionTitle,
                        Title = propertyScrape.Result.Title,
                        Pictures = JsonConvert.SerializeObject(propertyScrape.Result.Pictures),
                        Location = propertyScrape.Result.Location,
                        PropertyVector = JsonConvert.SerializeObject(propertyPredictionVector)
                    };

                    var propertyObject = new PropertyDbLogic();
                    propertyObject.SaveProperty(property);
                }

            }
        }
    }
}
