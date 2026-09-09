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

        public AuthController(AuthService authService)
        {
            this.authService = authService;
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
    }
}
