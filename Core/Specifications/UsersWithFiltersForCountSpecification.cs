using Core.Entities;

namespace Core.Specifications
{
    public class UsersWithFiltersForCountSpecification : BaseSpecifcation<User>
    {
        public UsersWithFiltersForCountSpecification(UserSpecParams userSpecParams)  : base(x => 
            (string.IsNullOrEmpty(userSpecParams.Search) || x.Name.ToLower().Contains(userSpecParams.Search)) &&
            (!userSpecParams.Id.HasValue || x.Id == userSpecParams.Id))
        {
            
        }
        
    }
}
