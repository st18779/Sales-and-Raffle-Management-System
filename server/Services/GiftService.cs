using Microsoft.AspNetCore.Http.HttpResults;
using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Repositories;
using static server.DTOs.GiftDTOs;

namespace server.Services
{
    public class GiftService: IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly ILogger<GiftService> _logger;

        public GiftService(
            IGiftRepository giftRepository,
            ICategoryRepository categoryRepository,
            IDonorRepository donorRepository,
            ILogger<GiftService> logger)
        {
            _giftRepository = giftRepository;
            _categoryRepository = categoryRepository;
            _donorRepository = donorRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<GiftResponseDto>> GetAllGifts()
        {
            var gifts = await _giftRepository.GetAllGifts();

            return gifts.Select(MapToResponseDto);
        }

        public async Task<GiftResponseDto?> GetGiftById(int id)
        {
            var gift = await _giftRepository.GetGiftById(id);
            return gift != null? MapToResponseDto(gift) : null;
        }

        public async Task<IEnumerable<GiftResponseDto>> GetGiftsByCategory(int categoryId)
        {
            var gifts = await _giftRepository.GetGiftsByCategory(categoryId);
            return gifts.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<GiftResponseDto>> SearchGiftByName(string name)
        {
            var gifts = await _giftRepository.SearchGiftByName(name);
            return gifts.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<GiftResponseDto>> SearchGiftByDonorName(string donorName)
        {
            var gifts = await _giftRepository.SearchGiftByDonorName(donorName);
            return gifts.Select(MapToResponseDto);
        }
        public async Task<GiftResponseDto> CreateGift(GiftCreateDto gift)
        {
            if(await _categoryRepository.GetCategoryById(gift.CategoryId) == null)
                throw new ArgumentException($"Category with ID {gift.CategoryId} does not exist.");

            if (await _donorRepository.GetDonorById(gift.DonorId) == null)
                throw new ArgumentException($"Donor with ID {gift.DonorId} does not exist.");
            Console.WriteLine($"DonorId received: '{gift.DonorId}'");

            var newGift = new Gift
            {
                Name = gift.Name,
                Description = gift.Description,
                Price = gift.Price,
                CategoryId = gift.CategoryId,
                DonorId = gift.DonorId,
                ImageUrl = gift.ImageUrl,
            };

            var createdGift = await _giftRepository.CreateGift(newGift);
            return MapToResponseDto(createdGift);
        }

        public async Task<bool> DeleteGift(int id)
        {
            return await _giftRepository.DeleteGift(id);
        }

        public async Task<GiftResponseDto?> UpdateGift(int id,GiftUpdateDto gift)
        {
            var existGift = await _giftRepository.GetGiftById(id);
            if(existGift == null)
                return null;
            if(gift.Name != null) existGift.Name = gift.Name;
            if (gift.Description != null) existGift.Description = gift.Description;
            if (gift.Price.HasValue) existGift.Price = gift.Price.Value;
            if (gift.ImageUrl!= null) existGift.ImageUrl = gift.ImageUrl;
            if(gift.CategoryId.HasValue)
            {
                if(await _categoryRepository.GetCategoryById(gift.CategoryId.Value) == null)
                {
                    throw new ArgumentException($"Category with ID {gift.CategoryId} does not exist.");
                }
                existGift.CategoryId = gift.CategoryId.Value;
            }
            if (gift.DonorId != null)
            {
                if (await _donorRepository.GetDonorById(gift.DonorId) == null)
                {
                    throw new ArgumentException($"Donor with ID {gift.DonorId} does not exist.");
                }
                existGift.DonorId = gift.DonorId;
            }

            var updateGift = await _giftRepository.UpdateGift(existGift);
            return updateGift != null ? MapToResponseDto(updateGift) : null;
        }

        public async Task<User?> RandomWinner(int id)
        {
            if (await _giftRepository.GetGiftById(id) == null)
                throw new ArgumentException($"Gift with ID {id} does not exist.");
            else
                return await _giftRepository.RandomWinner(id);
        }

        private static GiftResponseDto MapToResponseDto(Gift gift)
        {
            return new GiftResponseDto
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                Price = gift.Price,
                IsRandomized = gift.IsRandomized || false,
                CategoryName = gift.Category?.Name ?? string.Empty,
                DonorName = gift.Donor?.Name ?? string.Empty,
                WinnerUser = WinnerDto.From(gift.WinnerUser),
                ImageUrl = gift.ImageUrl ?? string.Empty,
            };
        }
       public async Task<IEnumerable<GiftResponseDto>> GetGiftsOrderByPrice()
        {
            var gifts = await _giftRepository.GetGiftsOrderByPrice();
            return gifts.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<GiftResponseDto>> GetGiftsOrderByCategory()
        {
            var gifts = await _giftRepository.GetGiftsOrderByCategory();
            return gifts.Select(MapToResponseDto);
        }

        public async Task<GiftResponseDto?> GetGiftWithWinner(int giftId)
        {
            var gift = await _giftRepository.GetGiftWithWinner(giftId);
            return gift != null ? MapToResponseDto(gift) : null;
        }

        public async Task<IEnumerable<GiftResponseDto>> GetGiftsWithWinners()
        {
            var gifts = await _giftRepository.GetGiftsWithWinners();
            return gifts.Select(MapToResponseDto);
        }


    }
}
