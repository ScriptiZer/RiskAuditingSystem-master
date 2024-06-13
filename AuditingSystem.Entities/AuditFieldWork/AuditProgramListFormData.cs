using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditFieldWork
{
    public class AuditProgramListFormData
    {
        public string? Description { get; set; }
        public string? TestResult { get; set; }
        public List<IFormFile>? ReferenceFiles { get; set; }
    }

}
