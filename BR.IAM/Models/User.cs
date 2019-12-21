using Amazon.DynamoDBv2.DataModel;
using BR.Core;
using BR.Core.Abstractions;
using BR.Core.Tools;
using BR.Iam.Tools;
using System;

namespace BR.Iam.Models
{
    public class User : IAggregate, IDbModel
    {
        public User()
        {
        }

        public User(string username, string email, string password)
        {   
            this.Username = Validator.ReturnOrThrowIfNullOrWhiteSpace(username);
            this.Email = Validator.ReturnOrThrowIfNullOrWhiteSpace(email);
            var dbHash = Hasher.GetPasswordHash(Validator.ReturnOrThrowIfNullOrWhiteSpace(password));
            this.PasswordHash = dbHash.Hash;
            this.PasswordSalt = dbHash.Salt;
            this.Id = Hasher.GetHashString($"{username}|{email}");
        }

        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public static User CreateNull()
        {
            return new User(Constants.NA, Constants.NA, Constants.NA);
        }
    }
}
