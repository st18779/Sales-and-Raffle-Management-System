using server.Models;
using static server.DTOs.GiftDTOs;

namespace server.Interfaces
{
    public interface IGiftService
    {
        Task<IEnumerable<GiftResponseDto>> GetAllGifts();
        Task<GiftResponseDto?> GetGiftById(int id);
        Task<IEnumerable<GiftResponseDto>> GetGiftsByCategory(int categoryId);
        Task<IEnumerable<GiftResponseDto>> SearchGiftByName(string name);
        Task<bool> DeleteGift(int id);
        Task<GiftResponseDto> CreateGift(GiftCreateDto gift);
        Task<GiftResponseDto?> UpdateGift(int id,GiftUpdateDto gift);
        Task<User?> RandomWinner(int id);
        Task<IEnumerable<GiftResponseDto>> GetGiftsOrderByPrice();
        Task<IEnumerable<GiftResponseDto>> GetGiftsOrderByCategory();
        Task<GiftResponseDto?> GetGiftWithWinner(int giftId);
        Task<IEnumerable<GiftResponseDto>> GetGiftsWithWinners();
        Task<IEnumerable<GiftResponseDto>> SearchGiftByDonorName(string name);
    }
}
