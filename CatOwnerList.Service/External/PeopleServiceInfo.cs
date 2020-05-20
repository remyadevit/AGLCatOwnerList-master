using System;
using System.Collections.Generic;
using System.Text;

namespace CatOwnerList.Services.External
{
    /// <summary>
    /// A class containing details of a People (Web) Service
    /// </summary>
    public class AGLTestService
    {
        /// <summary>
        /// The base URL of the service
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// The endpoint for the people JSON data
        /// </summary>
        public string EndPoint { get; set; }
    }
}
