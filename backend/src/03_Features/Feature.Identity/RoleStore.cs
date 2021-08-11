using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Simplify.Storage.MongoDb;

namespace Simplify.Feature.Identity
{
    public class RoleStore : MongoDbCollectionBase<Role>, IRoleStore<Role>
    {
        public RoleStore(ILogger<MongoDbCollectionBase<Role>> logger, IMongoDbContext mongoDbContext) : base(logger, mongoDbContext, true) { }

        public IQueryable<Role> Roles => Collection.AsQueryable();

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await Collection.Find(x => x.Id == Guid.Parse(roleId)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await Collection.Find(x => x.NormalizedName == normalizedRoleName).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(role, null, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            await Collection.ReplaceOneAsync(x => x.Id == role.Id, role, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            await Collection.DeleteOneAsync(x => x.Id == role.Id, null, cancellationToken);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;

            return Task.CompletedTask;
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;

            return Task.CompletedTask;
        }

        protected override string CollectionName() => "identityRoles";

        protected override Task SetupCollectionAsync(CancellationToken ct = default)
        {
            return Collection.Indexes.CreateOneAsync(
                new CreateIndexModel<Role>(Index.Ascending(q => q.NormalizedName), new CreateIndexOptions { Unique = true }),
                cancellationToken: ct);
        }

        public void Dispose() { }
    }
}
