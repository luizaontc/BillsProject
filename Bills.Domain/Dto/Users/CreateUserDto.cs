using Bills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Domain.Dto.Users
{
    public class CreateUserDto
    {

        public string? name { get; set; }

        public DateOnly? birthday { get; set; }

        public string? document { get; set; }

        public int? status { get; set; } = 1;

        public int? currency { get; set; } = 1;
    }
}
