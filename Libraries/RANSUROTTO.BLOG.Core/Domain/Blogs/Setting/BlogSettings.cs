using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs.Setting
{

    /// <summary>
    /// 博客设置
    /// </summary>
    public class BlogSettings : ISettings
    {

        /// <summary>
        /// 获取或设置是否允许游客(匿名用户)进行评论
        /// </summary>
        public bool AllowGuestsToCreateComments { get; set; }

        /// <summary>
        /// 获取或设置博文可有最多标签个数
        /// </summary>  
        public int MaxNumberOfTags { get; set; }

        /// <summary>
        /// 获取或设置博客评论是否需要经过审批
        /// </summary>
        public bool BlogCommentsMustBeApproved { get; set; }

        /// <summary>
        /// 获取或设置富文本编辑器是否开启Javascript支持
        /// </summary>
        public bool RichEditorAllowJavaScript { get; set; }

        /// <summary>
        /// 获取或设置富文本编辑器的附加设置
        /// </summary>
        public string RichEditorAdditionalSettings { get; set; }

        public bool RelativeDateTimeFormattingEnabled { get; set; }

        public bool AllowCustomersToEditComments { get; set; }

        public bool AllowCustomersToDeleteComments { get; set; }

        public bool NotifyAboutNewNewsComments { get; set; }

    }

}
