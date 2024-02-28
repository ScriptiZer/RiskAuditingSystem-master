using AuditingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.Implements
{
    public class SplitRepository : ISplitRepository<int>
    {
        public List<int> SplitData(string data)
        {
            try
            {
                return data.Split(',').Select(int.Parse).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
