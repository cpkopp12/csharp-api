
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF : ControllerBase //inhereting ControllerBase class
{

    DataContextEF _entityFramwork;

    IMapper _mapper;

    public UserControllerEF(IConfiguration config)
    {
        _entityFramwork = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.CreateMap<UserToAddDto,User>();
        }));
    }

    [HttpGet("GetUsers")]

    // public IEnumerable<User> GetUsers()
    //for now return array of strings
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramwork.Users.ToList<User>();
        return users;
    }

     

    [HttpGet("GetUser/{userId}")]

  
    public User GetSingleUser(int userId)
    {
        
        User? user = _entityFramwork.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (user != null)
        {
            return user;
        }

        throw new Exception("Failed to Get Userr");
   
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramwork.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;

            if (_entityFramwork.SaveChanges() > 0)
            {
                return Ok();
            }


        }

        throw new Exception("Failed to Update Userr");

    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        _entityFramwork.Add(userDb);

        if (_entityFramwork.SaveChanges() > 0)
        {
            return Ok();
        }

            

        throw new Exception("Failed to Add User");

    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
       User? userDb = _entityFramwork.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityFramwork.Users.Remove(userDb);

            if (_entityFramwork.SaveChanges() > 0)
            {
                return Ok();
            }


        }

        throw new Exception("Failed to Delete Userr");

    }





}
