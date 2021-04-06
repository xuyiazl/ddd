using AutoMapper;
using DDD.Applaction.Common;
using DDD.Applaction.Dtos;
using DDD.Domain.AdminUsers.Commands.Create;
using DDD.Domain.AdminUsers.Commands.Delete;
using DDD.Domain.AdminUsers.Commands.Update;
using DDD.Domain.AdminUsers.Queries.GetDetail;
using DDD.Domain.AdminUsers.Queries.GetList;
using DDD.Domain.AdminUsers.Queries.GetPagedList;
using DDD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Services
{
    /// <summary>
    /// 管理员账号管理
    /// </summary>
    public class AdminUserAppService : AppService
    {
        public AdminUserAppService(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

        /// <summary>
        /// 增加记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        public async Task<Result<int>> CreateAsync(CreateAdminUserCommand command, CancellationToken cancellationToken)
            => await mediator.Send(command, cancellationToken);
        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut]
        public async Task<Result<int>> UpdateAsync(UpdateAdminUserCommand command, CancellationToken cancellationToken)
            => await mediator.Send(command, cancellationToken);
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete("{id:int}")]
        public async Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken)
            => await mediator.Send(new DeleteAdminUserCommand { Id = id }, cancellationToken);
        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<Result<AdminUserDto>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var res = await mediator.Send(new AdminUserDetailQuery { Id = id }, cancellationToken);

            return mapper.ToResult<AdminUser, AdminUserDto>(res);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<IList<AdminUserDto>>> GetListAsync(int limit, string keyword, CancellationToken cancellationToken)
        {
            var res = await mediator.Send(new AdminUserListQuery { Limit = limit, Keyword = keyword }, cancellationToken);

            return mapper.ToResult<List<AdminUser>, IList<AdminUserDto>>(res);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(int currentPage, int pageSize, string keyword, CancellationToken cancellationToken)
        {
            var res = await mediator.Send(new AdminUserPagedListQuery { CurrentPage = currentPage, PageSize = pageSize, Keyword = keyword }, cancellationToken);

            return mapper.ToPageResult<AdminUser, AdminUserDto>(res);
        }
    }
}
