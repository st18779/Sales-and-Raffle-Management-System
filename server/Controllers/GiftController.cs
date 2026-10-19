using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using server.Models;
using static server.DTOs.GiftDTOs;
using static server.DTOs.UserDto;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController: ControllerBase
    {
        private readonly IGiftService _giftService;
        
        public GiftController(IGiftService giftService)
        {
            _giftService = giftService;
        }

        [HttpGet]
        [Route("giftsList")]
        //[Authorize(Roles = "Admin,Donor")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetAllGifts()
        {
            var gifts = await _giftService.GetAllGifts();
            return Ok(gifts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GiftResponseDto>> GetGiftById(int id)
        {
            var gift = await _giftService.GetGiftById(id);
            if(gift == null) 
                return NotFound(new {message = $"Gift with ID {id} is not exist!"});
            return Ok(gift);
        }
        [HttpGet("GiftsByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetGiftsByCategory(int categoryId)
        {
            var gifts = await _giftService.GetGiftsByCategory(categoryId);
            return Ok(gifts);
        }

        [HttpGet("SearchGiftByName")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> SearchGiftByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "The search func must get a name!" });
            var gifts = await _giftService.SearchGiftByName(name);
            return Ok(gifts);
        }

        [HttpGet("SearchGiftByDonorName")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> SearchGiftByDonorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "The search func must get a name!" });
            var gifts = await _giftService.SearchGiftByDonorName(name);
            return Ok(gifts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> CreateGift([FromBody] GiftCreateDto newGift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var gift = await _giftService.CreateGift(newGift);
                return Ok(gift); //CreatedAtAction(nameof(GetById), new { id = product.Id }, product); אולי 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> UpdateGift(int id, [FromBody] GiftUpdateDto updGift)
        {
            try
            {
                var gift = await _giftService.UpdateGift(id, updGift);
                if (gift == null)
                    return NotFound(new { message = $"Gift with ID {id} is not exist!" });
                return Ok(gift);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            var result = await _giftService.DeleteGift(id);
            return result switch
            {
                DeleteResult.NotFound => NotFound(new { message = $"Gift with ID {id} does not exist!" }),
                DeleteResult.InUse => Conflict(new { message = $"Gift with ID {id} already has purchases and cannot be deleted." }),
                _ => Ok(new { message = "Deleted!" })
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("randomWinner")]
        public async Task<ActionResult<WinnerDto>> RandomWinner([FromBody] int id)
        {
            try
            {
                var winnerUser = await _giftService.RandomWinner(id);
                return Ok(WinnerDto.From(winnerUser));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GiftsOrderByPrice")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetGiftsOrderByPrice()
        {
            var gifts = await _giftService.GetGiftsOrderByPrice();
            return Ok(gifts);
        }

        [HttpGet("GetGiftsOrderByCategory")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetGiftsOrderByCategory()
        {
            var gifts = await _giftService.GetGiftsOrderByCategory();
            return Ok(gifts);
        }

        [HttpGet("GetGiftWithWinner/{giftId}")]
        public async Task<ActionResult<GiftResponseDto>> GetGiftWithWinner(int giftId)
        {
            var gift = await _giftService.GetGiftWithWinner(giftId);
            if (gift == null)
                return NotFound(new { message = $"Gift with ID {giftId} is not exist, or this gift is not randomized!" });
            return Ok(gift);
        }

        [HttpGet("GetGiftsWithWinners")]
        public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetGiftsWithWinners()
        {
            var gifts = await _giftService.GetGiftsWithWinners();
            return Ok(gifts);
        }
    }
}
