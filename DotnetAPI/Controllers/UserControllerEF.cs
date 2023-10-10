
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF : ControllerBase 
{

   
    IUserRepository _userRepository;

    IMapper _mapper;

    public UserControllerEF(IConfiguration config, IUserRepository userRepository)
    {

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
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
    }

     

    [HttpGet("GetUser/{userId}")]

  
    public User GetSingleUser(int userId)
    {
        
        return _userRepository.GetSingleUser(userId);
   
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _userRepository.GetSingleUser(user.UserId);

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
       User? userDb = _userRepository.GetSingleUser(userId);

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
    public UserSalary GetUserSalaryEF(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);
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
        UserSalary? userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId);

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
        UserSalary? userToDelete = _userRepository.GetSingleUserSalary(userId);

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
    public UserJobInfo GetUserJobInfoEf(int userId)
    {
        return _userRepository.GetSingleUserJobInfo(userId);
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
        UserJobInfo? userToUpdate = _userRepository.GetSingleUserJobInfo(userJobInfoForUpdate.UserId);

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
        UserJobInfo? userToDelete = _userRepository.GetSingleUserJobInfo(userId);

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
