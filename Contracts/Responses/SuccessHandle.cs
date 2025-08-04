using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public interface ISuccessHandle
    {
        public bool IsSuccessful { get; set; }
    }
}
