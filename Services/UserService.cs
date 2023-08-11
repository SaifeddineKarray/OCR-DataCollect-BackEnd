namespace WebApi.Services;

using BCrypt.Net;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;
using MongoDB.Driver;
using WebAPI.Models;
using System.Xml.Linq;
using System.Diagnostics;
using WebAPI.Models.Users;
using System;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User GetById(string id);
    bool Create(RegisterRequest model);
}

public class UserService : IUserService
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;
    private readonly IMongoCollection<User> _users;

    public UserService(
        DataContext context,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings,
        IOCRDataCollectDatabaseSettings settings,
        IMongoClient mongoClient)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.UsersCollectionName);
    }


    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        //var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);
        var user = _users.Find(x => x.Username == model.Username).FirstOrDefault();

        // validate
        if (user == null)
        {
            //throw new AppException("Username not found");
            return new AuthenticateResponse("Username not found");
        }

        else if (!BCrypt.Verify(model.Password, user.PasswordHash))
        {
            //throw new AppException("Password is incorrect");
            return new AuthenticateResponse("Password is incorrect");
        }
        // authentication successful so generate jwt token
        else
        {
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken);
        }
    }

    public IEnumerable<User> GetAll()
    {
        //var myVariable = BCrypt.HashPassword("user");
        //Debug.WriteLine($"Admin password hash value: {myVariable}");
        return _users.Find(x => true).ToList();
    }

    public User GetById(string id) 
    {
        var user = _users.Find(x => x.Id == id).FirstOrDefault();
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public bool Create(RegisterRequest model)
    {
        var newuser = new User();
        var user = _users.Find(x => x.Username == model.Username).FirstOrDefault();
        if (!(user == null))
            return false;
        newuser.FirstName = model.FirstName;
        newuser.LastName = model.LastName;
        newuser.Username = model.Username;
        newuser.Role = model.Role;
        newuser.PasswordHash = BCrypt.HashPassword(model.Password);
        _users.InsertOne(newuser);
        return true;
    }
}