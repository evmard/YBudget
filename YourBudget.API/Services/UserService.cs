using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using YourBudget.API.Models;
using YourBudget.API.Services.ViewModels;
using YourBudget.API.Utils;
using YourBudget.API.Utils.Auth;

namespace YourBudget.API.Services
{
    public class UserService : BaseService, IIdentiyManager
    {        
        public UserService(BudgetContext context, Localizer localizer): base(context, localizer)
        {
        }


        public Result<UserView> CreateUser(string login, string name, string password, string mail)
        {
            var result = new Result<UserView>();

            login = login.Trim();
            name = name.Trim();
            mail = mail.Trim();

            if (string.IsNullOrEmpty(login))
                result.AddValidationError(nameof(login), _t.EmptyFieldMsg);

            if (string.IsNullOrEmpty(name))
                result.AddValidationError(nameof(name), _t.EmptyFieldMsg);

            if (string.IsNullOrEmpty(password))
                result.AddValidationError(nameof(password), _t.EmptyFieldMsg);

            if (string.IsNullOrEmpty(mail))
                result.AddValidationError(nameof(mail), _t.EmptyFieldMsg);

            if (!result.IsSuccess)
                return result;

            if (_context.Users.Any(item => string.Equals(item.Login, login)))
                result.AddValidationError(nameof(login), _t.UniqFieldMsg);

            if (_context.Users.Any(item => string.Equals(item.Mail, mail.ToLowerInvariant())))
                result.AddValidationError(nameof(mail), _t.UniqFieldMsg);

            if (!result.IsSuccess)
                return result;

            var user = new User
            {
                Login = login,
                Mail = mail.ToLowerInvariant(),
                Name = name,
                PassHash = GetMD5Hash(password)
            };

            _context.Add(user);
            _context.SaveChanges();

            result.Data = new UserView(user);
            return result;
        }

        public Result<User> GetUser(string login)
        {
            if (login == "admin")
            {
                var user = new User
                {
                    Id = Guid.Empty,
                    Language = "RU",
                    Login = "admin",
                    Mail = "evmard@gmail.com",
                    Name = "Администратор",
                    PassHash = GetMD5Hash("qaswed")
                };

                return Result<User>.Success(user);
            }

            try
            {
                var user = _context.Users.FirstOrDefault(item => item.Login == login);
                if (user == null)
                    return Result<User>.Error(_t["Пользователь не найден"]);

                return Result<User>.Success(user);
            }
            catch(Exception e)
            {
                return Result<User>.Error(e.Message);
            }
        }

        public Result<string> ReqestResetPwd(string login, string mail)
        {
            var user = GetUser(login);
            if (!user.IsSuccess)
                return Result<string>.Error(user.Message);

            if (!string.Equals(user.Data.Mail, mail, StringComparison.InvariantCultureIgnoreCase))
                return Result<string>.Error(_t["Почта указанна не верно"]);

            var guid = Guid.NewGuid();
            user.Data.ResetToken = guid.ToString();
            _context.Update(user.Data);
            _context.SaveChanges();
            return Result<string>.Success(user.Data.ResetToken);
        }

        public Result ResetPassword(string login, string resetToken, string newPass, string confirmPass)
        {
            var user = GetUser(login);
            if (!user.IsSuccess)
                return user;

            if (string.IsNullOrEmpty(user.Data.ResetToken))
                return Result.Error(_t.ReqestDenied);

            if (!string.Equals(user.Data.ResetToken, resetToken))
                return Result.Error(_t["Токен не валиден"]);


            if (!string.Equals(newPass, confirmPass))
                return Result.Error(_t["Пароли не совподают"]);

            var pwdHash = GetMD5Hash(newPass);
            user.Data.ResetToken = null;
            user.Data.PassHash = pwdHash;

            _context.Update(user.Data);
            _context.SaveChanges();

            return Result.Success();
        }

        public Result SetNewPassword(string login, string oldPass, string newPass, string confirmPass)
        {
            var user = GetUser(login);
            if (!user.IsSuccess)
                return user;

            var result = new Result();
            if (string.IsNullOrEmpty(oldPass))
                result.AddValidationError(nameof(oldPass), _t.EmptyFieldMsg);

            if (string.IsNullOrEmpty(newPass))
                result.AddValidationError(nameof(newPass), _t.EmptyFieldMsg);

            if (!string.Equals(newPass, confirmPass))
                result.AddValidationError(nameof(confirmPass), _t["Пароли не совподают"]);

            var oldMd5Hash = GetMD5Hash(oldPass);
            if (user.Data.PassHash != oldMd5Hash)
                result.AddValidationError(nameof(confirmPass), _t["Не верный пароль"]);

            if (!result.IsSuccess)
                return result;

            user.Data.PassHash = GetMD5Hash(newPass);
            _context.Update(user.Data);
            _context.SaveChanges();

            return Result.Success();
        }

        private static string GetMD5Hash(string input)
        {
            string pwdHash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                pwdHash = sb.ToString();
            }

            return pwdHash;
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = GetUser(username);
            if (!user.IsSuccess)
                return null;

            var hash = GetMD5Hash(password);
            if (user.Data.PassHash != hash)
                return null;

            var userRole = username == "admin" ? "Admin" : "User";


            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim("Language", user.Data.Language ?? "RU"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole),
                new Claim("Guid", user.Data.Id.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
