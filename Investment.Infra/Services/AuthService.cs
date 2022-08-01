using Investment.Domain.Constants;
using Investment.Domain.Entities;
using Investment.Domain.Exceptions;
using Investment.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Infra.Services
{
    public interface IAuthService
    {
        Task<KeyValuePair<int, bool>> Login(string userLogin, string password);
        string GenerateToken(int accountId);
        void ValidateToken(string token);
        void CheckTokenBelongsToUser(string token, int userId);
        void Logout();

    }
    public class AuthService : IAuthService
    {
        private readonly string _secret, _issuer, _audience;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _repository;

        public AuthService(IConfiguration configuration, IUserRepository repository, IAccountRepository accountRepository)
        {
            _secret = configuration.GetSection("AppSettings:Secret").Value;
            _issuer = configuration.GetSection("Jwt:Issuer").Value;
            _audience = configuration.GetSection("Jwt:Audience").Value;

            _accountRepository = accountRepository;
            _repository = repository;
        }
        public async Task<KeyValuePair<int, bool>> Login(string userLogin, string password)
        {
            bool isPasswordCorrect = await _repository.VerifyUserCredentials(userLogin, password);

            if (!isPasswordCorrect) throw new UnauthorizedException(ErrorMessage.INVALID_LOGIN);

            Account account = await _accountRepository.GetByAccountNumberOrCpf(userLogin);

            var response = new KeyValuePair<int, bool>(account.UserId, isPasswordCorrect);

            return response;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(int accountId)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, accountId.ToString())
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secret));

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public void ValidateToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var parameters = GetParameters();
            handler.ValidateToken(token, parameters, out SecurityToken validatedToken);
        }
        public void CheckTokenBelongsToUser (string token, int userId)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jwt = (JwtSecurityToken)handler.ReadToken(token);
            bool isValid = jwt.Claims.Any(claim => claim.Type == ClaimTypes.Sid && claim.Value == userId.ToString());

            if (!isValid)
                throw new ForbiddenException(ErrorMessage.TOKEN_INVALID);

        }

        private TokenValidationParameters GetParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
            };
        }
    }
}
