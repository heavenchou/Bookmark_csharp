using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bookmark
{
    public partial class MainForm : Form
    {
        Bookmark bookmark;
        private TreeNode draggedNode;
        private TreeNode targetNode;
        private int dropLineY;

        public MainForm()
        {
            InitializeComponent();
            bookmark = new Bookmark(treeView);
        }

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

            BookmarkItem sutra = new BookmarkItem("經", "");
            BookmarkItem sutra1 = new BookmarkItem("阿含", "");
            BookmarkItem sutra2 = new BookmarkItem("尼柯耶", "");
            BookmarkItem lun = new BookmarkItem("論", "");
            BookmarkItem lun1 = new BookmarkItem("七大論", "");
            BookmarkItem lun12 = new BookmarkItem("發智論", "");
            BookmarkItem lun2 = new BookmarkItem("清淨道論", "");

            bookmark.addBookmark(bookmark.root, sutra);
            bookmark.addBookmark(bookmark.root, lun);
            bookmark.addBookmark(sutra, sutra1);
            bookmark.addBookmark(sutra, sutra2);
            bookmark.addBookmark(lun, lun1);
            bookmark.addBookmark(lun, lun2);
            bookmark.addBookmark(lun1, lun12);
        }

        private void btDelBookmark_Click(object sender, EventArgs e)
        {
            TreeNode sele = treeView.SelectedNode;
            if (sele == null) { 
                return; 
            }
            bookmark.removeBookmark((BookmarkItem)sele.Tag);
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            draggedNode = (TreeNode)e.Item;
            targetNode = null; // 重置目標節點
            treeView.Invalidate();
            DoDragDrop(draggedNode, DragDropEffects.Move);
        }

        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            //TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            //TreeNode targetNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

            if (draggedNode != null && targetNode != null) {
                // 確認目標節點不是要移動節點的子節點，避免出現無窮迴圈
                if (!IsNodeAncestor(draggedNode, targetNode)) {
                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);

                    // 更新書籤管理器中的書籤結構
                    // bookmark.updateTreeView();
                }
            }

            // 重置目標節點和拖放提示線
            targetNode = null;
            treeView.Invalidate();
        }

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

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            //TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            TreeNode newTargetNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

            if (newTargetNode != targetNode) {
                treeView.Invalidate();
            }

            if (draggedNode != null && newTargetNode != null && newTargetNode != draggedNode) {
                // 判斷拖動節點是否可以移動到目標位置（同一層或下一層）
                //e.Effect = CanMoveToTargetNode(draggedNode, newTargetNode) ? DragDropEffects.Move : DragDropEffects.None;

                // 更新目標節點並顯示拖放提示線
                targetNode = newTargetNode;
                if (targetNode != null) {
                    dropLineY = targetNode.Bounds.Bottom;

                    // 如果目標節點是收合的，則在停頓一段時間後自動展開
                    if (!targetNode.IsExpanded) {
                        var timer = new Timer();
                        timer.Interval = 1000; // 設定停頓時間，這裡設為1秒（1000毫秒）
                        timer.Tick += (s, args) =>
                        {
                            if (targetNode != null) {
                                targetNode.Expand();
                            }
                            timer.Stop();
                        };
                        timer.Start();
                    }
                }
                //treeView.Invalidate();
                e.Effect = DragDropEffects.Move;
                // 繪製拖放提示線
                using (Pen pen = new Pen(Color.Black)) {
                    treeView.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left, dropLineY, targetNode.Bounds.Right, dropLineY);
                }
            } else {
                e.Effect = DragDropEffects.None;
                treeView.Invalidate();
            }
        }

        private void treeView_DragLeave(object sender, EventArgs e)
        {
            // 重置目標節點和拖放提示線
            targetNode = null;
            treeView.Invalidate();
        }
    }
}
