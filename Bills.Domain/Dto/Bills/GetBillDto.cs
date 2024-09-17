using Bills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Domain.Dto.Bills
{
    public class GetBillDto
    {
        public DateOnly? initialDate { get; set; }
        public DateOnly? endDate { get; set; }
        public List<Bill> bills { get; set; }
    }
}
