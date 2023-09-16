using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using WebApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users;

    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; } = Role.User;

    }
