using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;
using Infrastructure;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {     
        private readonly IGenericRepository<User> _usersRepo;
        private readonly IMapper _mapper;
        private readonly IQADbContext _db;

        public UsersController(IGenericRepository<User> usersRepo,
            IMapper mapper,IQADbContext db)
        {
            _mapper = mapper;
            _usersRepo = usersRepo;
            _db=db;
        }

        
        //Enable for Caching
        //[Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<UserDto>>> Getusers(
            [FromQuery] UserSpecParams userParams)
        {
            var spec = new UsersWithIDSpecification(userParams);
            var countSpec = new UsersWithFiltersForCountSpecification(userParams);

            var totalItems = await _usersRepo.CountAsync(countSpec);

            var users = await _usersRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<UserDto>>(users);

            return Ok(new Pagination<UserDto>(userParams.PageIndex,
                userParams.PageSize, totalItems, data));
        }


        //Enable for Caching
        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var spec = new UsersWithIDSpecification(id);

            var User = await _usersRepo.GetEntityWithSpec(spec);

            if (User == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<UserDto>(User);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Create(User user)
        {
            if (user!=null)
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }
            return Ok(user);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Delete(User user)
        {
            _db.Remove(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<User>> Update(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }

        //Image Upload
        //public HttpResponseMessage UploadFiles()
        //{
        //    //Create the Directory.
        //    string path = HttpContext.Current.Server.MapPath("~/Uploads/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    //Fetch the File.
        //    HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

        //    //Fetch the File Name.
        //    string fileName = Path.GetFileName(postedFile.FileName);

        //    //Save the File.
        //    postedFile.SaveAs(path + fileName);

        //    //Send OK Response to Client.
        //    return Request.CreateResponse(HttpStatusCode.OK, fileName);
        //}

        //Image Download
        //public HttpResponseMessage GetFiles()
        //{
        //    string path = HttpContext.Current.Server.MapPath("~/Uploads/");

        //    //Fetch the Image Files.
        //    List<string> images = new List<string>();

        //    //Extract only the File Names to save data.
        //    foreach (string file in Directory.GetFiles(path))
        //    {
        //        images.Add(Path.GetFileName(file));
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, images);
        //}
    }
}
