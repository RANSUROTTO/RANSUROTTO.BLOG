﻿using System;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Xml;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Security;

namespace RANSUROTTO.BLOG.Framework.Menu
{
    public class XmlSiteMap
    {

        public SiteMapNode RootNode { get; set; }

        public XmlSiteMap()
        {
            RootNode = new SiteMapNode();
        }

        public virtual void LoadFrom(string physicalPath)
        {
            string filePath = CommonHelper.MapPath(physicalPath);
            string content = File.ReadAllText(filePath);

            if (!string.IsNullOrEmpty(content))
            {
                using (var sr = new StringReader(content))
                {
                    using (var xr = XmlReader.Create(sr,
                        new XmlReaderSettings
                        {
                            CloseInput = true,
                            IgnoreWhitespace = true,
                            IgnoreComments = true,
                            IgnoreProcessingInstructions = true
                        }))
                    {
                        var doc = new XmlDocument();
                        doc.Load(xr);

                        if ((doc.DocumentElement != null) && doc.HasChildNodes)
                        {
                            XmlNode xmlRootNode = doc.DocumentElement.FirstChild;
                            Iterate(RootNode, xmlRootNode);
                        }
                    }
                }
            }
        }

        private static void Iterate(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            PopulateNode(siteMapNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.LocalName.Equals("siteMapNode", StringComparison.InvariantCultureIgnoreCase))
                {
                    var siteMapChildNode = new SiteMapNode();
                    siteMapNode.ChildNodes.Add(siteMapChildNode);

                    Iterate(siteMapChildNode, xmlChildNode);
                }
            }
        }

        private static void PopulateNode(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            //system name
            siteMapNode.SystemName = GetStringValueFromAttribute(xmlNode, "systemName");

            //title
            var resource = GetStringValueFromAttribute(xmlNode, "resource");
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            siteMapNode.Title = localizationService.GetResource(resource);

            //routes, url
            string controllerName = GetStringValueFromAttribute(xmlNode, "controller");
            string actionName = GetStringValueFromAttribute(xmlNode, "action");
            string url = GetStringValueFromAttribute(xmlNode, "url");
            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                siteMapNode.ControllerName = controllerName;
                siteMapNode.ActionName = actionName;
                siteMapNode.RouteValues = new RouteValueDictionary { { "area", "Admin" } };
            }
            else if (!string.IsNullOrEmpty(url))
            {
                siteMapNode.Url = url;
            }

            //image URL
            siteMapNode.IconClass = GetStringValueFromAttribute(xmlNode, "iconClass");

            //permission name
            var permissionNames = GetStringValueFromAttribute(xmlNode, "permissionNames");
            if (!string.IsNullOrEmpty(permissionNames))
            {
                var permissionService = EngineContext.Current.Resolve<IPermissionService>();
                siteMapNode.Visible = permissionNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Any(permissionName => permissionService.Authorize(permissionName.Trim()));
            }
            else
            {
                siteMapNode.Visible = true;
            }

            // Open URL in new tab
            var openUrlInNewTabValue = GetStringValueFromAttribute(xmlNode, "openUrlInNewTab");
            bool booleanResult;
            if (!string.IsNullOrWhiteSpace(openUrlInNewTabValue) && bool.TryParse(openUrlInNewTabValue, out booleanResult))
            {
                siteMapNode.OpenUrlInNewTab = booleanResult;
            }
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            string value = null;

            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                XmlAttribute attribute = node.Attributes[attributeName];

                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }

            return value;
        }

    }
}
