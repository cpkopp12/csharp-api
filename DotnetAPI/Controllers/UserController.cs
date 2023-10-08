
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

    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        return _dapper.LoadData<UserSalary>(@"
            SELECT UserSalary.UserId,
                UserSalary.Salary
            FROM TutorialAppSchema.UserSalary
                WHERE UserId = " +  userId);
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserSalary (
                UserId,
                Salary
            ) VALUES (" + userSalaryForInsert.UserId
            + ", " + userSalaryForInsert.Salary
            + ")";
        if (_dapper.ExecuteSql(sql)) 
        {
            return Ok(userSalaryForInsert);
        }
        throw new Exception("adding usersalary faild on save");
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalary(UserSalary userSalaryForUpdate)
    {
        string sql = @"
            UPDATE TutorialAppSchema.UserSalary SET Salary ="
             + userSalaryForUpdate.Salary 
             + "WHERE UserId=" + userSalaryForUpdate.UserId;

        if (_dapper.ExecuteSql(sql)) 
        {
            return Ok(userSalaryForUpdate);
        }
        throw new Exception("updating usersalary faild on save");
    }

    [HttpDelete("UserSalary/{userId}")]

    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserSalary WHERE UserId=" +  userId;

        if (_dapper.ExecuteSql(sql)) 
        {
            return Ok();
        }
        throw new Exception("Deleting user salary faild on save");
    }

    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId)
    {
        return _dapper.LoadData<UserJobInfo>(@"
            SELECT UserJobInfo.UserId
                    , UserJobInfo.JobTitle
                    , UserJobInfo.Department
            FROM TutorialAppSchema.UserJobInfo
                WHERE UserId  = " + userId);
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfo(UserJobInfo userJobInfoForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserJobInfo (
                UserId,
                Department,
                JobTitle
            ) VALUES (" + userJobInfoForInsert.UserId
            + ", '" + userJobInfoForInsert.Department
            +"', '" + userJobInfoForInsert.JobTitle
            + "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForInsert);
        }
        throw new Exception("Adding user job info failed on save");
            
    }

    [HttpPut("UserJobInfo")]
    public IActionResult UserJobInfo(UserJobInfo userJobInfoForUpdate)
    {
        string sql = @"
            UPDATE TutorialAppSchema.UserJobInfo SET Department ='"
            + userJobInfoForUpdate.Department
            +"', JobTitle='"
             + userJobInfoForUpdate.JobTitle 
             + "' WHERE UserId=" + userJobInfoForUpdate.UserId;

        if (_dapper.ExecuteSql(sql)) 
        {
            return Ok(userJobInfoForUpdate);
        }
        throw new Exception("updating user Job Info faild on save");
    }

    [HttpDelete("DeleteJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId = " + userId.ToString();

         if (_dapper.ExecuteSql(sql)) {
            return Ok();
        }

        throw new Exception("Failed to Delete User Job Info");
    }
    

}
