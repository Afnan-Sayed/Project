using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Identity
{
    public class TokenResult
    {
        public required string Token { get; set; }
        public DateTime TokenExpiryTime { get; set; }

        public required List<AppClaim> Claims { get; set; }


    }

    public class AppClaim
    {
        public required string Type { get; set; }
        public required string Value { get; set; }
    }
}
