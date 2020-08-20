using CSharpFunctionalExtensions;

namespace Simplify.SeedWork
{
    public class PersonFullName : SimpleValueObject<string>
    {
        private const string _displayText = "Person Name";

        private PersonFullName(string value) : base(value) { }

        public static Result<PersonFullName> Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Result.Failure<PersonFullName>($"{_displayText} cannot be empty");

            value = value.Trim();

            return value.Length > 500 ? Result.Failure<PersonFullName>($"{_displayText} is too long") : Result.Success(new PersonFullName(value));
        }
    }
}