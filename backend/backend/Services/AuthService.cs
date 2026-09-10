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
                Password = hashedPassword,
                ProfileImage = user.ProfileImage
            };

            await userSchema.InsertOneAsync(newUser);

            response.StatusCode = 201;
            response.Message = "User created successfully";

            return response;
        }


        public async Task<ApiResponseWithData<LoginResponseDTO>> LoginUser(LoginRequestDTO loginRequest)
        {
            ApiResponseWithData<LoginResponseDTO> response = new ApiResponseWithData<LoginResponseDTO>();
            response.Data = new();

            UserSchema existUser = await userSchema.Find(us => us.Email == loginRequest.Email).FirstOrDefaultAsync();

            if (existUser == null)
            {
                response.StatusCode = 404;
                response.Message = "User does not exist";

                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, existUser.Password))
            {
                response.StatusCode = 401;
                response.Message = "Wrong user email or password";

                return response;
            }

            response.StatusCode = 200;
            response.Message = "User logged in successfully";
            response.Data.Id = existUser.Id!;
            response.Data.Name = existUser.Name;
            response.Data.Email = existUser.Email;

            return response;
        }
    }
}
