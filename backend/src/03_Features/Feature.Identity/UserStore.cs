using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Simplify.SeedWork.Extensions;
using Simplify.Storage.MongoDb;

namespace Simplify.Feature.Identity
{
    public class UserStore : MongoDbCollectionBase<User>,
        IUserClaimStore<User>,
        IUserRoleStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserSecurityStampStore<User>,
        IUserLoginStore<User>,
        IUserAuthenticationTokenStore<User>,
        IUserAuthenticatorKeyStore<User>,
        IUserLockoutStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User>,
        IUserTwoFactorRecoveryCodeStore<User>,
        IQueryableUserStore<User>
    {
        private const string _internalLoginProvider = "[IdentityUserStore]";
        private const string _authenticatorKeyTokenName = "AuthenticatorKey";
        private const string _recoveryCodeTokenName = "RecoveryCodes";

        static UserStore()
        {
            BsonClassMap.RegisterClassMap<Claim>(cm =>
            {
                cm.MapConstructor(typeof(Claim).GetConstructors()
                    .First(x =>
                    {
                        var parameters = x.GetParameters();

                        return parameters.Length == 2 &&
                            parameters[0].Name == "type" &&
                            parameters[0].ParameterType == typeof(string) &&
                            parameters[1].Name == "value" &&
                            parameters[1].ParameterType == typeof(string);
                    }))
                    .SetArguments(new[]
                    {
                        nameof(Claim.Type),
                        nameof(Claim.Value)
                    });

                cm.MapMember(x => x.Type);
                cm.MapMember(x => x.Value);
            });

            BsonClassMap.RegisterClassMap<UserLoginInfo>(cm =>
            {
                cm.MapConstructor(typeof(UserLoginInfo).GetConstructors()
                    .First(x =>
                    {
                        var parameters = x.GetParameters();

                        return parameters.Length == 3;
                    }))
                    .SetArguments(new[]
                    {
                        nameof(UserLoginInfo.LoginProvider),
                        nameof(UserLoginInfo.ProviderKey),
                        nameof(UserLoginInfo.ProviderDisplayName)
                    });

                cm.AutoMap();
            });

            BsonClassMap.RegisterClassMap<IdentityUserToken<string>>(cm =>
            {
                cm.AutoMap();

                cm.UnmapMember(x => x.UserId);
            });

            BsonClassMap.RegisterClassMap<IdentityUser<Guid>>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(w => w.Id)
                    .SetSerializer(new GuidSerializer(BsonType.String));

                cm.MapMember(x => x.LockoutEnd)
                    .SetElementName("LockoutEndDateUtc");
            });
        }

        public UserStore(ILogger<MongoDbCollectionBase<User>> logger, IMongoDbContext mongoContext) : base(logger, mongoContext, true) { }

        public IQueryable<User> Users => Collection.AsQueryable();

        public Task SetTokenAsync(User user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            user.SetToken(loginProvider, name, value);


            return Task.CompletedTask;
        }

        public Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.RemoveToken(loginProvider, name);

            return Task.CompletedTask;
        }

        public async Task<string> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken) => await Task.FromResult(user.GetToken(loginProvider, name));


        public async Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken) => await SetTokenAsync(user, _internalLoginProvider, _authenticatorKeyTokenName, key, cancellationToken);

        public async Task<string> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
        {
            var result = await GetTokenAsync(user, _internalLoginProvider, _authenticatorKeyTokenName, cancellationToken);

            return await Task.FromResult(result);
        }

        public void Dispose() { }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.Id.ToString());

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.UserName);

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.NormalizedUserName);

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(user, null, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await Collection.ReplaceOneAsync(p => p.Id == user.Id, user, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await Collection.DeleteOneAsync(x => x.Id == user.Id, null, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await Collection.Find(p => p.Id == Guid.Parse(userId)).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) => await Collection.Find(p => p.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);

        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken) => Task.FromResult<IList<Claim>>(user.Claims.Where(q => !q.Value.IsNullOrWhiteSpace()).ToList());

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.AddClaims(claims);

            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            user.ReplaceClaim(claim, newClaim);


            return Task.CompletedTask;
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.RemoveClaims(claims);

            return Task.CompletedTask;
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
            => await Collection.Find(x => x.Claims.Any(q => q.Type == claim.Type && q.Value == claim.Value)).ToListAsync(cancellationToken);

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            return Task.CompletedTask;
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.Email);

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.EmailConfirmed);

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) => await Collection.Find(x => x.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync(cancellationToken);

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.Email);

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;

            return Task.CompletedTask;
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.LockoutEnd);

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd?.UtcDateTime;
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken) => Task.FromResult(user.LockoutEnabled = enabled);


        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken) => Task.FromResult(user.AccessFailedCount);

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken) => Task.FromResult(user.LockoutEnabled);

        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.AddLogin(login);

            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.RemoveLogin(loginProvider, providerKey);
            return Task.CompletedTask;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken) => Task.FromResult<IList<UserLoginInfo>>(user.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName)).ToList());

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) => await Collection.Find(x => x.Logins.Any(y => y.LoginProvider == loginProvider && y.ProviderKey == providerKey)).FirstOrDefaultAsync(cancellationToken);


        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken) => Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.PhoneNumber);

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.PhoneNumberConfirmed);

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            user.AddRole(roleName);

            await Task.CompletedTask;
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            user.RemoveRole(roleName);
            await Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.Roles.ToList());

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken) => await Task.FromResult(user.Roles.Contains(roleName));

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) => await Collection.Find(x => x.Roles.Contains(roleName)).ToListAsync(cancellationToken);


        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken) => await Task.FromResult(user.SecurityStamp = stamp);

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.SecurityStamp);

        public async Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken) => await SetTokenAsync(user, _internalLoginProvider, _recoveryCodeTokenName, string.Join(";", recoveryCodes), cancellationToken);

        public async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
        {
            var result = await GetTokenAsync(user, _internalLoginProvider, _recoveryCodeTokenName, cancellationToken);

            var splitCodes = result.Split(';');
            if(!splitCodes.Contains(code)) return await Task.FromResult(false);

            var updatedCodes = new List<string>(splitCodes.Where(s => s != code));

            await SetTokenAsync(user, _internalLoginProvider, _recoveryCodeTokenName, string.Join(";", updatedCodes), cancellationToken);

            return await Task.FromResult(true);
        }

        public async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
        {
            var codes = await GetTokenAsync(user, _internalLoginProvider, _recoveryCodeTokenName, cancellationToken);

            return await Task.FromResult(codes.Split(';').Length);
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken) => await Task.FromResult(user.TwoFactorEnabled = enabled);

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken) => await Task.FromResult(user.TwoFactorEnabled);

        public override async Task<bool> SeedDataAsync(CancellationToken ct = default)
        {
            var result = await GetAllAsync(ct);

            if(result.Any()) return await Task.FromResult(false);

            await Collection.InsertManyAsync(SeedData.GetUsers(), cancellationToken: ct);

            return await base.SeedDataAsync(ct);
        }

        protected override string CollectionName() => "identityUsers";
    }
}