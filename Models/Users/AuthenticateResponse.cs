namespace WebApi.Models.Users;

using WebApi.Entities;

public class AuthenticateResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    public string Token { get; set; }
    public string errorMessage { get; set; }   

    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        Role = user.Role;
        Token = token;
        errorMessage = string.Empty;
    }
    public AuthenticateResponse(string errormessage)
    {
        //Id = string.Empty;
        //FirstName = string.Empty;
        //LastName = string.Empty;
        //Username = string.Empty;
        //Role = Role.User;
        //Token = string.Empty;
        errorMessage = errormessage;
    }
}