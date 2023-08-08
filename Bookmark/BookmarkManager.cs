using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookmark
{
    internal class BookmarkManager
    {
        private TreeView treeView;          // 書籤的樹
        public BookmarkItem BookmarkRoot;   // 書籤的根

        public BookmarkManager(TreeView treeView, BookmarkItem root)
        {
            this.treeView = treeView;
            BookmarkRoot = root;
        }
      
        // 加入書籤到指定的父書籤
        public void addBookmark(BookmarkItem parent, BookmarkItem newBookmark)
        {
            if (parent.Children == null) { return; }
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
        // bOnlyFolder : true 表示只呈現目錄
        public void updateTreeView(bool bOnlyFolder = false)
        {
            treeView.Nodes.Clear();
            foreach (var bookmark in BookmarkRoot.Children) {
                if (bOnlyFolder == false || bookmark.IsFolder == true) {
                    TreeNode node = createBookmarkNode(bookmark, bOnlyFolder);
                    treeView.Nodes.Add(node);
                }
            }
        }

        // 將書籤轉換成 TreeNode
        private TreeNode createBookmarkNode(BookmarkItem bookmark, bool bOnlyFolder)
        {
            TreeNode node = new TreeNode(bookmark.Title);
            node.Tag = bookmark;
            if(bookmark.IsFolder == true) {
                //node.BackColor = System.Drawing.Color.Yellow;
                node.ImageIndex = 0;
            } else {
                node.ImageIndex = 2;
            }
            node.SelectedImageIndex = node.ImageIndex;

            foreach (var child in bookmark.Children) {
                if (bOnlyFolder == false || child.IsFolder == true) {
                    TreeNode childNode = createBookmarkNode(child, bOnlyFolder);
                    node.Nodes.Add(childNode);
                }
            }

            return node;
        }
    }
}
