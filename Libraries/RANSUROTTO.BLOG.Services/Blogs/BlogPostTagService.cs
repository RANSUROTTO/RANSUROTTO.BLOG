﻿using System;
using System.Linq;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Blogs
{
    public class BlogPostTagService : IBlogPostTagService
    {

        #region Constants

        /// <summary>
        /// 标签对应文章数
        /// </summary>
        /// <remarks>
        /// {0} : 标签ID
        /// </remarks>
        private const string BLOGPOSTTAG_COUNT_KEY = "Ransurotto.blogposttag.count";

        /// <summary>
        /// 清除标签缓存键匹配模式
        /// </summary>
        private const string BLOGPOSTTAG_PATTERN_KEY = "Ransurotto.blogposttag.";

        #endregion

        #region Fields

        private readonly IRepository<BlogPostTag> _blogPostTagRepository;
        private readonly IBlogService _blogService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly CommonSettings _commonSettings;

        #endregion

        #region Constructor

        public BlogPostTagService(IRepository<BlogPostTag> blogPostTagRepository, IBlogService blogService, IEventPublisher eventPublisher, ICacheManager cacheManager, IDataProvider dataProvider, CommonSettings commonSettings)
        {
            _blogPostTagRepository = blogPostTagRepository;
            _blogService = blogService;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
            _dataProvider = dataProvider;
            _commonSettings = commonSettings;
        }

        #endregion

        #region Methods

        public virtual IList<BlogPostTag> GetAllBlogPostTags()
        {
            var query = _blogPostTagRepository.Table;
            var blogPostTags = query.ToList();
            return blogPostTags;
        }

        public virtual BlogPostTag GetBlogPostTagById(int blogPostTagId)
        {
            if (blogPostTagId == 0)
                return null;

            return _blogPostTagRepository.GetById(blogPostTagId);
        }

        public virtual BlogPostTag GetBlogPostTagByName(string name)
        {
            var query = from pt in _blogPostTagRepository.Table
                        where pt.Name == name
                        select pt;

            var blogPostTag = query.FirstOrDefault();
            return blogPostTag;
        }

        public virtual void InsertBlogPostTag(BlogPostTag blogPostTag)
        {
            if (blogPostTag == null)
                throw new ArgumentNullException(nameof(blogPostTag));

            _blogPostTagRepository.Insert(blogPostTag);

            _cacheManager.RemoveByPattern(BLOGPOSTTAG_PATTERN_KEY);

            _eventPublisher.EntityInserted(blogPostTag);
        }

        public virtual void UpdateBlogPostTag(BlogPostTag blogPostTag)
        {
            if (blogPostTag == null)
                throw new ArgumentNullException(nameof(blogPostTag));

            _blogPostTagRepository.Update(blogPostTag);

            _cacheManager.RemoveByPattern(BLOGPOSTTAG_PATTERN_KEY);

            _eventPublisher.EntityUpdated(blogPostTag);
        }

        public virtual void DeleteBlogPostTag(BlogPostTag blogPostTag)
        {
            if (blogPostTag == null)
                throw new ArgumentNullException(nameof(blogPostTag));

            _blogPostTagRepository.Delete(blogPostTag);

            _cacheManager.RemoveByPattern(BLOGPOSTTAG_PATTERN_KEY);

            _eventPublisher.EntityDeleted(blogPostTag);
        }

        public virtual int GetBlogPostCount(int blogPostTagId)
        {
            var dictionary = GetBlogPostCount();
            if (dictionary.ContainsKey(blogPostTagId))
                return dictionary[blogPostTagId];

            return 0;
        }

        public virtual void UpdateBlogPostTags(BlogPost blogPost, string[] blogPostTags)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            var existingBlogPostTags = blogPost.BlogPostTags.ToList();
            var blogPostTagsToRemove = new List<BlogPostTag>();

            //筛选出需要删除的标签
            foreach (var existingBlogPostTag in existingBlogPostTags)
            {
                var found = false;
                foreach (var newProductTag in blogPostTags)
                {
                    if (existingBlogPostTag.Name.Equals(newProductTag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    blogPostTagsToRemove.Add(existingBlogPostTag);
                }
            }

            //删除标签
            foreach (var blogPostTag in blogPostTagsToRemove)
            {
                blogPost.BlogPostTags.Remove(blogPostTag);
                _blogService.UpdateBlogPost(blogPost);
            }

            //新增标签
            foreach (var tagName in blogPostTags)
            {
                BlogPostTag blogPostTag;
                var blogPostTag2 = GetBlogPostTagByName(tagName);
                if (blogPostTag2 == null)
                {
                    blogPostTag = new BlogPostTag
                    {
                        Name = tagName
                    };
                    InsertBlogPostTag(blogPostTag);
                }
                else
                {
                    blogPostTag = blogPostTag2;
                }
                if (!blogPost.BlogPostTagExists(blogPostTag.Id))
                {
                    blogPost.BlogPostTags.Add(blogPostTag);
                    _blogService.UpdateBlogPost(blogPost);
                }
            }
        }

        #endregion

        #region Utilities

        protected virtual Dictionary<int, int> GetBlogPostCount()
        {
            var key = string.Format(BLOGPOSTTAG_COUNT_KEY);
            return _cacheManager.Get(key, () =>
            {
                if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
                {
                    //TODO 没有实现存储过程获取博客文章数量的方法
                    throw new NotImplementedException();
                }
                else
                {
                    var query = _blogPostTagRepository.Table.Select(bpt => new
                    {
                        Id = bpt.Id,
                        BlogCount = bpt.BlogPosts.Count(p => !p.Deleted)
                    });
                    var dictionary = new Dictionary<int, int>();
                    foreach (var item in query)
                        dictionary.Add(item.Id, item.BlogCount);
                    return dictionary;
                }
            });
        }

        #endregion

    }
}
