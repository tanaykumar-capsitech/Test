using backend.DataBaseSettings;
using backend.DTO.Auth;
using backend.Models;
using backend.Responses;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace backend.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<UserSchema> userSchema;

        public AuthService(IOptions<DatabaseSettings> dbSettings)
        {
            MongoClient mongo = new MongoClient(dbSettings.Value.ConnectionString);
            userSchema = mongo.GetDatabase(dbSettings.Value.DatabaseName).GetCollection<UserSchema>(dbSettings.Value.UserCollection);
        }

        public async Task<ApiResponse> RegisterUser(UserCreationDTO user)
        {
            ApiResponse response = new ApiResponse();

            UserSchema existUser = await userSchema.Find(us => us.Email == user.Email).FirstOrDefaultAsync();

            if (existUser != null) 
            {
                response.StatusCode = 409;
                response.Message = "User already exist";

                return response;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            UserSchema newUser = new UserSchema()
            {
                Email = user.Email,
                Name = user.Name,
                Password = hashedPassword
            };

            await userSchema.InsertOneAsync(newUser);

            response.StatusCode = 201;
            response.Message = "User created successfully";

            return response;
        }
    }
}
