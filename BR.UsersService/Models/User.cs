using BR.Core.Abstractions;
using System;

namespace BR.UsersService.Models
{
    internal class User : AggregateBase
    {
        public override Guid Id { get; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}
