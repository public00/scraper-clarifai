using IronWebScraper;
using System;
using System.Collections.Generic;

namespace Scraper
{
    public class PageScraper : WebScraper
    {
        public string _url;
        public Dictionary<string, object> resultItems = new Dictionary<string, object>(); // later in this object we will put more info.. for now only property url

        public PageScraper(string url)
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
            foreach (var Divs in response.Css(".has-ingrid.grid__item.js-default-ajax-scroll-target"))
            {
                var items = Divs.Css(".default--1-3.lap-wide--1-2.lap--1-2.palm-wide--1-1.palm--1-1.grid__item");
                foreach (var item in items)
                { // za svaki property
                    var propertyContainer = item.Css(".listing-item__image.listing-item__tabs-container.js-c-thumbnail-carousel");
                    if (propertyContainer.Length > 0) {
                        var attributeLink = string.Empty;
                        var goToLink = propertyContainer[0].Css("a")[0].Attributes.TryGetValue("href", out attributeLink);

                        var uri = new Uri(attributeLink);

                        resultItems.Add(uri.AbsolutePath.ToString(), attributeLink);
                    }
                }
            }

            //      Scrape(new ScrapedData() { { "Res", resultItems } } );

        }

    }
}
