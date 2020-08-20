using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Simplify.SeedWork
{
    public class OpenIdUser : ValueObject
    {
        private OpenIdUser(string uniqueId, Email emailId)
        {
            UniqueId = uniqueId;
            EmailId = emailId;
        }

        public string UniqueId { get; }
        public Email EmailId { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UniqueId;
            yield return EmailId;
        }

        public static Result<OpenIdUser> Create(string uniqueId, string email)
        {
            if(string.IsNullOrWhiteSpace(uniqueId) || string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<OpenIdUser>("Invalid OpenId User");
            }

            var emailResult = Email.Create(email);
            if(emailResult.IsFailure)
            {
                return Result.Failure<OpenIdUser>("Invalid email address of OpenId User");
            }

            return Result.Success(new OpenIdUser(uniqueId, emailResult.Value));
        }
    }
}