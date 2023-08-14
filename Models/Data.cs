using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAPI.Models
{
    public class Data
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("imageurl")]
        public string ImageURL { get; set; } = string.Empty;

        [BsonElement("text")]
        public string Text { get; set; } = string.Empty;

        [BsonElement("language")]
        public string Language { get; set; } = string.Empty;

        [BsonElement("score")]
        public int Score { get; set; } = 0;
    }
}