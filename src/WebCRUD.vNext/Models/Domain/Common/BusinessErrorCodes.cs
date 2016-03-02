using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCRUD.vNext.Models.Domain.Common
{
    public sealed class BusinessErrorCodes
    {
        public const string BusinessRulesViolation = "BusinessRulesViolation";
        public const string DomainModelRulesViolation = "DomainModelRulesViolation";
    }
}
