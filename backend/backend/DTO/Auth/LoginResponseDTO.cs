using MongoDB.Bson.Serialization.Attributes;

namespace backend.DTO.Auth
{
    public class LoginResponseDTO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
