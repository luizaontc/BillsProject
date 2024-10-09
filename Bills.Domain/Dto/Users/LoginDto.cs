using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Domain.Dto.Users
{
    public class LoginDto : IValidatableObject
    {
        public long? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string password { get; set; }
        public bool IsChangePassword { get; set; } = false;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.IsChangePassword)
            {
                if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(username))
                {
                    yield return new ValidationResult("O Login ou Email deverão ser preenchidos.", new[] { nameof(email), nameof(username) });
                }

                if (string.IsNullOrEmpty(password))
                {
                    yield return new ValidationResult("A senha deverá ser preenchida.", new[] { nameof(password) });
                }
                else if (password.Length < 8)
                {
                    yield return new ValidationResult("A senha deve possuir no mínimo 8 caracteres.", new[] { nameof(password) });
                }
            }
        }
    }
}
