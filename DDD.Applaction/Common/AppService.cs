using AutoMapper;
using DDD.Applaction.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;

namespace DDD.Applaction.Common
{
    [DynamicWebApi]
    [ApiError]
    [ApiTrace(Ignore = true)]
    [ApiElapsedTime]
    public class AppService : IAppService
    {
        public readonly IMediator mediator;
        public readonly IMapper mapper;

        public AppService(IMediator mediator, IMapper mapper)
        {
            this.mediator ??= mediator;
            this.mapper ??= mapper;
        }
    }
}
