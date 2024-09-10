using Bills.Domain.Dto;
using Bills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Service.Interface
{
    public interface IBillsService
    {
        string CreateBill(Bill bill);
        Bill GetBill(int id, int userId);
        Task<List<Bill>> GetAllBills(FilterDto dto);
        string UpdateBill(int id,Bill bill);
        string DeleteBill(int id);
        Task<Bill> GetBillAsync(int id, int userId);
    }
}
