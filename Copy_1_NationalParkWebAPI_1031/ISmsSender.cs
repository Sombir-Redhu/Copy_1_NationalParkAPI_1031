using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copy_1_NationalParkWebAPI_1031
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
