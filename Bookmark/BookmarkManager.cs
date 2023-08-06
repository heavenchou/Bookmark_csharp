using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookmark
{
    internal class Bookmark
    {
        private TreeView treeView;
        public BookmarkItem root;

        public Bookmark(TreeView treeView)
        {
            this.treeView = treeView;
            root = new BookmarkItem("","");
        }
      
        // 加入書籤到指定的父書籤
        public void addBookmark(BookmarkItem parent, BookmarkItem newBookmark)
        {
            parent.Children.Add(newBookmark);
            newBookmark.Parent = parent; // 設定新書籤的父書籤
            updateTreeView();
        }

        // 刪除書籤
        public void removeBookmark(BookmarkItem bookmark)
        {
            bookmark.Parent.Children.Remove(bookmark);
            bookmark.Parent = null; // 移除書籤的父書籤關聯
            updateTreeView();
        }

        // 更新 TreeView 顯示
        private void updateTreeView()
        {
            treeView.Nodes.Clear();
            foreach (var bookmark in root.Children) {
                TreeNode node = createBookmarkNode(bookmark);
                treeView.Nodes.Add(node);
            }
        }

        // 將書籤轉換成 TreeNode
        private TreeNode createBookmarkNode(BookmarkItem bookmark)
        {
            TreeNode node = new TreeNode(bookmark.Title);
            node.Tag = bookmark;

            foreach (var child in bookmark.Children) {
                TreeNode childNode = createBookmarkNode(child);
                node.Nodes.Add(childNode);
            }

            return node;
        }
    }
}
