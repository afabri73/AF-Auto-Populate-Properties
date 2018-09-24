// SYSTEM
using System;
using System.Globalization;
using System.Net.Http.Formatting;
using System.Web;
// UMBRACO
using AF.AutoPopulateProperties.Constants;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace AF.AutoPopulateProperties.Trees
{
    /// <summary>
    /// Tree(APPConstants.Application.Alias, APPConstants.Tree.Alias, APPConstants.Tree.Title)
    /// PluginController(APPConstants.Controller.Alias)
    /// APPTreeController
    /// </summary>
    [Tree(APPConstants.Application.Alias, APPConstants.Tree.Alias, APPConstants.Tree.Title)]
    [PluginController(APPConstants.Controller.Alias)]
    public class APPTreeController : TreeController
    {
        /// <summary>
        /// GetTreeNodes(string id, FormDataCollection queryStrings)
        /// This method create the Base Tree of FALM custom section
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings"></param>
        /// <returns>tree</returns>
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var tree = new TreeNodeCollection();
            var textService = ApplicationContext.Services.TextService;

            // check if we're rendering the root node's children
            if (id == global::Umbraco.Core.Constants.System.Root.ToInvariantString())
            {
                tree = new TreeNodeCollection {
                    CreateTreeNode("config", "-1", queryStrings, textService.Localize("AFAPP/Config.TreeSection", CultureInfo.CurrentCulture), APPConstants.Tree.Icon, false)
                };

                return tree;
            }

            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }

        /// <summary>
        /// GetMenuForNode(string id, FormDataCollection queryStrings)
        /// This method create the actions on a single menu item (by pressing "..." symbol)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings"></param>
        /// <returns>menu</returns>
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return null;
        }
    }
}