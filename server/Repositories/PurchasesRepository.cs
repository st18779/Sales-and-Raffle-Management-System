using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Interfaces;
using server.Models;
using static server.DTOs.PurchasesDTO;
using static server.DTOs.UserDto;

namespace server.Repositories
{
    public class PurchasesRepository : IPurchasesRepository
    {
        private readonly ChineseSaleContext _context;
        private readonly IGiftRepository _giftRepository;
        public PurchasesRepository(ChineseSaleContext context, IGiftRepository giftRepository)
        {
            _context = context;
            _giftRepository = giftRepository;
        }

        public async Task<IEnumerable<GiftsWithSumTicketsDto>> GetGiftsWithSumOfTickets()
        {
            var purchases = await _context.purchases
                            .Where(p => p.IsClosed == true)
                            .GroupBy(p => new { p.GiftId, p.Gift.Name})
                            .Select(group => new GiftsWithSumTicketsDto
                            {
                                GiftId = group.Key.GiftId,
                                GiftName = group.Key.Name,
                                TotalTickets = group.Sum(p => p.NumOfTickets),
                                //Users = group.Select(p => new UserWithSumTicketsDto
                                //{
                                //    Id = p.User.Id,
                                //    FullName = p.User.FullName,
                                //    NumOfTickets = p.NumOfTickets
                                //}).ToList()
                            }).ToListAsync();
            return purchases;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllPurchasers()
        {
            var purchases = await _context.purchases
                            .Where(p => p.IsClosed == true)
                            .GroupBy(p => new { p.User.Id, p.User.FullName, p.User.Email, p.User.Phone })
                            .Select(group => new UserResponseDto
                            {
                                Id = group.Key.Id,
                                FullName = group.Key.FullName,
                                Email = group.Key.Email,
                                Phone = group.Key.Phone,
                            }).ToListAsync();

            return purchases;
        }

        public async Task<IEnumerable<GiftsWithSumTicketsDto>> TheMostPurchasedGift()
        {
            var gifts = await GetGiftsWithSumOfTickets();
            var result = gifts.OrderByDescending(g => g.TotalTickets);
            return result;
        }

        public async Task<bool> CheckOut(string userId)
        {
            var users = await _context.purchases
                            .Where(p => p.UserId == userId)
                            .ToListAsync();
            if(!users.Any())
                return false;
            foreach (var user in users) {
                user.IsClosed = true;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddGiftToBasket(string userId, int giftId)
        {
            var isRandomizedGift = await _giftRepository.GetGiftWithWinner(giftId);
            if(isRandomizedGift?.WinnerUser != null)
                return false;

            var gift = await _context.purchases
                      .FirstOrDefaultAsync(p => p.UserId == userId && p.GiftId == giftId && !p.IsClosed);
            if(gift != null){
                gift.NumOfTickets += 1;
                await _context.SaveChangesAsync();
                return true;
            }
            else{
                var newPurchase = new Purchases{GiftId= giftId,UserId=userId,NumOfTickets=1,IsClosed=false};
                 _context.purchases.Add(newPurchase);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteGiftFromBasket(string userId, int giftId)
        {

            var gift = await _context.purchases
                      .FirstOrDefaultAsync(p => p.UserId == userId && p.GiftId == giftId && !p.IsClosed);
            if (gift != null)
            {
                if(gift.NumOfTickets > 1)
                {
                    gift.NumOfTickets -= 1;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _context.purchases.Remove(gift);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;

        }

        public async Task<IncomeStatementDto> GetIncomeStatement()
        {
            var incomeStatement = await _context.purchases
                .Where(p => p.IsClosed == true)
                .GroupBy(p => new { p.GiftId, p.Gift.Name, p.Gift.Price })
                .Select(group => new GiftsWithTotalIncomeDto
                {
                    GiftId = group.Key.GiftId,
                    GiftName = group.Key.Name,
                    TotalIncome = group.Sum(p => p.NumOfTickets) * group.Key.Price
                }).ToListAsync();

            int TotalIncome = incomeStatement.Sum(g => g.TotalIncome);

            var incomeStatementDto = new IncomeStatementDto
            {
                GiftsIncome = incomeStatement,
                TotalIncome = TotalIncome
            };
            return incomeStatementDto;

        }

        public async Task<IEnumerable<GiftsWithSumTicketsDto>> GetBasket(string userId)
        {
            var basket = await _context.purchases
                               .Include(p => p.Gift)
                               .Where(p => p.UserId == userId && !p.IsClosed)
                               .Select(p => new GiftsWithSumTicketsDto
                               {
                                   GiftId = p.GiftId,
                                   GiftName = p.Gift.Name,
                                   TotalTickets = p.NumOfTickets,
                                   Price = p.Gift.Price
                               })
                               .ToListAsync();
            return basket;
        }

        // Replace all occurrences of '_context.Purchases' with '_context.purchases'
        /* public async Task<IEnumerable<GiftsWithSumDto>> GetGiftsWithSum()
         {
             var purchases = await _context.purchases
                                   .Where(p => p.IsClosed == true)
                                   .Join(_context.Gifts,
                                         p => p.GiftId,
                                         g => g.Id,
                                         (p, g) => new {p,g})
                                   .GroupBy(x => new { x.g.Id, x.g.Name })
                                   .Select(group => new GiftsWithSumDto
                                   {
                                     GiftId = group.Key.Id,
                                     GiftName = group.Key.Name,
                                     TotalTickets = group.Sum(x  => x.p.NumOfTickets)
                                   })
                                   .ToListAsync();
             //foreach (var purchase in purchases)
             //{
             //    Console.WriteLine(purchase);
             //}
             return purchases;
         }*/
        //public async Task<IEnumerable<GiftsWithUsersDto>> GetGiftsWithUsers()
        //{
        //    var purchases = await _context.purchases
        //                          .Where(p => p.IsClosed == true)
        //                          .Join(_context.Gifts,
        //                                p => p.GiftId,
        //                                g => g.Id,
        //                                (p, g) => new { p, g })
        //                          .Join(_context.Users,
        //                                pg => pg.p.UserId,
        //                                u => u.Id,
        //                                (pg, u) => new { pg, u })
        //                          .GroupBy(x => new { x.pg.g.Id, x.pg.g.Name })
        //                          .Select(group => new GiftsWithUsersDto
        //                          {
        //                              GiftId = group.Key.Id,
        //                              GiftName = group.Key.Name,
        //                              Users = group.Select(x => new UserWithSumTicketsDto
        //                              {
        //                                  Id = x.u.Id,
        //                                  FullName = x.u.FullName,
        //                                  NumOfTickets = x.pg.p.NumOfTickets
        //                              }).ToList()
        //                          })
        //                          .ToListAsync();

        //    return purchases;
        //}



        //var purchases = await _context.purchases
        //                      .Where(p => p.IsClosed == true)
        //                      .Join(_context.Gifts,
        //                            p => p.GiftId,
        //                            g => g.Id,
        //                            (p, g) => new {p,g})


        //.GroupBy(x => new { x.g.Id, x.g.Name })
        //.Join(_context.Users,
        //      pg => pg.p.UserId,
        //      u => u.Id,
        //      (pg, u) => new { pg, u })
        //.GroupBy(x => new { x.pg.g.Id, x.pg.g.Name })
        //.Select(group => new GiftsWithUsersDto
        //{
        //    GiftId = group.Key.Id,
        //    GiftName = group.Key.Name,
        //    Users = group.Select(x => new UserDto
        //    {
        //        UserId = x.u.Id,
        //        UserName = x.u.Name,
        //        NumOfTickets = x.pg.p.NumOfTickets
        //    }).ToList()
        //})
        //.ToListAsync();










        // return purchases;


    }
}
