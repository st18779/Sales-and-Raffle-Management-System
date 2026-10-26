using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using server.Models;
using server.Services;
using static server.DTOs.DonorDto;

namespace server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        [Route("donorsList")]
        public async Task<ActionResult<IEnumerable<DonorResponseDto>>> GetDonorsList()
        {
            var donors = await _donorService.GetDonorsList();
            return Ok(donors);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<DonorResponseDto>> GetDonorById(string id)
            {
            var donor = await _donorService.GetDonorById(id);
            if (donor == null)
                return NotFound(new { message = $"Donor with ID {id} does not exist!" });
            return Ok(donor);
        }

        [HttpGet]
        [Route("SearchDonorsByName")]
        public async Task<ActionResult<IEnumerable<DonorResponseDto>>> SearchDonorsByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "The search function must receive a name!" });
            var donors = await _donorService.SearchDonorsByName(name);
            return Ok(donors);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DonorResponseDto>> CreateDonor([FromBody] DonorCreateDto newDonor)
        {
            try
            {
                var donor = await _donorService.CreateDonor(newDonor);
                if (donor == null)
                    return BadRequest(new { message = $"Donor with ID {newDonor.Id} already exists!" });
                return Ok(donor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDonor(string id)
        {
                var result = await _donorService.DeleteDonor(id);
                return result switch
                {
                    DeleteResult.NotFound => NotFound(new { message = $"Donor with ID {id} does not exist!" }),
                    DeleteResult.InUse => Conflict(new { message = $"Donor with ID {id} has gifts and cannot be deleted." }),
                    _ => Ok(new { message = "Deleted!" })
                };
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DonorResponseDto>> UpdateDonor(string id, [FromBody] DonorUpdateDto donor)
        {
            var updatedDonor = await _donorService.UpdateDonor(id, donor);
            if (updatedDonor == null)
                return NotFound(new { message = $"Donor with ID {id} does not exist!" });
            return Ok(updatedDonor);
        }

    }
}
