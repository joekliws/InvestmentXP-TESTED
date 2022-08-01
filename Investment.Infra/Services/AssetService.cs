using AutoMapper;
using Investment.Domain.Constants;
using Investment.Domain.DTOs;
using Investment.Domain.Entities;
using Investment.Domain.Exceptions;
using Investment.Infra.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Infra.Services
{
    public interface IAssetService 
    {
        Task Buy(AssetCreateDTO asset);
        Task Sell(AssetCreateDTO asset);
        Task<List<CustomerAssetReadDTO>> GetAssetsByCustomer(int customerId);
        Task<AssetReadDTO> GetAssetById(int id);
    }
    public class AssetService : IAssetService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAssetRepository _repository;
        private readonly IMapper _mapper;

        public AssetService(IAccountRepository accountRepository, IAssetRepository repository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task Buy(AssetCreateDTO asset)
        {
            await validateAsset(asset);
            await validateBalance(asset);
            validateTimeOfCommerce();          
            await _repository.BuyAsset(asset);
        }

        public async Task Sell(AssetCreateDTO asset)
        {
            await validateAsset(asset, isSelling:true);
            await validateWallet(asset);
            validateTimeOfCommerce();

            await _repository.SellAsset(asset);
        }

        public async Task<List<CustomerAssetReadDTO>> GetAssetsByCustomer(int customerId)
        {
            await validateAccountExists(customerId);

            List<UserAsset> assets = await _repository.GetAssetsByCustomer(customerId);
            var response = _mapper.Map<List<CustomerAssetReadDTO>>(assets);
            return response;
        }

        public async Task<AssetReadDTO> GetAssetById(int id)
        {
            var asset = await _repository.GetAssetById(id);
            validateAssetExists(id, asset);
            var response = _mapper.Map<AssetReadDTO>(asset);
            
            return response;
        }

        private async Task validateAsset(AssetCreateDTO cmd, bool isSelling = false)
        {
            bool isValid = cmd.CodCliente > 0
                && cmd.CodAtivo > 0
                && cmd.QtdeAtivo > 0;

            bool customerNotHaveAsset = !await _repository
            .VerifyCustomerBoughtAsset(cmd.CodAtivo, cmd.CodCliente);

            if (!isValid) 
                throw new InvalidPropertyException("dados inválidos");

            if (isSelling && customerNotHaveAsset) 
                throw new NotFoundException("Cliente não possui esse ativo na carteira");
        }

        private async Task validateAccountExists(int customerId)
        {
            if (customerId <= 0)
                throw new InvalidPropertyException(ErrorMessage.VALUE_LESS_ZERO);

            bool accountExists = await _accountRepository.VerifyAccount(customerId);
            
            if (!accountExists)
                throw new NotFoundException(ErrorMessage.ACCOUNT_NOT_FOUND);

        }

        private void validateAssetExists(int id, Asset? asset)
        {
            if (id <= 0) 
                throw new InvalidPropertyException(ErrorMessage.VALUE_LESS_ZERO);

            if (asset == null) 
                throw new NotFoundException(ErrorMessage.ASSET_NOT_FOUND);

        }

        private async Task validateBalance(AssetCreateDTO cmd)
        {
            Asset? asset = await _repository.GetAssetById(cmd.CodAtivo);
            Account account = await _accountRepository.GetByCustomerId(cmd.CodCliente);

            if (account.Balance < asset?.Price * cmd.QtdeAtivo) throw new InvalidPropertyException(ErrorMessage.OUT_OF_BALANCE);

            if (asset?.Volume < cmd.QtdeAtivo) throw new InvalidPropertyException(ErrorMessage.ASSET_UNAVAILABLE);
        }

        private async Task validateWallet(AssetCreateDTO cmd)
        {
            var assets = await _repository.GetAssetsByCustomer(cmd.CodCliente);
            assets = assets.Where(ua => ua.AssetId == cmd.CodAtivo && ua.UtcSoldAt == null).ToList();

            decimal assetsQty = assets.Sum(a => a.Quantity);

            if (assetsQty < cmd.QtdeAtivo) 
                throw new InvalidPropertyException(ErrorMessage.OUT_OF_ASSET); 
        }

        private void validateTimeOfCommerce()
        {
            var timeOpenning = DateTime.Today.AddHours(13);
            var timeClosed = DateTime.Today.AddHours(20).AddMinutes(55);

            bool isValid = DateTime.UtcNow >= timeOpenning 
                && DateTime.UtcNow <= timeClosed 
                && !DateTime.Today.DayOfWeek.Equals(DayOfWeek.Saturday) 
                && !DateTime.Today.DayOfWeek.Equals(DayOfWeek.Sunday);

            if (!isValid) throw new InvalidPropertyException(ErrorMessage.MARKET_CLOSED);
           
        }
    }
}
