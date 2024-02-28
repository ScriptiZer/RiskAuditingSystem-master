using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.Interfaces
{
    public interface ISplitRepository<T>
    {
        List<T> SplitData(string data);
    }
}
