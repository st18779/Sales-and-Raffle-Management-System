using server.Models;
using System.ComponentModel.DataAnnotations;
using static server.DTOs.UserDto;

namespace server.DTOs
{
    public class PurchasesDTO
    {
        public class GiftsWithSumTicketsDto
        {
            public int GiftId { get; set; }
            public string GiftName { get; set; } = string.Empty;
            public int TotalTickets { get; set; }
            public int Price { get; set; } = 10;
            //public List<UserResponseDto> Users { get; set; } = new();
        }

        public class GiftsWithTotalIncomeDto
        {
            public int GiftId { get; set; }
            public string GiftName { get; set; } = string.Empty;
            public int TotalIncome { get; set; }
            //public List<UserResponseDto> Users { get; set; } = new();
        }

        public class IncomeStatementDto
        {
            public List<GiftsWithTotalIncomeDto> GiftsIncome { get; set; } = new();
            public int TotalIncome { get; set; }
        }

    }
}