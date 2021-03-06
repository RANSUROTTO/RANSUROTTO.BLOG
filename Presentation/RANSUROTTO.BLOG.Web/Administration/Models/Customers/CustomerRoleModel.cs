﻿using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Customers;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Customers
{
    [Validator(typeof(CustomerRoleValidator))]
    public class CustomerRoleModel : BaseEntityModel
    {

        [AllowHtml]
        [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.SystemName")]
        public string SystemName { get; set; }

        [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Active")]
        public bool Active { get; set; }

        [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.IsSystemRole")]
        public bool IsSystemRole { get; set; }

    }
}