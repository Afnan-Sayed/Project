using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Application.DTOs.User
{
    public class UserRegisterDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime? BirthDate { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string UserName { get; set; }

        public string[]? Roles { get; set; }
    }
}
