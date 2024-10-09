using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bills.Service.Interface.Shared
{
    public interface ISendMailService
    {
        Task<bool> SendMail(string sender, string recipients, string subject, string body, string cc, string cco);
    }
}
