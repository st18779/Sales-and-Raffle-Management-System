using server.Models;
using static server.DTOs.PurchasesDTO;
using static server.DTOs.UserDto;

namespace server.Interfaces
{
    public interface IPurchasesRepository
    {
        Task<IEnumerable<GiftsWithSumTicketsDto>> GetGiftsWithSumOfTickets();
        Task<IEnumerable<UserResponseDto>> GetAllPurchasers();
        Task<IEnumerable<GiftsWithSumTicketsDto>> TheMostPurchasedGift();
        Task<bool> CheckOut(string userId);
        Task<bool> AddGiftToBasket(string userId, int giftId);
        Task<bool> DeleteGiftFromBasket(string userId, int giftId);
        Task<IncomeStatementDto> GetIncomeStatement();
        Task<IEnumerable<GiftsWithSumTicketsDto>> GetBasket(string userId);
    } 
}
