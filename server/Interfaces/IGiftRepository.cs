using server.Models;

namespace server.Interfaces
{
    public interface IGiftRepository
    {
        Task<IEnumerable<Gift>> GetAllGifts();
        Task<Gift?> GetGiftById(int id);
        Task<IEnumerable<Gift>> GetGiftsByCategory(int categoryId);
        Task<IEnumerable<Gift>> SearchGiftByName(string name);
        Task<bool> DeleteGift(int id);
        Task<Gift> CreateGift(Gift gift);
        Task<Gift?> UpdateGift(Gift gift);
        Task<User?> RandomWinner(int id);
        Task<IEnumerable<Gift>> GetGiftsOrderByPrice();
        Task<IEnumerable<Gift>> GetGiftsOrderByCategory();
        Task<Gift?> GetGiftWithWinner(int giftId);
        Task<IEnumerable<Gift>> GetGiftsWithWinners();
        Task<IEnumerable<Gift>> SearchGiftByDonorName(string name);
    }
}
