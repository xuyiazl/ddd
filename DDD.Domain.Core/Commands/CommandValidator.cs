using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Core.Commands
{
    public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
    {
    }
}
