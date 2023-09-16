namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;
using WebApi.Models.Users;
using Microsoft.AspNetCore.Identity;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        // only admins can access other user records
        var currentUser = (User)HttpContext.Items["User"];
        if (id != currentUser.Id && currentUser.Role != Role.Admin)
            return Unauthorized(new { message = "Unauthorized" });

        var user =  _userService.GetById(id);
        return Ok(user);
    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser([FromBody] User user)
    {
        _userService.Update(user);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(string id)
    {
        _userService.Remove(id);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Post([FromBody] RegisterRequest model)
    {
        if (!_userService.Create(model))
            return BadRequest(new { message = "Error: Username already registered" });
        //return CreatedAtAction(nameof(GetById), new { id = newuser.Id }, newuser);
        return Ok(new { message = "Creation Successful"});
    }
}