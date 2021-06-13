using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DDD.Domain.Core;
using XUCore.Ddd.Domain.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Sys.AdminMenu
{
    /// <summary>
    /// 查询导航记录命令
    /// </summary>
    public class AdminMenuQueryDetail : CommandId<AdminMenuDto, long>
    {
        public class Validator : CommandIdValidator<AdminMenuQueryDetail, AdminMenuDto, long>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<AdminMenuQueryDetail, AdminMenuDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<AdminMenuDto> Handle(AdminMenuQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthMenus
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}
