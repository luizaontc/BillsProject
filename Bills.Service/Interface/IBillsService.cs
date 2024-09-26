using Bills.Domain.Dto;
using Bills.Domain.Dto.Bills;
using Bills.Domain.Entities;

namespace Bills.Service.Interface
{
    public interface IBillsService
    {
        Task<string> CreateBill(BillDto bill);
        Task<GetBillDto> GetAllBills(FilterDto dto);
        Task<string> UpdateBill(int id,BillDto bill);
        Task<Bill> GetBillAsync(int id, int userId);
    }
}
