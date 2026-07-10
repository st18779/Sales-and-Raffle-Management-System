using server.Models;

namespace server.Interfaces
{
    public interface IDonorRepository
    {
        Task<Donor?> GetDonorById(string id);
        Task<IEnumerable<Donor>> GetDonorsList();
        Task<IEnumerable<Donor>> SearchDonorsByname(string name);
        Task<bool> DeleteDonor(string id);
        Task<Donor?> CreateDonor(Donor donor);
        Task<Donor?> UpdateDonor(Donor donor);

    }
}
