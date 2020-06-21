using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace YourBudget.API.Utils.Auth
{
    public class AuthJWTOptions
    {
        public AuthJWTOptions(string issuer, string audience, string key, int lifeTime)
        {
            Issuer = issuer;
            Audience = audience;
            _key = key;
            LifeTime = lifeTime;
        }

        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; private set; }

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public string Audience { get; private set; }

        /// <summary>
        /// Ключ для шифрации
        /// </summary>
        private string _key;

        /// <summary>
        /// Время жизни токена в минутах
        /// </summary>
        public int LifeTime { get; private set; }

        /// <summary>
        /// Генериреут семитричный ключ шифрования
        /// </summary>
        /// <returns>семитричный ключ шифрования</returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        }
    }
}
