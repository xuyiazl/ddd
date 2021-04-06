using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Domain.Common
{
    public static class ResultExtensions
    {
        public static Target ToResult<TSource, Target>(this IMapper mapper, TSource result)
            => mapper.Map<Target>(result);

        public static PagedModel<Target> ToPageResult<TSource, Target>(this IMapper mapper, PagedList<TSource> result)
        {
            var data = mapper.Map<IList<Target>>(result);

            return new PagedModel<Target>(data, result.TotalCount, result.CurrentPage, result.PageSize);
        }
    }
}
