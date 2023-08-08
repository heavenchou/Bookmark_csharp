using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Bookmark.MainForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bookmark
{
    public partial class MainForm : Form
    {
        BookmarkManager BookmarkTreeFolder; // 只有目錄，編輯用的
        BookmarkManager BookmarkTree;       // 完整的
        BookmarkItem BookmarkRoot;          // 書籤的根

        private TreeNode draggedNode;
        private TreeNode targetNode;
        //private int dropLineY;              // 畫線用的
        private bool insertAbove;
        int insertType = 0;             // 1: 插入在上方，2:放入節點中，3:插入在下方

        public MainForm()
        {
            InitializeComponent();

            BookmarkRoot = new BookmarkItem("");
            BookmarkTreeFolder = new BookmarkManager(treeViewFolder, BookmarkRoot);
            BookmarkTree = new BookmarkManager(treeView, BookmarkRoot);
        }

        // 建立測試用節點
        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * 經
             *   阿含
             *   尼柯耶
             * 論
             *   七大論
             *     發智論
             *   清淨道論
             */

            BookmarkItem sutra = new BookmarkItem("經");
            BookmarkItem sutra1 = new BookmarkItem("雜阿含經", "No.99");
            BookmarkItem sutra2 = new BookmarkItem("尼柯耶", "Nikaya");
            BookmarkItem sutra3 = new BookmarkItem("中部", "Nikaya2");
            BookmarkItem sutra4 = new BookmarkItem("長部", "Nikaya3");
            BookmarkItem lun = new BookmarkItem("論");
            BookmarkItem lun1 = new BookmarkItem("七大論");
            BookmarkItem lun12 = new BookmarkItem("發智論", "No.100");
            BookmarkItem lun2 = new BookmarkItem("清淨道論", "No.999");

            BookmarkRoot.addBookmark(sutra);
            sutra.addBookmark(sutra1);
            sutra.addBookmark(sutra2);
            sutra.addBookmark(sutra3);
            sutra.addBookmark(sutra4);
            BookmarkRoot.addBookmark(lun);
            lun.addBookmark(lun1);
            lun.addBookmark(lun2);
            lun1.addBookmark(lun12);

            BookmarkTree.updateTreeView();
            BookmarkTreeFolder.updateTreeView();
        }

        // 刪除選擇節點
        private void btDelBookmark_Click(object sender, EventArgs e)
        {
            TreeNode sele = treeView.SelectedNode;
            if (sele == null) { 
                return; 
            }
            BookmarkTreeFolder.removeBookmark((BookmarkItem)sele.Tag);
        }

        // 選擇列表中某一筆，將資料複製至編輯區
        private void lvBookmarkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = lvBookmarkList.FocusedItem;
            if (item == null) { return; }
            edTitle.Text = item.Text;
            edURL.Text = item.SubItems[1].Text;
            //edTag.Text = item.SubItems[2].Text;
        }

        // 將某個目錄的內容複製到 ListView
        private void treeViewFolder_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeViewFolder.SelectedNode;
            //if (node == null) { return; }
            BookmarkItem bookmark = (BookmarkItem)node.Tag;
            lvBookmarkList.Items.Clear();
            foreach (BookmarkItem child in bookmark.Children) {
                ListViewItem item = new ListViewItem();
                item.Tag = child;
                item.Text = child.Title;
                item.SubItems.Add(child.URL);
                item.SubItems.Add("tag");
                lvBookmarkList.Items.Add(item);
            }
            if (lvBookmarkList.Items.Count > 0) {
                lvBookmarkList.Focus();
                lvBookmarkList.Items[0].Selected = true;
            }
        }

        /// <summary>
        /// 樹狀目錄移動節點
        /// </summary>

        private void lvBookmarkList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem draggedNode = (ListViewItem)e.Item;
            DoDragDrop(draggedNode, DragDropEffects.Move);
        }
        private void lvBookmarkList_DragDrop(object sender, DragEventArgs e)
        {
            Point dropPoint = lvBookmarkList.PointToClient(new Point(e.X, e.Y));
            ListViewItem dropItem = lvBookmarkList.GetItemAt(dropPoint.X, dropPoint.Y);
            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            if (dropItem != null && draggedItem != null) {
                int draggedIndex = draggedItem.Index;
                int dropIndex = dropItem.Index;

                lvBookmarkList.Items.RemoveAt(draggedIndex);
                lvBookmarkList.Items.Insert(dropIndex, draggedItem);
            }
        }

        private void lvBookmarkList_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void lvBookmarkList_DragOver(object sender, DragEventArgs e)
        {
            ListViewItem draggedNode = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            Point Pos = lvBookmarkList.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetNode = lvBookmarkList.GetItemAt(Pos.X, Pos.Y);

            if (draggedNode != null && targetNode != null && targetNode != draggedNode) {
                // 判斷拖動節點是否可以移動到目標位置（同一層或下一層）
                //e.Effect = CanMoveToTargetNode(draggedNode, newTargetNode) ? DragDropEffects.Move : DragDropEffects.None;

                lvBookmarkList.Invalidate();
                e.Effect = DragDropEffects.Move;
                // 繪製拖放提示線
                using (Pen pen = new Pen(Color.Black)) {
                    lvBookmarkList.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left, targetNode.Bounds.Bottom, targetNode.Bounds.Right, targetNode.Bounds.Bottom);
                }

                targetNode.BackColor = Color.LightGray;
            } else {
                e.Effect = DragDropEffects.Move;
                //treeView.Invalidate();
            }
        }

        /// =================================================
        /// 只有目錄的樹狀目錄移動節點
        /// =================================================

        // 開始拖曳節點
        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            targetNode = null;                  // 重置目標節點
            insertType = 0;                     // 插入模式歸 0
            treeView.Invalidate();              // 畫面重新整理
            draggedNode = e.Item as TreeNode;   // 要移動的節點
            DoDragDrop(draggedNode, DragDropEffects.Move);  // 開始拖曳
        }

        // 開始拖曳節點
        private void treeViewFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            targetNode = null;                  // 重置目標節點
            draggedNode = e.Item as TreeNode;
            DoDragDrop(draggedNode, DragDropEffects.Move);
        }

        // 拖曳經過其它節點上方
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            //TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            Point point = treeView.PointToClient(new Point(e.X, e.Y));
            TreeNode newTargetNode = treeView.GetNodeAt(point);     // 目前目標

            // 切換目標後，畫線才會重畫，否則會一直閃爍
            if (newTargetNode != targetNode) {
                treeView.Invalidate();
                if(targetNode != null) {
                    targetNode.BackColor = Color.White;
                    targetNode.ForeColor = Color.Black;
                }
                targetNode = newTargetNode;
            }

            // 如果滑鼠靠近 TreeView 的上方或下方邊界，則捲動捲軸
            int scrollMargin = 20; // 捲動邊界的大小，可以自行調整
            if (targetNode != null) {
                if (point.Y < scrollMargin) {
                    // 捲動到上一個可見節點
                    TreeNode prevNode = targetNode.PrevVisibleNode;
                    if (prevNode != null) {
                        prevNode.EnsureVisible();
                        treeView.Invalidate();
                    }
                    //e.Effect = DragDropEffects.Scroll;
                } else if (point.Y > treeView.Height - scrollMargin) {
                    // 捲動到下一個可見節點
                    TreeNode nextNode = targetNode.NextVisibleNode;
                    if (nextNode != null) {
                        nextNode.EnsureVisible();
                        treeView.Invalidate();
                    }
                    //e.Effect = DragDropEffects.Scroll;
                }
            }

            if (draggedNode != null && targetNode != null && newTargetNode != draggedNode) {
                // 判斷拖動節點是否可以移動到目標位置（同一層或下一層）
                //e.Effect = CanMoveToTargetNode(draggedNode, newTargetNode) ? DragDropEffects.Move : DragDropEffects.None;
                                
                // 如果目標節點是收合的，則在停頓一段時間後自動展開
                if (!targetNode.IsExpanded) {
                    var timer = new Timer();
                    timer.Interval = 1000; // 設定停頓時間，這裡設為1秒（1000毫秒）
                    timer.Tick += (s, args) => {
                        if (targetNode != null) {
                            targetNode.Expand();
                            //treeView.Invalidate();
                        }
                        timer.Stop();
                    };
                    timer.Start();
                }

                // 判斷拖放提示線位置
                int newInsertType = 0;
                BookmarkItem bookmark = (BookmarkItem)targetNode.Tag;
                if (bookmark.IsFolder) {
                    // 目錄節點
                    if (point.Y - targetNode.Bounds.Top < 8) {
                        // 插入在上方
                        newInsertType = 1;
                    } else if (targetNode.Bounds.Bottom - point.Y < 8) {
                        // 插入在下方
                        newInsertType = 3;
                    } else {
                        // 放入目錄中
                        newInsertType = 2;
                    }
                } else {
                    // 一般節點
                    if (point.Y - targetNode.Bounds.Top < targetNode.Bounds.Height / 2) {
                        // 插入在上方
                        newInsertType = 1;
                    } else {
                        // 插入在下方
                        newInsertType = 3;
                    }
                }

                // 拖放線有改變才要重整畫面
                if (newInsertType != insertType) { 
                    treeView.Invalidate();
                    insertType = newInsertType;
                }

                // 繪製拖放提示線
                if (insertType == 1) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        treeView.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left - 20, targetNode.Bounds.Top, targetNode.Bounds.Left + 200, targetNode.Bounds.Top);

                        targetNode.BackColor = Color.White;
                        targetNode.ForeColor = Color.Black;
                    }
                } else if (insertType == 3) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        treeView.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left - 20, targetNode.Bounds.Bottom, targetNode.Bounds.Left + 200, targetNode.Bounds.Bottom);

                        targetNode.BackColor = Color.White;
                        targetNode.ForeColor = Color.Black;
                    }
                } else {
                    using (Pen pen = new Pen(Color.LightGreen, 4)) {
                        targetNode.BackColor = Color.LightPink;
                        //targetNode.ForeColor = Color.Red;
                        treeView.CreateGraphics().DrawRectangle(pen, targetNode.Bounds);
                    }
                }
                e.Effect = DragDropEffects.Move;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        // 拖曳經過其它節點上方
        private void treeViewFolder_DragOver(object sender, DragEventArgs e)
        {
            Point point = treeViewFolder.PointToClient(new Point(e.X, e.Y));
            var newtargetNode = treeViewFolder.GetNodeAt(point);

            if(newtargetNode != targetNode) {
                targetNode = newtargetNode;
                //treeViewFolder.Refresh(); // Force the TreeView to redraw, triggering DrawNode event
                treeViewFolder.Invalidate();
            }

            if (targetNode != null) {
                // 如果滑鼠小於節點的一半，就是插入節點上方
                float relativeY = point.Y - targetNode.Bounds.Top;
                insertAbove = relativeY < (targetNode.Bounds.Height / 2);
            }

            e.Effect = DragDropEffects.Move;
        }

        // 拖曳到達目標

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            //TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            //TreeNode targetNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

            if (draggedNode != null && targetNode != null) {
                // 確認目標節點不是要移動節點的子節點，避免出現無窮迴圈
                if (!IsNodeAncestor(draggedNode, targetNode)) {
                    TreeNodeCollection parentNodes;
                    
                    if(targetNode.Parent == null) {
                        parentNodes = treeView.Nodes;
                    } else {
                        parentNodes = targetNode.Parent.Nodes;
                    }
                    // 插入節點上方
                    if(insertType == 1) {
                        draggedNode.Remove();
                        int i = parentNodes.IndexOf(targetNode);
                        parentNodes.Insert(i, draggedNode);
                    }
                    // 放入目錄中
                    if (insertType == 2) {
                        draggedNode.Remove();
                        targetNode.Nodes.Add(draggedNode);
                    }
                    // 插入節點下方
                    if (insertType == 3) {
                        draggedNode.Remove();
                        int i = parentNodes.IndexOf(targetNode);
                        parentNodes.Insert(i+1, draggedNode);
                    }

                    targetNode.BackColor = Color.White;
                    targetNode.ForeColor = Color.Black;
                    // 更新書籤管理器中的書籤結構
                    // BookmarkTreeFolder.updateTreeView();
                }
            }

            // 重置目標節點和拖放提示線
            targetNode = null;
            treeView.Invalidate();
        }

        // 拖曳到達目標

        private void treeViewFolder_DragDrop(object sender, DragEventArgs e)
        {
            if (draggedNode == null || targetNode == null) return;

            draggedNode.Remove();
            if (insertAbove) {
                targetNode.Parent.Nodes.Insert(targetNode.Index, draggedNode);
            } else {
                if (targetNode.IsExpanded) {
                    targetNode.Nodes.Insert(0, draggedNode);
                } else {
                    targetNode.Parent.Nodes.Insert(targetNode.Index + 1, draggedNode);
                }
            }
            treeViewFolder.Refresh(); // Refresh to remove the drawn line
        }

        // 重新繪製節點，主要是要畫拖曳目的的輔助線

        private void treeViewFolder_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true; // Draw the node as usual

            if (e.Node == targetNode) {
                int lineY = insertAbove ? e.Bounds.Top : e.Bounds.Bottom;
                using (Pen pen = new Pen(Color.Red, 2)) {
                    e.Graphics.DrawLine(pen, e.Bounds.Left, lineY, e.Bounds.Right, lineY);
                }
            }
        }

        ///////////////////////////////////////////////////////////////


        // 輔助函式：檢查節點是否是目標節點的祖先
        private bool IsNodeAncestor(TreeNode node, TreeNode targetNode)
        {
            TreeNode parentNode = targetNode;
            while (parentNode != null) {
                if (parentNode == node) {
                    return true;
                }
                parentNode = parentNode.Parent;
            }
            return false;
        }

        /*
        
        // 輔助函式：檢查節點是否可以移動到目標節點（同一層或下一層）
        private bool CanMoveToTargetNode(TreeNode draggedNode, TreeNode targetNode)
        {
            if (draggedNode == targetNode)
            {
                return false;
            }

            // 確認目標節點不是要移動節點的祖先
            if (IsNodeAncestor(draggedNode, targetNode))
            {
                return false;
            }

            // 確認目標節點不是要移動節點的子節點
            if (draggedNode.Nodes.Contains(targetNode))
            {
                return false;
            }

            return true;
        }

        // 判斷一個節點是否包含另一個節點
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // 檢查 node2 是否是 node1 的子節點
            if (node2.Parent == null) return false;
            if (node2.Parent == node1) return true;
            // 遞迴檢查 node2 的父節點
            return ContainsNode(node1, node2.Parent);
        }

        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView_DragLeave(object sender, EventArgs e)
        {
            // 重置目標節點和拖放提示線
            targetNode = null;
            treeView.Invalidate();
        }
        */
    }
}
