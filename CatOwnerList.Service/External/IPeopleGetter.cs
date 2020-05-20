using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatOwnerList.Services.External.Models;

namespace CatOwnerList.Services.External
{
    /// <summary>
    /// Represents a helper for getting data from a People (Web) Service
    /// </summary>
    public interface IPeopleGetter
    {
        /// <summary>
        /// Gets the data from the service
        /// </summary>
        /// <returns></returns>
        Task<List<PetOwner>> GetAsync();
    }
}
