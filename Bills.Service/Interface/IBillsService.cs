using Bills.Domain.Dto;
using Bills.Domain.Dto.Bills;
using Bills.Domain.Entities;

namespace Bills.Service.Interface
{
    public interface IBillsService
    {
        Task<string> CreateBill(CreateBillDto bill);
        Task<Bill> GetBill(int id, int userId);
        Task<GetBillDto> GetAllBills(FilterDto dto);
        Task<string> UpdateBill(int id,Bill bill);
        Task<string> DeleteBill(int id);
        Task<Bill> GetBillAsync(int id, int userId);
    }
}
