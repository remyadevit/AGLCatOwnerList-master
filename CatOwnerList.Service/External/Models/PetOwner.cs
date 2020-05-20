using System.Collections.Generic;

namespace CatOwnerList.Services.External.Models
{
    /// <summary>
    /// Represents the pet owner model returned by the External People Service 
    /// </summary>
    public class PetOwner
    {
        /// <summary>
        /// Name of the pet owner
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gender of the pet owner
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Age of the pet owner
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// A list of all pets the owner has
        /// </summary>
        public List<Pet> Pets { get; set; }
    }
}
