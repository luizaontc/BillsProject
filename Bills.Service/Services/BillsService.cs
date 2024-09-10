using Bills.Domain.Dto;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Service.Services
{
    public class BillsService : IBillsService
    {
        private readonly BillsProjectContext _context;

        public BillsService(BillsProjectContext context)
        {
            _context = context;
        }

        public string CreateBill(Bill bill)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Bills.Add(bill);
                    _context.SaveChanges();

                    List<InstallmentBill> lInstallmentBill = new List<InstallmentBill>();

                    for (int i = 0; i < bill.Installments; i++)
                    {
                        InstallmentBill installmentBill = new InstallmentBill()
                        {
                            BillsId = bill.Id,
                            DueDate = CalcInstallment(bill.DueDate, i),
                            InstallmentNumber = i + 1,
                            Amount = bill.Amount / bill.Installments,
                            Status = true
                        };
                        lInstallmentBill.Add(installmentBill);
                    }
                    //bill.InstallmentBills = lInstallmentBill;
                    _context.InstallmentBills.AddRange(lInstallmentBill);

                    _context.SaveChanges();

                    transaction.Commit();

                    return "Ok";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        public async Task<List<Bill>> GetAllBills(FilterDto dto)
        {
            try
            {
                var bills = _context.Bills
                        .Include(b => b.InstallmentBills)
                        .Where(b => b.InstallmentBills.Any(i => i.DueDate >= dto.initialDate && i.DueDate <= dto.endDate))
                        .ToList();

                if (bills.Count() == 0 || bills == null)
                {
                    throw new ArgumentException("Não existem contas neste período.");
                }

                return bills;
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

        public string UpdateBill(int id, Bill bill)
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

        public string DeleteBill(int id)
        {
            try
            {
                var bill = _context.Bills.Where(x => x.Id == id).FirstOrDefault();

                if (bill != null)
                {
                    throw new ArgumentException("Não foi possível deletar a finança.");
                }

                bill.Status = false;
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

        public Bill GetBill(int id, int userId)
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
