using AutoMapper;
using DDD.Applaction.Common.Interfaces;
using DDD.Infrastructure.Filters;
using MediatR;
using XUCore.NetCore.DynamicWebApi;

namespace DDD.Applaction.Common
{
    [DynamicWebApi]
    [ApiError]
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
