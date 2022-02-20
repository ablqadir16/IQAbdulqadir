using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class UserUrlResolver : IValueResolver<User, UserDto, string>
    {
        private readonly IConfiguration _config;
        public UserUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImagePath)) 
            {
                return _config["ApiUrl"] + source.ImagePath;
            }
            return null;
        }   
    }
}