# csharp-api
## Description
An API designed using .NET core 7.0, dapper, and Microsoft Azure SQL. For authentication the API uses JwtBearer. Most of the queries are done through stored procedures which can be found in a folder located in this directory.

## Table of Contents

- [Usage](#usage)

- [License](#license)

- [Deployed Application](#deployed-application)

- [Questions](#questions)



## Usage

Most of the endpoints require a jwt token for authorization, so the api is best tested using a postman or insomnia-like software. After registering as a user and logging in, use the bearer token as an authorization for the available endpoints. <br/>

POST	/Auth/Register	<br/>
PUT		/Auth/ResetPassword	<br/>
POST	/Auth/Login	<br/>
GET		/Auth/RefreshToken <br/>

GET		/Post/Posts/{postId}/{userId}/{searchParam} <br/>
      /Post/Posts/0/0/none returns all posts <br/>
GET		/Post/MyPosts <br/>
PUT		/Post/UpsertPost <br/>
DELETE	/Post/Post/{postId} <br/>

GET		/UserComplete/TestConnection <br/>
GET		/UserComplete/GetUsers/{userId}/{isActive} <br/>
      /UserComplete/GetUsers/0/false returns all users <br/>
PUT		/UserComplete/UpsertUser <br/>
DELETE	/UserComplete/DeleteUser/{userId} <br/>

GET		/Test/TestConnection <br/>
GET		/Test <br/>


## License
None.

## Deployed Application
Deployed through Azure at this adress [http://dtcodeapi123.azurewebsites.net](http://dtcodeapi123.azurewebsites.net).


## Questions

Email: cameronpkopp@gmail.com

GitHub Link: [https://github.com/cpkopp12](https://github.com/cpkopp12)

Contact Instructions: Contact me using the listed email!
