using AF.AutoPopulateProperties.Constants;
using System;
using System.Globalization;
using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace AF.AutoPopulateProperties.Trees
{
    /// <summary>
    /// Tree(APPConstants.Application.Alias, APPConstants.Tree.Alias, TreeTitle = APPConstants.Tree.Title, TreeGroup = APPConstants.Tree.GroupTitle)
    /// PluginController(APPConstants.Controller.Alias)
    /// APPTreeController
    /// </summary>
    [Tree(APPConstants.Application.Alias, APPConstants.Tree.Alias, TreeTitle = APPConstants.Tree.Title, TreeGroup = APPConstants.Tree.GroupTitle)]
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
            // you can get your custom nodes from anywhere, and they can represent anything...
            //Dictionary<int, string> ExampleTreeNodes = new Dictionary<int, string>();
            //ExampleTreeNodes.Add(1, "Nodo 1");
            //ExampleTreeNodes.Add(2, "Nodo 2");
            //ExampleTreeNodes.Add(3, "Nodo 3");
            //ExampleTreeNodes.Add(4, "Nodo 4");
            //ExampleTreeNodes.Add(5, "Nodo 5");
            //ExampleTreeNodes.Add(6, "Nodo 6");

            var Tree = new TreeNodeCollection();
            //var textService = Services.TextService;

            // check if we're rendering the root node's children
            if (id == global::Umbraco.Core.Constants.System.Root.ToInvariantString())
            {
                Tree = new TreeNodeCollection {
                    
                };

                // add each node to the tree collection using the base CreateTreeNode method
                // it has several overloads, using here unique Id of tree item, -1 is the Id of the parent node to create, eg the root of this tree is -1 by convention - the querystring collection passed into this route - the name of the tree node -  css class of icon to display for the node - and whether the item has child nodes
                var node = CreateTreeNode("config", "-1", queryStrings, Services.TextService.Localize("AFAPP/Config.TreeSection", CultureInfo.CurrentCulture), APPConstants.Tree.Icon, false);
                Tree.Add(node);

                return Tree;
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
            // create a Menu Item Collection to return so people can interact with the nodes in your tree
            var menu = new MenuItemCollection();

            if (id == global::Umbraco.Core.Constants.System.Root.ToInvariantString())
            {
                // root actions, perhaps users can create new items in this tree, or perhaps it's not a content tree, it might be a read only tree, or each node item might represent something entirely different...
                // add your menu item actions or custom ActionMenuItems
                menu.Items.Add(new CreateChildEntity(Services.TextService));
                // add refresh menu item (note no dialog)
                menu.Items.Add(new RefreshNode(Services.TextService, true));
                return menu;
            }
            // add a delete action to each individual item
            //menu.Items.Add<ActionDelete>(Services.TextService, true, opensDialog: true);

            return menu;
        }

        /// <summary>
        /// Root Node
        /// </summary>
        /// <param name="queryStrings"></param>
        /// <returns></returns>
        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            var root = base.CreateRootNode(queryStrings);

            // set the icon
            root.Icon = APPConstants.Application.Icon;

            // could be set to false for a custom tree with a single node.
            root.HasChildren = true;
            
            //url for menu
            root.MenuUrl = null;

            return root;
        }
    }
}