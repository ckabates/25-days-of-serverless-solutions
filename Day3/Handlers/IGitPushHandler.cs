using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Day3.Handlers
{
    public interface IGitPushHandler
    {
        Task HandleRequest(string payload);
    }
}
