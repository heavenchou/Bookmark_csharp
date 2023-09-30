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

        // 把書籤 A 移到 B 的上方
        public void moveA2BTop(BookmarkItem a, BookmarkItem b)
        {
            a.Parent.Children.Remove(a);
            int indexOfB = b.Parent.Children.IndexOf(b);
            b.Parent.Children.Insert(indexOfB, a);
            a.Parent = b.Parent;
        }

        // 把書籤 A 移到 B 的下方
        public void moveA2BBottom(BookmarkItem a, BookmarkItem b)
        {
            a.Parent.Children.Remove(a);
            int indexOfB = b.Parent.Children.IndexOf(b);
            b.Parent.Children.Insert(indexOfB + 1, a);
            a.Parent = b.Parent;
        }

        // 把書籤 A 移到 B 的裡面
        public void moveAIntoB(BookmarkItem a, BookmarkItem b)
        {
            a.Parent.Children.Remove(a);
            b.Children.Add(a);
            a.Parent = b;
        }

        // 某個目錄樹節點重新排序，排序目錄的部份
        // 理由可能是目錄有移動了

        public void noteUpdateFolder(TreeNode node)
        {
            BookmarkItem bookmark = node.Tag as BookmarkItem;
            int i = 0;
            foreach (var item in bookmark.Children) {
                if (item.IsFolder) {
                    treeViewFolderNoteFindItemMove2NewPos(node, item, i);
                    i++;
                }
            }
        }

        // 目錄樹的節點找到 Item 位置後，將它移到第 n 個節點

        private void treeViewFolderNoteFindItemMove2NewPos(TreeNode node, BookmarkItem bookmark, int i)
        {
            if(node.Nodes.Count == 0) {
                return;
            }
            foreach (TreeNode n in node.Nodes) {
                if ((BookmarkItem) n.Tag == bookmark) {
                    n.Remove();
                    node.Nodes.Insert(i, n);
                    return;
                }
            }
        }


        // 更新 TreeView 顯示
        // bOnlyFolder : true 表示只呈現目錄
        public void updateTreeView(bool bOnlyFolder = false)
        {
            treeView.Nodes.Clear();

            if (bOnlyFolder) {
                // 只有目錄，所以要有第一層
                TreeNode node = createBookmarkNode(BookmarkRoot, bOnlyFolder);
                treeView.Nodes.Add(node);
            } else {
                // 沒有 Root 那一層
                foreach (var bookmark in BookmarkRoot.Children) {
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
