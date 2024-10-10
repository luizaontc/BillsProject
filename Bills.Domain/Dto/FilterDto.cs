using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Domain.Dto
{
    public class FilterDto
    {
        public DateOnly? initialDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string? textFilter { get; set; }
        public int? category { get; set; }
        public int? userId { get; set; }
    }
}
