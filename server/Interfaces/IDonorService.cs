using static server.DTOs.DonorDto;

namespace server.Interfaces
{
    public interface IDonorService
    {
        Task<DonorResponseDto?> GetDonorById(string id);
        Task<IEnumerable<DonorResponseDto>> GetDonorsList();
        Task<IEnumerable<DonorResponseDto>> SearchDonorsByName(string name);
        Task<DonorResponseDto> CreateDonor(DonorCreateDto donor);    
        Task<bool> DeleteDonor(string id);
        Task<DonorResponseDto?> UpdateDonor(string id, DonorUpdateDto donor);
        
    }
}
