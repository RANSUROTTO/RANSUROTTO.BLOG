﻿using Autofac;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Infrastructure.TypeFinder;

namespace RANSUROTTO.BLOG.Core.Infrastructure
{
    /// <summary>
    /// 依赖注册接口
    /// </summary>
    public interface IDependencyRegistrar
    {

        /// <summary>
        /// 注册服务的实现
        /// </summary>
        /// <param name="builder">容器构建</param>
        /// <param name="typeFinder">类型查找器</param>
        /// <param name="config">配置</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, WebConfig config);


        /// <summary>
        /// 依赖注册顺序
        /// </summary>
        int Order { get; }

    }
}
