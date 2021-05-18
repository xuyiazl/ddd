using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;


namespace DDD.Domain.AdminUsers
{
    public class AdminUserQueryList : CommandLimit<IList<AdminUserDto>>
    {
        public string Keyword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandLimitValidator<AdminUserQueryList, IList<AdminUserDto>>
        {
            public Validator()
            {
                AddLimitVaildator();
            }
        }

        public class Handler : CommandHandler<AdminUserQueryList, IList<AdminUserDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<AdminUserDto>> Handle(AdminUserQueryList request, CancellationToken cancellationToken)
            {
                // 仓储提供的单表查询

                //Expression<Func<AdminUser, bool>> selector = c => true;

                //selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

                //var list = await db.GetListAsync(
                //    selector: selector,
                //    orderby: "Id desc",
                //    limit: request.Limit,
                //    cancellationToken: cancellationToken);

                //if (list != null)
                //    return (SubCode.Success, mapper.ToResult<List<AdminUser>, IList<AdminUserDto>>(list));

                //return (SubCode.Fail, default);

                // ef 直接查询

                var list = await db.Context.AdminUser
                     .Where(c => c.Status == Status.Show)
                     .WhereIf(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty())
                     .OrderBy(c => c.Id)
                     .Take(request.Limit)
                     .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);

                return list;
            }
        }
    }
}
