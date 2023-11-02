using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        public IEnumerable<Post> GetPosts(int postId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get";
            string stringParams = "";

            DynamicParameters sqlParams = new DynamicParameters();

            if (postId != 0)
            {
                stringParams += ", @PostId = @PostIdParam";
                sqlParams.Add("@PostIdParam", postId, DbType.Int32);
            }
            if (userId != 0)
            {
                stringParams += ", @UserId = @UserIdParam";
                sqlParams.Add("@UserIdParam", userId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none")
            {
                stringParams += ", @SearchValue = @SearchValueParam";
                sqlParams.Add("@SearchValueParam", searchParam, DbType.String);
            }

            if (stringParams.Length > 0)
            {
                sql += stringParams.Substring(1);
            }
            
            return _dapper.LoadDataWithParameters<Post>(sql, sqlParams);
        }

        
        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId=@UserIdParam";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UserIdParam", this.User.FindFirst("userId")?.Value, DbType.Int32);
            
            return _dapper.LoadDataWithParameters<Post>(sql, sqlParams);
        }


        [HttpPut("UpsertPost")]
        public IActionResult AddPost(Post postToAdd)
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
                @UserId=@UserIdParam,
                @PostTitle=@PostTitleParam,
                @PostContent=@PostContentParam";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UserIdParam", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParams.Add("@PostTitleParam", postToAdd.PostTitle, DbType.String);
            sqlParams.Add("@PostContentParam", postToAdd.PostContent, DbType.String);

            if(postToAdd.PostId > 0)
            {
                sql += ", @PostId=@PostIdParam";
                sqlParams.Add("@PostIdParam", postToAdd.PostId, DbType.Int32);
            }
            

            if(_dapper.ExecuteSqlWithParameters(sql, sqlParams))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert new post!");

        }




        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sql = @"EXEC TutorialAppSchema.spPost_Delete 
                @UserId=@UserIdParam,
                @PostId=@PostIdParam";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@UserIdParam", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParams.Add("@PostIdParam", postId, DbType.Int32);
            
            if(_dapper.ExecuteSqlWithParameters(sql,sqlParams))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post!");
        }

       
    }
}