﻿using DDD.Applaction.AdminUsers.Services;
using DDD.Domain.AdminUsers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Interfaces
{
    public interface ITokenAppService
    {
        Task<Result<string>> CreateAsync(CancellationToken cancellationToken);

        Task<Result<TokenDto>> VerifyAsync(CancellationToken cancellationToken);
    }
}