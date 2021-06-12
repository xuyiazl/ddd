using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace DDD.Domain.Core
{
    public static class Throw
    {
        public static void Validation(List<ValidationFailure> failures)
        {
            throw new XUCore.Ddd.Domain.Exceptions.ValidationException(failures);
        }

        public static void Validation(string error) => Validation("", error);

        public static void Validation(string propertyName, string error)
        {
            throw new XUCore.Ddd.Domain.Exceptions.ValidationException(new List<ValidationFailure> { new ValidationFailure(propertyName, error) });
        }

        public static bool IsValidation(this Exception ex)
        {
            return ex is XUCore.Ddd.Domain.Exceptions.ValidationException;
        }
    }
}
