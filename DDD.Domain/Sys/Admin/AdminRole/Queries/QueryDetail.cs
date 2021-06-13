using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace DDD.Domain.Sys.AdminRole
{
    /// <summary>
    /// 查询一条角色记录命令
    /// </summary>
    public class AdminRoleQueryDetail : CommandId<AdminRoleDto, long>
    {
        public class Validator : CommandIdValidator<AdminRoleQueryDetail, AdminRoleDto, long>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<AdminRoleQueryDetail, AdminRoleDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<AdminRoleDto> Handle(AdminRoleQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthRole
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}