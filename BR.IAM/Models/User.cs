﻿using Amazon.DynamoDBv2.DataModel;
using BR.Core.Abstractions;
using BR.Core.Helpers;
using BR.Core.Tools;
using BR.Iam.Tools;
using System;

namespace BR.Iam.Models
{
    internal class User : IDbModel
    {
        public User(string username, string email, string password)
        {
            this.Username = Validator.ReturnOrThrowIfNullOrWhiteSpace(username);
            this.Email = Validator.ReturnOrThrowIfNullOrWhiteSpace(email);
            var dbHash = Hasher.GetHash(Validator.ReturnOrThrowIfNullOrWhiteSpace(password));
            this.PasswordSalt = dbHash.Salt;
            this.PasswordHash = dbHash.Hash;
            this.Id = Guid.NewGuid();
        }

        [DynamoDBHashKey]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
