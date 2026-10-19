using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly ChineseSaleContext _context;
        public GiftRepository(ChineseSaleContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gift>> GetAllGifts()
        {
            return await _context.Gifts
                         .Include(g => g.Category)
                         .Include(g => g.Donor)
                         .Include(g => g.WinnerUser)
                         .ToListAsync();
        }

        public async Task<Gift?> GetGiftById(int id)
        {
            return await _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.WinnerUser)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Gift>> GetGiftsByCategory(int categoryId)
        {
            return await _context.Gifts
                .Where(g => g.Category.Id == categoryId)
                .Include(g => g.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gift>> SearchGiftByName(string name)
        {
            return await _context.Gifts
                .Where(g => g.Name.Contains(name))
                .Include(g => g.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gift>> SearchGiftByDonorName(string name)
        {
            return await _context.Gifts
                .Where(g => g.Donor.Name.Contains(name))
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .ToListAsync();
        }

        public async Task<Gift> CreateGift(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            return gift;
        }
        public async Task<DeleteResult> DeleteGift(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
                return DeleteResult.NotFound;
            if (await _context.purchases.AnyAsync(p => p.GiftId == id))
                return DeleteResult.InUse;
            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
            return DeleteResult.Deleted;
        }

        public async Task<Gift?> UpdateGift(Gift gift)
        {
            var existGift = await _context.Gifts.FindAsync(gift.Id);
            if (existGift == null)
                return null;
            existGift.Name = gift.Name;
            existGift.Description = gift.Description;
            existGift.Price = gift.Price;
            existGift.ImageUrl = gift.ImageUrl;
            existGift.CategoryId = gift.CategoryId;
            existGift.DonorId = gift.DonorId;
            await _context.SaveChangesAsync();
            return existGift;
        }

        public async Task<User?> RandomWinner(int id)
        {
            var giftCheck = await _context.Gifts
            .Include(g => g.WinnerUser)
            .FirstOrDefaultAsync(g => g.Id == id);

            if (giftCheck != null && giftCheck.IsRandomized && giftCheck.WinnerUser != null)
            {
                return giftCheck.WinnerUser; 
            }
            var purchases = await _context.purchases
                            .Include(p => p.User)
                            .Where(p => p.GiftId == id && p.IsClosed)
                            .ToListAsync();
            if (!purchases.Any())
                return null;

            List<User> users = new();
            foreach (var purchase in purchases)
            {
                for (int i = 0; i < purchase.NumOfTickets; i++)
                {
                    users.Add(purchase.User);
                }
            }
            if (!users.Any())
                return null;

            Random random = new();
            int winnerIndex = random.Next(users.Count);
            var curGift = await GetGiftById(id);
            if (curGift != null)
            {
                curGift.WinnerUserId = users[winnerIndex].Id;
                curGift.IsRandomized = true;
                await _context.SaveChangesAsync();
            }
            return users[winnerIndex];
        }

        public async Task<IEnumerable<Gift>> GetGiftsOrderByPrice()
        {
            var gifts = await _context.Gifts
                            .OrderByDescending(g => g.Price)
                            .Include(g => g.Category)
                            .ToListAsync();
            return gifts;
        }

        public async Task<IEnumerable<Gift>> GetGiftsOrderByCategory()
        {
            var gifts = await _context.Gifts
                            .OrderBy(g => g.Category.Name)
                            .Include(g => g.Category)
                            .ToListAsync();
            return gifts;
        }

        public async Task<Gift?> GetGiftWithWinner(int giftId)
        {
            var gift = await _context.Gifts
                             .Include(g => g.WinnerUser)
                             .Include(g => g.Category)
                             .Include(g => g.Donor)
                            .FirstOrDefaultAsync(g => g.Id == giftId && g.WinnerUserId != null);
                          
            return gift;
        }

        public async Task<IEnumerable<Gift>> GetGiftsWithWinners()
        {
            var gifts = await _context.Gifts
                             .Where(g => g.WinnerUserId != null)
                             .Include(g => g.WinnerUser)
                             .Include(g => g.Category)
                             .Include(g => g.Donor)
                             .ToListAsync();
            return gifts;
        }
    }
}
