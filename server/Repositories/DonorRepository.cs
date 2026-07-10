using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class DonorRepository: IDonorRepository
    {
        private readonly ChineseSaleContext _context;
        public DonorRepository(ChineseSaleContext context)
        {
            _context = context;
        }
        public async Task<Donor?> GetDonorById(string id)
        {
            return await _context.Donors  
                        .Include(d => d.Gifts)
                        .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Donor>> GetDonorsList()
        {
            return await _context.Donors
                        .Include(d => d.Gifts)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Donor>> SearchDonorsByname(string name)
        {
            return await _context.Donors
                .Where(d => d.Name.Contains(name))
                .Include(d => d.Gifts)
                .ToListAsync();
        }

        public async Task<Donor?> CreateDonor(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            return donor;
        }
        public async Task<bool> DeleteDonor(string id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor == null)
                return false;
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Donor?> UpdateDonor(Donor donor)
        {
            var existDonor = await _context.Donors.FindAsync(donor.Id);
            if (existDonor == null)
                return null;
            existDonor.Name = donor.Name;
            existDonor.Email = donor.Email;
            await _context.SaveChangesAsync();
            return existDonor;
        }
    }
}
