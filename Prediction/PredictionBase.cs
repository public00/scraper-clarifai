using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scraper.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Prediction
{
    public class PredictionBase
    {
        public List<string> keyWords = new List<string>();
        Predictions predictions = new Predictions();

        public List<string> _pictures;

        public PredictionBase(List<string> pictures)
        {
            this._pictures = pictures;
        }

        public async Task<Dictionary<string, double>> GetPredictionsForProperty()
        {
            var Client = new ClarifaiClient(Constants.ApiKey);

            var imagesResult = new List<IClarifaiInput>();
            foreach (var picture in _pictures)
                imagesResult.Add(new ClarifaiURLImage(picture));

            var response = await Client.Predict<Concept>(
                  Client.PublicModels.GeneralModel.ModelID,
                 imagesResult, null, null, null, 50)
              .ExecuteAsync();

            predictions = GetPredictionModel(response.RawBody);
            keyWords = new List<string>(GetKeyWords(predictions.Pictures));

            return GetVector(predictions);

        }

        public Dictionary<string, double> GetVector(Predictions predictions)
        {
            var result = new Dictionary<string, double>();

            for (int i = 0; i < predictions.Pictures.Count; i++)
            {
                for (var j = 0; j < keyWords.Count; j++)
                {
                    double tfIdf = 0;
                    ConceptModel model;
                    if (predictions.Pictures[i].PictureData.TryGetValue(keyWords[j], out model))
                    {
                        tfIdf = model.Value * GetIdf(keyWords[i]);
                        if (!result.ContainsKey(model.Name))
                        {
                            result.Add(model.Name, tfIdf);
                        }
                        else
                        {
                            result[model.Name] = (result[model.Name] + tfIdf) / 2;
                        }
                    }
                }
            }

            return result;
        }

        public double GetIdf(string term)
        {
            double df = 0.0;
            for (var i = 0; i < _pictures.Count; i++)
            {
                if (predictions.Pictures[i].PictureData.ContainsKey(term))
                {
                    df++;
                }
            }

            return Math.Log10(_pictures.Count / 1 + df);
        }

        public static Predictions GetPredictionModel(string rawBody)
        {
            var res = JObject.Parse(rawBody);

            var pictures = res["outputs"];
            var predictions = new Predictions
            {
                Pictures = new List<PictureModel>()
            };

            foreach (var picture in pictures)
            {
                var pictureObject = JObject.Parse(picture.ToString());

                var data = JsonConvert.DeserializeObject<dynamic>(pictureObject["data"].ToString());
                List<ConceptModel> concepts = new List<ConceptModel>();

                if (data["concepts"] != null)
                {
                    concepts = JsonConvert.DeserializeObject<List<ConceptModel>>(pictureObject["data"]["concepts"].ToString());
                }
                else
                {
                    concepts = new List<ConceptModel>();
                }

                var pictureModel = new PictureModel
                {
                    PictureData = new Dictionary<string, ConceptModel>()
                };

                foreach (var concept in concepts)
                {
                    pictureModel.PictureData.Add(concept.Name, concept);
                }

                predictions.Pictures.Add(pictureModel);
            }

            return predictions;

        }

        public static HashSet<string> GetKeyWords(List<PictureModel> pictures)
        {
            var result = new HashSet<string>();

            foreach (var picture in pictures)
            {
                foreach (var item in picture.PictureData)
                {
                    result.Add(item.Key);
                }
            }

            return result;
        }

    }
}
