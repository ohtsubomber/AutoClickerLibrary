using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Engine
{
    public interface IEngine
    {
        Task ExecuteScriptAsync(string script);
    }
}
