using server.DTOs;
using server.Interfaces;
using server.Repositories;
using static server.DTOs.PurchasesDTO;
using static server.DTOs.UserDto;

namespace server.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly IPurchasesRepository _purchasesRepository;
        public PurchasesService(IPurchasesRepository purchasesRepository)
        {
            _purchasesRepository = purchasesRepository;
        }

        public async Task<IEnumerable<GiftsWithSumTicketsDto>> GetGiftsWithSumOfTickets()
        {
            var purchases = await _purchasesRepository.GetGiftsWithSumOfTickets();
            var result = purchases.Select(p => new GiftsWithSumTicketsDto
            {
                GiftId = p.GiftId,
                GiftName = p.GiftName,
                TotalTickets = p.TotalTickets,
            });
            return result;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllPurchasers()
        {
            var purchasers = await _purchasesRepository.GetAllPurchasers();
            return purchasers;
        }

        public async Task<IEnumerable<GiftsWithSumTicketsDto>> TheMostPurchasedGift()
        {
            var gifts = await _purchasesRepository.TheMostPurchasedGift();
            return gifts;
        }

        public async Task<bool> CheckOut(string userId)
        {
            return await _purchasesRepository.CheckOut(userId);
        }

        public async Task<bool> AddGiftToBasket(string userId, int giftId)
        {
            return await _purchasesRepository.AddGiftToBasket(userId, giftId);
        }

        public async Task<bool> DeleteGiftFromBasket(string userId, int giftId)
        {
            return await _purchasesRepository.DeleteGiftFromBasket(userId, giftId);
        }

        public async Task<IncomeStatementDto> GetIncomeStatement()
        {
            return await _purchasesRepository.GetIncomeStatement();
        }

        public async Task<IEnumerable<GiftsWithSumTicketsDto>> GetBasket(string userId)
        {
            return await _purchasesRepository.GetBasket(userId);
        }
    }
}
