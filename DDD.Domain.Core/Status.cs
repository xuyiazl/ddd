using System.ComponentModel;

namespace DDD.Domain.Core
{
    public enum Status
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部数据")]
        All = 0,
        /// <summary>
        /// 正常显示
        /// </summary>
        [Description("正常显示")]
        Show = 1,
        /// <summary>
        /// 数据下架（隐藏）
        /// </summary>
        [Description("数据下架")]
        SoldOut = 2,
        /// <summary>
        /// 数据已被删除，非物理删除（回收站）
        /// </summary>
        [Description("回收站")]
        Trash = 3
    }
}
