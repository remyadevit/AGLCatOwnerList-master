using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CatOwnerList.Services.External.Models;

namespace CatOwnerList.Services.External
{
    /// <summary>
    /// A class for getting data from a People (Web) Service
    /// </summary>
    public class PeopleGetter : IPeopleGetter
    {
        private ILogger<PeopleGetter> logger;
        private static readonly HttpClient httpClient = new HttpClient();
        private AGLTestService aglTestService;

        public PeopleGetter(ILogger<PeopleGetter> logger,
            AGLTestService aglTestService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.aglTestService = aglTestService ?? throw new ArgumentNullException(nameof(aglTestService));
            httpClient.BaseAddress = new Uri(aglTestService.BaseUrl);
        }

        /// <summary>
        /// Gets the pet owner list from the people service
        /// </summary>
        /// <returns></returns>
        public async Task<List<PetOwner>> GetAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync(this.aglTestService.EndPoint);
                List<PetOwner> items = JsonConvert.DeserializeObject<List<PetOwner>>(json);
                return items;
            }
            catch
            {
                logger.LogError("Failed to get from external People Web Service");
                throw;
            }
        }
    }
}
