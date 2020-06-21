using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YourBudget.API.Controllers.ReqestData;
using YourBudget.API.Services;
using YourBudget.API.Services.ViewModels;
using YourBudget.API.Utils;
using YourBudget.API.Utils.Auth;

namespace YourBudget.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthJWTOptions _authJWTOptions;

        public UserController(UserService userService, AuthJWTOptions authJWTOptions)
        {
            _userService = userService;
            _authJWTOptions = authJWTOptions;
        }

        [HttpPost]
        public async Task Token([FromBody] UserCredantional userCredantional)
        {
            if (userCredantional == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            ClaimsIdentity identity;
            try
            {
                identity = _userService.GetIdentity(userCredantional.Login, userCredantional.Password);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.Message);
                return;
            }

            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                _authJWTOptions.Issuer,
                _authJWTOptions.Audience,
                identity.Claims,
                now,
                now.Add(TimeSpan.FromMinutes(_authJWTOptions.LifeTime)),
                new SigningCredentials(_authJWTOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                language = identity.Claims.First(item => item.Type == "Language").Value
            };

            Response.ContentType = "application/json";
            string JWTString = JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
            await Response.WriteAsync(JWTString);
        }

        [HttpPost]
        public ActionResult<Result<UserView>> Create([FromBody] NewUserItem user)
        {
            return _userService.CreateUser(user.Login, user.Name, user.Password, user.Mail);
        }

        [HttpGet]
        public ActionResult<Result<string>> ResetToken([FromQuery]string login, [FromQuery]string mail)
        {
            return _userService.ReqestResetPwd(login, mail);
        }

        [HttpPost]
        public ActionResult<Result> ResetPassword([FromBody] PasswordResetItem resetItem)
        {
            return _userService.ResetPassword(resetItem.Login, resetItem.ResetToken, resetItem.NewPass, resetItem.ConfirmPass);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Result> SetNewPassword([FromBody] NewPassItem item)
        {
            return _userService.SetNewPassword(HttpContext.User.Identity.Name, item.OldPass, item.NewPass, item.ConfirmPass);
        }
    }
}
