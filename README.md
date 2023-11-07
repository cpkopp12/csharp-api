# csharp-api
## Description
An API designed using .NET core 7.0, dapper, and Microsoft Azure SQL. For authentication the API uses JwtBearer. Most of the queries are done through stored procedures which can be found in a folder located in this directory.

## Table of Contents

- [Usage](#usage)

- [License](#license)

- [Deployed Application](#deployed-application)

- [Questions](#questions)



## Usage

Most of the endpoints require a jwt token for authorization, so the api is best tested using a postman or insomnia-like software. After registering as a user and loging in, use the bearer token as an authorization for the availble endpoints.

POST	/Auth/Register	
PUT		/Auth/ResetPassword	
POST	/Auth/Login	
GET		/Auth/RefreshToken

GET		/Post/Posts/{postId}/{userId}/{searchParam}
                  /0/0/none returns all posts
GET		/Post/MyPosts
PUT		/Post/UpsertPost
DELETE	/Post/Post/{postId}

GET		/UserComplete/TestConnection
GET		/UserComplete/GetUsers/{userId}/{isActive}
                              /0/false returns all users
PUT		/UserComplete/UpsertUser
DELETE	/UserComplete/DeleteUser/{userId}

GET		/Test/TestConnection
GET		/Test


## License
None.

## Deployed Application
Deployed through Azure at this adress [http://dtcodeapi123.azurewebsites.net](http://dtcodeapi123.azurewebsites.net).


## Questions

Email: cameronpkopp@gmail.com

GitHub Link: [https://github.com/cpkopp12](https://github.com/cpkopp12)

Contact Instructions: Contact me using the listed email!
