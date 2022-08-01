using AutoMapper;
using Investment.Domain.Constants;
using Investment.Domain.DTOs;
using Investment.Domain.Entities;
using Investment.Domain.Exceptions;
using Investment.Domain.Helpers;
using Investment.Infra.Context;
using Investment.Infra.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Infra.Services
{
    public interface IAccountService
    {
        Task<bool> Deposit(Operation operation);
        Task<bool> Withdraw(Operation operation);
        Task<Operation> GetBalance(int custmerId);
        Task<AccountReadDTO> CreateAccount(AccountCreateDTO cmd);
    }
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> Deposit(Operation operation)
        {
            bool isSuccefull;
            
            await validateOperation(operation);
            var account = await _repository.GetByCustomerId(operation.CodCliente);
            
            account.Balance += operation.Saldo;
            await _repository.UpdateBalance(account);
            isSuccefull = true;

            return isSuccefull;
        }

        public async Task<Operation> GetBalance(int custmerId)
        {
            Operation operation = new();
            var validUser = await _repository.VerifyAccount(custmerId);
            
            if (validUser)
                operation = await _repository.GetBalance(custmerId);
            
            return operation;
        }

        public async Task<bool> Withdraw(Operation operation)
        {
            bool isSuccefull;
            await validateOperation(operation);
            Account account = await _repository.GetByCustomerId(operation.CodCliente);
            validateBalance(operation, account);

            account.Balance -= operation.Saldo;
            await _repository.UpdateBalance(account);
            isSuccefull = true;
            return isSuccefull;
        }

        private async Task validateOperation(Operation operation)
        {
            bool accountExists = await _repository.VerifyAccount(operation.CodCliente);
            
            if (operation.Saldo <= 0)
                throw new InvalidPropertyException(ErrorMessage.VALUE_LESS_ZERO);     
            if (!accountExists)
                throw new NotFoundException(ErrorMessage.ACCOUNT_NOT_FOUND);     
        }

        private void validateBalance(Operation operation, Account account)
        {

            if (operation.Saldo > account.Balance)
                throw new InvalidPropertyException(ErrorMessage.INVALID_BALANCE);
            
        }

        public async Task<AccountReadDTO> CreateAccount(AccountCreateDTO cmd)
        {
            Account newAccount = await _repository.CreateAccount(cmd);
            if (newAccount == null) throw new InvalidPropertyException(ErrorMessage.ACCOUNT_NOT_CREATED);
            AccountReadDTO response = _mapper.Map<AccountReadDTO>(newAccount);
            return response;

        }
    }
}
