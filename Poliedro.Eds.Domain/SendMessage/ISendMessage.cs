using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Domain.SendMessage
{
    public interface ISendMessage
    {
        Task SendMessageAsync(string phoneNumber, string message);
    }
}
