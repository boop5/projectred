using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentValidation.Results;

namespace Red.Core.Application.Exceptions
{
    internal sealed class ValidationException : Exception
    {
        private readonly JsonSerializerOptions _toStringSerializerOptions = new() {WriteIndented = true};
        
        private readonly IDictionary<string, string[]> _errors = new Dictionary<string, string[]>();
        public IReadOnlyDictionary<string, string[]> Errors => _errors.ToDictionary(x => x.Key, x => x.Value);

        public ValidationException(Exception? innerException = null)
            : base("One or more validation failures have occurred.", innerException)
        {
        }

        public ValidationException(Exception innerException, IEnumerable<ValidationFailure> failures) 
            : this(innerException)
        {
            SaveGroupedErrors(failures);
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            SaveGroupedErrors(failures);
        }

        private void SaveGroupedErrors(IEnumerable<ValidationFailure> failures)
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                _errors.Add(propertyName, propertyFailures);
            }
        }

        public override string ToString()
        {
            var json = JsonSerializer.Serialize(this, _toStringSerializerOptions);
            
            return $"{nameof(ValidationException)}\t{json}";
        }
    }
}