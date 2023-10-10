
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

    DataContextEF _entityFramework;
    IUserRepository _userRepository;

    IMapper _mapper;

    public UserControllerEF(IConfiguration config, IUserRepository userRepository)
    {
        _entityFramework = new DataContextEF(config);

        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.CreateMap<UserToAddDto,User>();
            cfg.CreateMap<UserSalary,UserSalary>();
            cfg.CreateMap<UserJobInfo,UserJobInfo>();
        }));
    }

    [HttpGet("GetUsers")]

    // public IEnumerable<User> GetUsers()
    //for now return array of strings
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }

     

    [HttpGet("GetUser/{userId}")]

  
    public User GetSingleUser(int userId)
    {
        
        User? user = _entityFramework.Users
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
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;

            if (_userRepository.SaveChanges())
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

        _userRepository.AddEntity<User>(userDb);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

            

        throw new Exception("Failed to Add User");

    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
       User? userDb = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }


        }

        throw new Exception("Failed to Delete Userr");

    }

    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserJobInfoEF(int userId)
    {
        return _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalaryEF(UserSalary userSalaryForInsert) 
    {
        _userRepository.AddEntity<UserSalary>(userSalaryForInsert);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed on save");
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
    {
        UserSalary? userToUpdate = _entityFramework.UserSalary
            .Where(u => u.UserId == userForUpdate.UserId)
            .FirstOrDefault();

        if (userToUpdate != null) {
            _mapper.Map(userForUpdate, userToUpdate);

            if(_userRepository.SaveChanges()) 
            {
                return Ok();
            }
            throw new Exception("Update failed on save");
        }
        throw new Exception("failed to find user salary to update");

    }

    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalaryEf(int userId)
    {
        UserSalary? userToDelete = _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if (userToDelete != null)
        {
            _userRepository.RemoveEntity<UserSalary>(userToDelete);

            if(_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Delete failed on save");
        }

        throw new Exception("failed to find user");

    }


    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfoEf(int userId)
    {
        return _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfoEf(UserJobInfo userJobInfoForInsert)
    {
        _userRepository.AddEntity<UserJobInfo>(userJobInfoForInsert);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Adding UserJobInfo failed on save");

    }

    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfoEf(UserJobInfo userJobInfoForUpdate)
    {
        UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
            .Where(u => u.UserId == userJobInfoForUpdate.UserId)
            .FirstOrDefault();

        if (userToUpdate != null)
        {
            _mapper.Map(userJobInfoForUpdate, userToUpdate);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed on save");
        }
        throw new Exception("Failed to find user to update");
    }

    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfoEf(int userId)
    {
        UserJobInfo? userToDelete = _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if (userToDelete != null)
        {
            _userRepository.RemoveEntity<UserJobInfo>(userToDelete);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting failed on save");
        }

        throw new Exception("failed to find user");

    }



}
