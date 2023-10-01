
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase //inhereting ControllerBase class
{

    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }



    [HttpGet("GetUsers")]

    // public IEnumerable<User> GetUsers()
    //for now return array of strings
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.USERS";
        
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
        
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    }

     

    [HttpGet("GetUser/{userId}")]

  
    public User GetSingleUser(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.USERS
                WHERE UserId = " + userId.ToString();
        
        User user = _dapper.LoadDataSingle<User>(sql);

        return user;
   
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET
                [FirstName] = '" + user.FirstName + 
                "',[LastName] = '" + user.LastName +
                "', [Email] = '" + user.Email +
                "', [Gender] = '" + user.Gender +
                "', [Active] = '" + user.Active +
            "' WHERE UserId = " + user.UserId;

        if (_dapper.ExecuteSql(sql)) {
            return Ok();
        }

        throw new Exception("Failed to Update User");

    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        ) VALUES (
            '" + user.FirstName + 
            "', '" + user.LastName +
            "','" + user.Email +
            "', '" + user.Gender +
            "', '" + user.Active +
            "')";

        if (_dapper.ExecuteSql(sql)) {
            return Ok();
        }

        throw new Exception("Failed to Add User");

    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users WHERE UserId = " + userId.ToString();

         if (_dapper.ExecuteSql(sql)) {
            return Ok();
        }

        throw new Exception("Failed to Delete User");

    }





}
