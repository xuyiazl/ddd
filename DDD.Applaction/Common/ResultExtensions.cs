using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.Common
{
    public static class ResultExtensions
    {
        public static Result<Target> ToResult<TSource, Target>(this IMapper mapper, Result<TSource> result)
        {
            var data = mapper.Map<Target>(result.data);

            return new Result<Target>
            {
                code = result.code,
                data = data,
                elapsedTime = result.elapsedTime,
                message = result.message,
                operationTime = result.operationTime,
                subCode = result.subCode
            };
        }

        public static Result<PagedModel<Target>> ToPageResult<TSource, Target>(this IMapper mapper, Result<PagedList<TSource>> result)
        {
            var data = mapper.Map<IList<Target>>(result.data);

            var model = new PagedModel<Target>(data, result.data.TotalCount, result.data.CurrentPage, result.data.PageSize);

            return new Result<PagedModel<Target>>
            {
                code = result.code,
                data = model,
                elapsedTime = result.elapsedTime,
                message = result.message,
                operationTime = result.operationTime,
                subCode = result.subCode
            };
        }
    }
}
