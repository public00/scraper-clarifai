using IronWebScraper;
using Scraper.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Scraper.ScraperLogic
{
    public class PropertyScraper : WebScraper
    {
        public string _url;

        public Result Result;

        public PropertyScraper(string url)
        {
            this._url = url;
        }

        public override void Init()
        {
            License.LicenseKey = "LicenseKey"; // Write License Key
            this.LoggingLevel = WebScraper.LogLevel.All; // All Events Are Logged
            HttpIdentity identity = new HttpIdentity();
            identity.HttpRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                  "Windows NT 5.2; .NET CLR 1.0.3705;)");

            foreach (var UA in IronWebScraper.CommonUserAgents.ChromeDesktopUserAgents)
            {
                Identities.Add(new HttpIdentity()
                {
                    UserAgent = UA,
                    UseCookies = true
                });
            }

            this.Request(this._url, Parse);
        }


        public override void Parse(Response response)
        {
            var propertyInfo = new Result();
            var pictures = new HashSet<string>();
            var items = response.Css(".carousel__expand-src.js-expand-src.js-image-src.u-out-from-viewport");
            foreach (var item in items)
            {
                var link = string.Empty;
                item.Attributes.TryGetValue("href", out link);
                pictures.Add(link);
            }

            var locality = response.Css(".locality")[0].InnerText;
            var region = response.Css(".region")[0].InnerText;
            var countryName = response.Css(".country-name")[0].InnerText;
            var propertyDescriptionTitle = response.Css(".prop-description__title")[0].OuterHtml;
            var propertyDescription = response.Css(".prop-description__comments-long.u-text-align-justify.u-margin-bottom")[0].OuterHtml;

            var area = response.Css(".listing-info__value");
            string areaAcres = string.Empty;

            foreach (var a in area)
            {
                if (a.TextContent.Contains("Acre"))
                {
                    areaAcres = a.TextContent;
                    break;
                }
            }

            propertyInfo.Pictures = pictures.ToList();
            propertyInfo.Area = areaAcres;
            propertyInfo.Location = string.Format("{0}, {1}, {2}", locality, region, countryName);
            propertyInfo.PropertyDescriptionTitle = propertyDescriptionTitle;
            propertyInfo.Description = propertyDescription;
            propertyInfo.Title = locality;

            Result = new Result(propertyInfo);
        }
    }

    
}
