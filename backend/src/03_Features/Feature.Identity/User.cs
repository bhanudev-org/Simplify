using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MoreLinq;
using Simplify.SeedWork.Domain;
using Simplify.SeedWork.Events;

namespace Simplify.Feature.Identity
{
    public class User : IdentityUser<Guid>, IAggregateRoot
    {
        #region Custom Properties
        [BsonElement("fn")]
        public string? FirstName { get; set; }

        [BsonElement("ln")]
        public string? LastName { get; set; }

        [BsonElement("dn")]
        public string DisplayName { get; set; } = string.Empty;

        [BsonElement("dob")]
        public DateOnly DateOfBirth { get; set; }

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonRequired]
        [BsonElement("claims")]
        public List<Claim> Claims { get; set; } = new();

        [BsonRequired]
        [BsonElement("tokens")]
        public List<IdentityUserToken<Guid>> Tokens { get; set; } = new();

        [BsonRequired]
        [BsonElement("logins")]
        public List<UserLoginInfo> Logins { get; set; } = new();

        [BsonRequired]
        [BsonElement("roles")]
        public HashSet<string> Roles { get; set; } = new();

        [BsonElement("customData")]
        public string? CustomData { get; set; }

        [BsonExtraElements]
        [BsonElement("extras")]
        public BsonDocument? Extras { get; set; }
        #endregion

        #region Helper Methods

        internal void AddLogin(UserLoginInfo login) => Logins.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName));

        internal void RemoveLogin(string loginProvider, string providerKey) => Logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

        internal void AddRole(string role) => Roles.Add(role);

        internal void RemoveRole(string role) => Roles.Remove(role);

        internal void AddClaim(Claim claim) => Claims.Add(claim);

        internal void AddClaims(IEnumerable<Claim> claims) => claims.ForEach(AddClaim);

        internal void RemoveClaim(Claim claim) => Claims.RemoveAll(c => c.Type == claim.Type && c.Value == claim.Value);

        internal void RemoveClaims(IEnumerable<Claim> claims) => claims.ForEach(RemoveClaim);

        internal void ReplaceClaim(Claim existingClaim, Claim newClaim)
        {
            RemoveClaim(existingClaim);

            AddClaim(newClaim);
        }

        internal string GetToken(string loginProvider, string name) => Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name)?.Value ?? string.Empty;

        internal void AddToken(string loginProvider, string name, string value) => Tokens.Add(new IdentityUserToken<Guid> { LoginProvider = loginProvider, Name = name, Value = value });

        internal void SetToken(string loginProvider, string name, string value)
        {
            RemoveToken(loginProvider, name);

            AddToken(loginProvider, name, value);
        }

        internal void RemoveToken(string loginProvider, string name) => Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);

        #endregion


        #region Events

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
        public IReadOnlyList<IDomainEvent> DomainEvents() => _domainEvents.AsReadOnly();
        public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

        #endregion
    }
}