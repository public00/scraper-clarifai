using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper.DTO
{
    public class Predictions
    {
        public List<PictureModel> Pictures { get; set; }
    }

    public class PictureModel
    {
        public Dictionary<string, ConceptModel> PictureData { get; set; }
        //public List<ConceptModel> Data { get; set; }
    }
    public class ConceptModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

    }
}
