using Bills.Domain.Dto;
using Bills.Domain.Dto.Bills;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bills.Service.Services
{
    public class BillsService : IBillsService
    {
        private readonly BillsProjectContext _context;

        public BillsService(BillsProjectContext context)
        {
            _context = context;
        }

        public async Task<string> CreateBill(CreateBillDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Bill bill = new Bill()
                    {
                        BillsName = dto.billsName,
                        Description = dto.description,
                        Amount = dto.amount,
                        DueDate = dto.dueDate,
                        Status = dto.status,
                        Installments = dto.installments,
                        UserId = 1
                    };
                    await _context.Bills.AddAsync(bill);
                    await _context.SaveChangesAsync();

                    List<InstallmentBill> lInstallmentBill = new List<InstallmentBill>();

                    for (int i = 0; i < (dto.actualInstallmentNumber > 0 ? dto.actualInstallmentNumber + 1 : bill.Installments); i++)
                    {
                        InstallmentBill installmentBill = new InstallmentBill()
                        {
                            BillsId = bill.Id,
                            DueDate = CalcInstallment(bill.DueDate, i),
                            InstallmentNumber = dto.actualInstallmentNumber > 0 ? dto.actualInstallmentNumber + i : i + 1,
                            Amount = bill.Amount / bill.Installments,
                            Status = true
                        };
                        lInstallmentBill.Add(installmentBill);
                    }
                    //bill.InstallmentBills = lInstallmentBill;
                    await _context.InstallmentBills.AddRangeAsync(lInstallmentBill);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return "Ok";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }
        public async Task<GetBillDto> GetAllBills(FilterDto dto)
        {
            try
            {
                var bills = _context.Bills
                        .Include(b => b.InstallmentBills)
                        .Where(b => b.InstallmentBills.Any(i => dto.initialDate != null ? i.DueDate >= dto.initialDate : 1 == 1
                                                             && dto.endDate != null ? i.DueDate <= dto.endDate : 1 == 1))
                        .ToList();

                if (bills.Count() == 0 || bills == null)
                {
                    throw new ArgumentException("Não existem contas a serem mostradas");
                }
                GetBillDto billDto = new GetBillDto()
                {
                    initialDate = dto.initialDate,
                    endDate = dto.endDate,
                    bills = bills
                };

                return billDto;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Bill> GetBillAsync(int billId, int userId)
        {
            try
            {
                var bill = _context.Bills.Include("User").Where(x => x.Id == billId && x.UserId == userId).FirstOrDefault();

                if (bill == null)
                {
                    throw new ArgumentException("Id não encontrado.");
                }

                return bill;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateBill(int id, Bill bill)
        {
            try
            {
                var billReturn = _context.Bills.Where(x => x.Id == id).FirstOrDefault();

                if (billReturn == null)
                {
                    throw new ArgumentException();
                }

                billReturn = bill;

                _context.Update(billReturn);

                return "Ok";

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteBill(int id)
        {
            try
            {
                var bill = _context.Bills.Where(x => x.Id == id).FirstOrDefault();

                if (bill != null)
                {
                    throw new ArgumentException("Não foi possível deletar a finança.");
                }

                bill.Status = 2;
                //Adicionar alteração de status das parcelas também
                _context.Update(bill);

                return "Ok";

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Bill> GetBill(int id, int userId)
        {
            throw new NotImplementedException();
        }


        private DateOnly CalcInstallment(DateOnly dueDate, int installmentNumber)
        {
            DateOnly calcDate = new DateOnly();

            calcDate = dueDate.AddMonths(installmentNumber);

            return calcDate;
        }
    }
}
