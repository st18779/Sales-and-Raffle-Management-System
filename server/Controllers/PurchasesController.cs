using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;
using static server.DTOs.PurchasesDTO;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesService _purchasesService;
        private readonly IUserService _usersService;

        public PurchasesController(IPurchasesService purchasesService, IUserService usersService)
        {
            _purchasesService = purchasesService;
            _usersService = usersService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GiftsWithSumTickets")]
        public async Task<ActionResult<IEnumerable<GiftsWithSumTicketsDto>>> GetGiftsWithSumOfTickets()
        {
            var giftsWithSum = await _purchasesService.GetGiftsWithSumOfTickets();
            return Ok(giftsWithSum);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllPurchasers")]
        public async Task<ActionResult<IEnumerable<GiftsWithSumTicketsDto>>> GetAllPurchasers()
        {
            var giftsWithSum = await _purchasesService.GetAllPurchasers();
            return Ok(giftsWithSum);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("TheMostPurchasedGift")]
        public async Task<ActionResult<IEnumerable<GiftsWithSumTicketsDto>>> TheMostPurchasedGift()
        {
            var gifts = await _purchasesService.TheMostPurchasedGift();
            return Ok(gifts);
        }

        [HttpPut]
        [Route("CheckOut/{userId}")]
        public async Task<ActionResult> CheckOut(string userId)
        {
            var result = await _purchasesService.CheckOut(userId);
            if (!result)
            {
                return BadRequest(new { message = "Checkout failed!" });
            }
            return Ok(new { message = "Checkout successful!" });
        }

        [HttpPost]
        [Route("AddGiftToBasket")]
        public async Task<ActionResult> AddGiftToBasket(string userId, int giftId)
        {
            var result = await _purchasesService.AddGiftToBasket(userId, giftId);
            if (!result)
            {
                return BadRequest(new { message = "AddGiftToBasket failed!" });
            }
            return Ok(new { message = "Add Gift To Basket successful!" });
        }

        [HttpPut]
        [Route("DeleteGiftFromBasket")]
        public async Task<ActionResult> DeleteGiftFromBasket(string userId, int giftId)
        {
            var result = await _purchasesService.DeleteGiftFromBasket(userId, giftId);
            if (!result)
            {
                return BadRequest(new { message = "DeleteGiftFromBasket failed!" });
            }
            return Ok(new { message = "Delete Gift From Basket successful!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("IncomeStatement")]
        public async Task<ActionResult<IncomeStatementDto>> GetIncomeStatement()
        {
            var incomeStatement = await _purchasesService.GetIncomeStatement();
            return Ok(incomeStatement);
        }

        [HttpGet]
        [Route("GetBasket/{userId}")]
        public async Task<ActionResult<IEnumerable<GiftsWithSumTicketsDto>>> GetBasket(string userId)
        {
            var user = await _usersService.GetUserById(userId);
            if (user == null)
            {
                Console.WriteLine("ddddddddddd");
                return BadRequest($"There is no user with ID {userId}");
            }
                
            var basket = await _purchasesService.GetBasket(userId);
            return Ok(basket);
        }
    }
}