using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User: BaseEntity
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public string FullAddress { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string ImagePath { get; set; }            
    }

} 