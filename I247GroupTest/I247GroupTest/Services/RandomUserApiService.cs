using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using I247GroupTest.Interfaces;
using I247GroupTest.Models;

namespace I247GroupTest.Services
{
    public class RandomUserApiService : IRandomUserApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IRandomUserConfigWrapper _randomUserConfig;
        private const string randomUserEndPoint = "https://randomuser.me/api/?results=100";

        public RandomUserApiService(HttpClient httpClient, IRandomUserConfigWrapper randomUserConfig)
        {

            _httpClient = httpClient;
            _randomUserConfig = randomUserConfig;

        }

        public async Task<List<RequestedRandomUserDataModel>> GetRandomUserDataFromApi()
        {
            var limit = _randomUserConfig.GetValue();
            var requiredDataList = new List<RequestedRandomUserDataModel>();
            var response = await _httpClient.GetAsync(randomUserEndPoint);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var randomUserModelData = JsonSerializer.Deserialize<RandomUserModel>(responseData);

                if (randomUserModelData != null)
                {

                    foreach (var randomUser in randomUserModelData.results)
                    {
                        var requiredData = new RequestedRandomUserDataModel()
                        {
                            age = randomUser.dob.age,
                            country = randomUser.location.country,
                            first = randomUser.name.first,
                            last = randomUser.name.last,
                            title = randomUser.name.title,
                            latitude = randomUser.location.coordinates.latitude,
                            longitude = randomUser.location.coordinates.longitude

                        };

                        if (requiredDataList.Count >= limit)
                        {
                            break;
                        }

                        requiredDataList.Add(requiredData);
                    }

                }

                return requiredDataList;

            }
            else
            {
                throw new Exception("Failed to fetch data from the random user API");


            }
        }
    }

}