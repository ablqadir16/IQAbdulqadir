using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    //Specification Class Created for UsersWithTypesAndBrandsSpecification
    public class UsersWithIDSpecification : BaseSpecifcation<User>
    {
        public UsersWithIDSpecification(UserSpecParams userParams) : base(x => 
            (string.IsNullOrEmpty(userParams.Search) || x.Name.ToLower().Contains(userParams.Search)) &&
            (!userParams.Id.HasValue || x.Id == userParams.Id)
        )
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);

            if (!string.IsNullOrEmpty(userParams.Sort))
            {
                switch (userParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public UsersWithIDSpecification(int id) : base(x => x.Id == id)
        {
        }
    }
}