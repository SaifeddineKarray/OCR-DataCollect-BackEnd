using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BCryptNet = BCrypt.Net.BCrypt;

namespace WebApi.Entities;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [BsonElement("lastName")]
    public string LastName { get; set; } = string.Empty;

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("role")]
    public Role Role { get; set; } = Role.User;

    //[BsonIgnore]
    //public string Password { get; set; } = string.Empty;

    [JsonIgnore]
    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = string.Empty; /*= BCryptNet.HashPassword("admin");*/
}