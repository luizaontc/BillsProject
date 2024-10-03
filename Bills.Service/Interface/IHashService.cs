using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Service.Interface
{
    public interface IHashService
    {
        byte[] GenerateSalt(int size);
        byte[] ComputeHash(string password, byte[] salt);
        bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt);
    }
}
