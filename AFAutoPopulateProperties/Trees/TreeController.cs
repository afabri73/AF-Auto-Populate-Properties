// SYSTEM
using System;
using System.Globalization;
using System.Net.Http.Formatting;
using System.Web;
// UMBRACO
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace AFUmbracoLibrary
{
    /// <summary>
    /// Tree(APPConstants.Application.Alias, APPConstants.Tree.Alias, APPConstants.Tree.Title)
    /// PluginController(APPConstants.Controller.Alias)
    /// APPTreeController
    /// </summary>
    [Tree("developer", APPConstants.Tree.ParentAlias, APPConstants.Tree.Title, "icon-autofill color-red", "icon-autofill color-red", true, 100)]
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
            // check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var tree = new TreeNodeCollection {
                    CreateTreeNode(APPConstants.Tree.Alias, "-1", queryStrings, APPConstants.Tree.Action, APPConstants.Tree.Icon, false)
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