
using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase //inhereting ControllerBase class
{

    DataContextDapper _dapper;

    public UserCompleteController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }


    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpGet("GetUsers/{userId}/{isActive}")]
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Get"; 
        string stringParameters = "";
        DynamicParameters sqlParameters = new DynamicParameters();
        
        if (userId != 0) 
        {
            stringParameters += ", @UserId=@UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
        }
        if (isActive) 
        {
            stringParameters += ", @Active=@ActiveParameter";
            sqlParameters.Add("@ActiveParameter", isActive, DbType.Boolean);
        }
        if (stringParameters.Length > 0)
        {
            sql += stringParameters.Substring(1);
        }
        

        IEnumerable<UserComplete> users = _dapper.LoadDataWithParameters<UserComplete>(sql, sqlParameters);
        return users;
    }

     
    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
        string sql = @"EXEC TutorialAppSchema.spUser_Upsert
            @FirstName = @FistNameParam,
            @LastName = @LastNameParam,
            @Email = @EmailParam,
            @Gender = @GenderParam,
            @Active = @ActiveParam,
            @JobTitle = @JobTitleParam,
            @Department = @DepartmentParam,
            @Salary = @SalaryParam,
            @UserId = @UserIdParam";

        DynamicParameters sqlParameters = new DynamicParameters();

        sqlParameters.Add("@FistNameParam", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParam", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParam", user.Email, DbType.String);
        sqlParameters.Add("@GenderParam", user.Gender, DbType.String);
        sqlParameters.Add("@ActiveParam", user.Active, DbType.Boolean);
        sqlParameters.Add("@JobTitleParam", user.JobTitle, DbType.String);
        sqlParameters.Add("@DepartmentParam", user.Department, DbType.String);
        sqlParameters.Add("@SalaryParam", user.Salary, DbType.Decimal);
        sqlParameters.Add("@UserIdParam", user.UserId, DbType.Int32);

        if (_dapper.ExecuteSql(sql)) {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    
    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"EXEC TutorialAppSchema.spUser_Delete 
            @UserId = @UserIdParam";

        DynamicParameters sqlParameters = new DynamicParameters();

        sqlParameters.Add("@UserIdParam", userId, DbType.Int32);

         if (_dapper.ExecuteSqlWithParameters(sql,sqlParameters)) {
            return Ok();
        }

        throw new Exception("Failed to Delete User");
    }
    
}
