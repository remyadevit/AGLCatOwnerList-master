namespace CatOwnerList.Services.External.Models
{
    /// <summary>
    /// Represents an individual pet model returned by the External People Service 
    /// </summary>
    public class Pet
    {
        /// <summary>
        /// The name of the pet
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of pet
        /// </summary>
        public string Type { get; set; }
    }
}
