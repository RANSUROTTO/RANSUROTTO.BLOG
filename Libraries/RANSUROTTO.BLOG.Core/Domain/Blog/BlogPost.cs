using System;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Domain.Blog
{

    public class BlogPost
    {

        /// <summary>
        /// 获取或设置对应语言ID
        /// </summary>
        public long LanguageId { get; set; }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置正文内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 获取或设置概述
        /// </summary>
        public string BodyOverview { get; set; }

        /// <summary>
        /// 获取或设置标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 获取或设置是否允许评论标识
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        /// 获取或设置展示开始的UTC时间
        /// </summary>
        public DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// 获取或设置展示结束的UTC时间
        /// </summary>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// 获取或设置最后编辑结束的UTC时间
        /// </summary>
        public DateTime LastEditorDateUtc { get; set; }

        #region Navigation Properties

        public virtual Language Language { get; set; }

        #endregion

    }

}
