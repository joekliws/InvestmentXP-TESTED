using Investment.Domain.Entities;
using Investment.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Infra.Repository
{
    public interface IUserRepository 
    {
        Task<bool> VerifyUserCredentials(string userLogin, string password);
    }
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public  async Task<bool> VerifyUserCredentials(string userLogin, string password)
        {
            Account? account = await _context.Accounts
                .Include(acc => acc.User)
                .FirstOrDefaultAsync(usr => usr.AccountNumber.ToString() == userLogin 
                || usr.User.Cpf == userLogin);

            if (account == null) return false;

            return ReadPasswordHash(password, account.User.PasswordHash, account.User.PasswordSalt);
            
        }

        private bool ReadPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);

            }
        }
    }
}
