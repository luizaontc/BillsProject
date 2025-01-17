﻿using Bills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Domain.Dto.Bills
{
    public class BillDto
    {
        public int? userId { get; set; }
        public string billsName { get; set; }
        public string? description { get; set; }
        public DateOnly dueDate { get; set; }
        public int installments { get; set; }
        public int? actualInstallmentNumber { get; set; } = 0;
        public decimal amount { get; set; }
        public int status { get; set; }
    }
}
