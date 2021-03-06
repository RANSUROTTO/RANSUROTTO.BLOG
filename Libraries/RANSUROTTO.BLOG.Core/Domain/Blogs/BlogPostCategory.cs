﻿using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    public class BlogPostCategory : BaseEntity
    {

        public int BlogPostId { get; set; }

        public int BlogCategoryId { get; set; }

        public int DisplayOrder { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual Category Category { get; set; }

    }
}
