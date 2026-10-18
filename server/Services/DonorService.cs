using server.Interfaces;
using server.Models;
using server.Repositories;
using System.ComponentModel.DataAnnotations;
using static server.DTOs.CategoryDto;
using static server.DTOs.DonorDto;
using static server.DTOs.GiftDTOs;

namespace server.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;

        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        public async Task<DonorResponseDto?> GetDonorById(string id)
        {
            var donor = await _donorRepository.GetDonorById(id);
            return donor != null ? MapToResponseDto(donor) : null;
        }

        public async Task<IEnumerable<DonorResponseDto>> GetDonorsList()
        {
            var donors = await _donorRepository.GetDonorsList();
            return donors.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<DonorResponseDto>> SearchDonorsByName(string name)
        {
            var donors = await _donorRepository.SearchDonorsByname(name);
            return donors.Select(MapToResponseDto);
        }
        public async Task<DonorResponseDto> CreateDonor(DonorCreateDto donor)
        {
            var existDonor = await _donorRepository.GetDonorById(donor.Id);
                if (existDonor != null)
                    return null;
            
            var newDonor = new Donor
            {
                Id = donor.Id,
                Name = donor.Name,
                Email = donor.Email
            };

            var createdDonor = await _donorRepository.CreateDonor(newDonor);
            return MapToResponseDto(createdDonor);
        }

        public async Task<bool> DeleteDonor(string id)
        {
            return await _donorRepository.DeleteDonor(id);
        }

        public async Task<DonorResponseDto?> UpdateDonor(string id, DonorUpdateDto donor)
        {
            var existDonor = await _donorRepository.GetDonorById(id);
            if (existDonor == null)
                return null;
            if (donor.Name != null) existDonor.Name = donor.Name;
            if(donor.Email != null) existDonor.Email = donor.Email;

            var updateDonor = await _donorRepository.UpdateDonor(existDonor);
            return updateDonor != null ? MapToResponseDto(updateDonor) : null;
        }

        private static DonorResponseDto MapToResponseDto(Donor donor)
        {
            return new DonorResponseDto
            {
                Id = donor.Id,
                Name = donor.Name,
                Email = donor.Email,
                Gifts = donor.Gifts.Select(gift => new DTOs.GiftDTOs.GiftResponseDto
                {
                    Id = gift.Id,
                    Name = gift.Name,
                    Description = gift.Description ?? string.Empty,
                    Price = gift.Price,
                    IsRandomized = gift.IsRandomized,
                    CategoryName = gift.Category?.Name ?? string.Empty,
                    DonorName = donor.Name,
                    ImageUrl = gift.ImageUrl ?? string.Empty,
                    WinnerUser = DTOs.GiftDTOs.WinnerDto.From(gift.WinnerUser)
                }).ToList() ?? new List<DTOs.GiftDTOs.GiftResponseDto>()
            };
        }
    }
}
