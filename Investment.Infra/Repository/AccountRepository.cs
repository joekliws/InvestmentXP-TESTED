using Investment.Domain.DTOs;
using Investment.Domain.Entities;
using Investment.Domain.Helpers;
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
    public interface IAccountRepository 
    {
        Task<Account> GetByCustomerId(int userId);
        Task<Account> GetByAccountNumberOrCpf(string accountNumberOrCpf);
        Task<Account> CreateAccount(AccountCreateDTO cmd);
        Task<bool> UpdateBalance(Account account);
        Task<bool> VerifyAccount(int userId);
        Task<Operation> GetBalance(int customerId);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccount(AccountCreateDTO cmd)
        {
            Account newAccount = new();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    User newUser = createUser(cmd);
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    newAccount = GenerateAccountInfo(newUser);
                    _context.Accounts.Add(newAccount);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    transaction.Rollback();
                }
            }

            return newAccount;
        }

        private Account GenerateAccountInfo(User newUser)
        {
            int accountNumber = new Random().Next(1000, 100000);
            return new Account()
            {
                AccountNumber = accountNumber,
                UserId = newUser.UserId,
                User = newUser
            };
        }

        private User createUser(AccountCreateDTO cmd)
        {
            createPasswordHash(cmd.Password, out byte[] passwordHash, out byte[] passwordSalt);

            return new User()
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                PreferedName = cmd.PreferedName,
                Cpf = cmd.Cpf,
                InvestorStyle = cmd.InvestorStyle,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<Operation> GetBalance(int customerId)
        {
            Operation operation = new();
            Account account = await GetByCustomerId(customerId);
            operation.CodCliente = account.UserId;
            operation.Saldo = account.Balance;

            return operation;
        }

        public async Task<Account> GetByAccountNumberOrCpf(string accountNumberOrCpf)
        {
            Account account = await _context.Accounts
                .FirstAsync(acc => acc.AccountNumber.ToString() == accountNumberOrCpf 
                || acc.User.Cpf == accountNumberOrCpf);

            return account;
        }

        public async Task<Account> GetByCustomerId(int userId)
        {
            Account account =  await _context.Accounts.FirstAsync(acc => acc.UserId == userId);
            return account;
        }

        public async Task<bool> UpdateBalance(Account account)
        {
            _context.Update(account);
            int result = await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> VerifyAccount(int userId)
        {
            bool exists = await _context.Accounts.AnyAsync(acc => acc.UserId == userId);
            return exists;
        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
