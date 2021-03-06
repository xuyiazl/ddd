using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Common;
using DDD.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserQueryById : CommandId<AdminUserDto, long>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandIdValidator<AdminUserQueryById, AdminUserDto, long>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<AdminUserQueryById, AdminUserDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [RedisCacheMethod(HashKey = RedisKey.Admin, Key = "{Id}")]
            public override async Task<AdminUserDto> Handle(AdminUserQueryById request, CancellationToken cancellationToken)
            {
                return await db.Context.AdminUser
                    .Where(c => c.Id == request.Id && c.Status == Status.Show)
                    .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}
