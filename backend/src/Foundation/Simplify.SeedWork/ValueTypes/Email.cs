using System;
using System.Globalization;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Simplify.SeedWork
{
    public class Email : SimpleValueObject<string>
    {
        private const string _displayText = "Email";

        private Email(string value) : base(value) { }

        public static Result<Email> Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Result.Failure<Email>($"{_displayText} cannot be empty");

            value = value.Trim().ToLowerInvariant();

            if(value.Length > 500)
                return Result.Failure<Email>($"{_displayText} is too long");

            var defaultError = Result.Failure<Email>($"Invalid {_displayText}");

            try
            {
                // Normalize the domain
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return defaultError;
            }
            catch (ArgumentException)
            {
                return defaultError;
            }

            try
            {
                if(!Regex.IsMatch(value, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                    return defaultError;
            }
            catch(RegexMatchTimeoutException)
            {
                return defaultError;
            }

            return Result.Success(new Email(value));
        }
    }
}