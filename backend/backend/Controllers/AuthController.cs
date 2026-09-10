using backend.DTO.Auth;
using backend.Responses;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly JWTService jwtService;

        public AuthController(AuthService authService, JWTService jwtService)
        {
            this.authService = authService;
            this.jwtService = jwtService;
        }


        [HttpPost("register")]
        public async Task<ApiResponse> RegisterUser(UserCreationDTO user)
        {
            ApiResponse response = new();

            if(string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                response.StatusCode = 400;
                response.Message = "Enter all the fields";

                return response;
            }

            return await authService.RegisterUser(user);
        }


        [HttpPost("login")]
        public async Task<ApiResponse> LoginUser(LoginRequestDTO user)
        {
            ApiResponse response = new();

            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                response.StatusCode = 400;
                response.Message = "Enter all the fields";

                return response;
            }

            ApiResponseWithData<LoginResponseDTO> loginUser = await authService.LoginUser(user);

            if(loginUser.Data != null)
            {
                if (loginUser.Data.Id != null || loginUser.Data.Name != null || loginUser.Data.Email != null)
                {
                    string accessToken = jwtService.GenerateAccessToken(loginUser.Data.Id, loginUser.Data.Name, loginUser.Data.Email);

                    Response.Cookies.Append("accessToken", accessToken, new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });
                }
            }

            response.StatusCode = loginUser.StatusCode;
            response.Message = loginUser.Message;

            return response;
        }
    }
}
