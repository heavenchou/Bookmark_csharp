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

        // treeView 移動的節點和目標
        private TreeNode draggedNode;
        private TreeNode targetNode;

        // listView 移動的節點和目標
        private ListViewItem draggedItem;
        private ListViewItem targetItem;

        private TreeNode treeviewFoldrSelected = null; // 已經選取的目錄樹節點

        //private int dropLineY;              // 畫線用的
        private bool insertAbove;
        int insertType = 0;             // 1: 插入在上方，2:放入節點中，3:插入在下方

        public MainForm()
        {
            InitializeComponent();
            treeView.Font = new Font("Arial Unicode MS", 12);

            BookmarkRoot = new BookmarkItem("書籤");
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
            BookmarkItem sutra2 = new BookmarkItem("Kāmaṃ kāmayamānassāti", "Kāmaṃ kāmayamānassāti");
            BookmarkItem sutra3 = new BookmarkItem("ā pāvuraṇā [pāpuraṇā (sī. syā.)]", "ā pāvuraṇā [pāpuraṇā (sī. syā.)]");
            BookmarkItem sutra4 = new BookmarkItem("長部", "Nikaya3");
            BookmarkItem sutra5 = new BookmarkItem("小", "Nikaya3");
            BookmarkItem sutra11 = new BookmarkItem("雜阿含經", "No.99");
            BookmarkItem sutra12 = new BookmarkItem("尼柯耶", "Nikaya");
            BookmarkItem sutra13 = new BookmarkItem("中部", "Nikaya2");
            BookmarkItem sutra14 = new BookmarkItem("長部", "Nikaya3");
            BookmarkItem sutra15 = new BookmarkItem("小", "Nikaya3");
            BookmarkItem lun = new BookmarkItem("論");
            BookmarkItem lun1 = new BookmarkItem("七大論");
            BookmarkItem lun12 = new BookmarkItem("發智論", "No.100");
            BookmarkItem lun2 = new BookmarkItem("清淨道論", "No.999");
            BookmarkItem lun3 = new BookmarkItem("註釋書");
            BookmarkItem lun4 = new BookmarkItem("釋義");

            BookmarkRoot.addBookmark(sutra);
            sutra.addBookmark(sutra1);
            sutra.addBookmark(sutra2);
            sutra.addBookmark(sutra3);
            sutra.addBookmark(sutra4);
            sutra.addBookmark(sutra5);
            sutra.addBookmark(sutra11);
            sutra.addBookmark(sutra12);
            sutra.addBookmark(sutra13);
            sutra.addBookmark(sutra14);
            sutra.addBookmark(sutra15);
            BookmarkRoot.addBookmark(lun);
            lun.addBookmark(lun1);
            lun.addBookmark(lun2);
            lun.addBookmark(lun3);
            lun.addBookmark(lun4);
            lun1.addBookmark(lun12);

            BookmarkTree.updateTreeView();
            BookmarkTreeFolder.updateTreeView(true);
        }

        // 將某個目錄的內容複製到 ListView
        private void treeViewFolder_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeviewFoldrSelected = treeViewFolder.SelectedNode;
            //if (node == null) { return; }
            BookmarkItem bookmark = (BookmarkItem) treeviewFoldrSelected.Tag;
            lvBookmarkList.Items.Clear();
            foreach (BookmarkItem child in bookmark.Children) {
                ListViewItem item = new ListViewItem();
                item.Tag = child;
                item.Text = child.Title;
                if (child.IsFolder) {
                    item.Text = "🗁" + child.Title;  // 📁📂🗀🗁
                    //item.ForeColor = Color.Blue;
                }
                item.SubItems.Add(child.URL);
                lvBookmarkList.Items.Add(item);
            }
            if (lvBookmarkList.Items.Count > 0) {
                lvBookmarkList.Focus();
                lvBookmarkList.Items[0].Focused = true;
                lvBookmarkList.Items[0].Selected = true;
            } else {
                // 清掉編輯區
                edTitle.Text = "";
                edURL.Text = "";
            }
        }

        // 選擇列表中某一筆，將資料複製至編輯區
        private void lvBookmarkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = lvBookmarkList.FocusedItem;
            if (item == null) { return; }
            if(item.Text.Length > 2 && item.Text.Substring(0, 2) == "🗁") {
                edTitle.Text = item.Text.Substring(2);
            } else {
                edTitle.Text = item.Text;
            }
            edURL.Text = item.SubItems[1].Text;
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

        private void lvBookmarkList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            targetItem = null;                  // 重置目標節點
            insertType = 0;                     // 插入模式歸 0
            lvBookmarkList.Invalidate();              // 畫面重新整理
            draggedItem = e.Item as ListViewItem;   // 要移動的節點
            DoDragDrop(draggedItem, DragDropEffects.Move);  // 開始拖曳
        }

        // 開始拖曳節點
        private void treeViewFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            targetNode = null;                  // 重置目標節點
            insertType = 0;                     // 插入模式歸 0
            treeViewFolder.Invalidate();        // 畫面重新整理
            draggedNode = e.Item as TreeNode;   // 要移動的節點
            // 根節點不能拖曳
            if (draggedNode != treeViewFolder.Nodes[0]) { 
                DoDragDrop(draggedNode, DragDropEffects.Move);  // 開始拖曳
            }
        }

        // =========================================

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
                    //targetNode.BackColor = Color.White;
                    //targetNode.ForeColor = Color.Black;
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
                //e.Effect = CanMoveToTargetNode(draggedNode, newTargetItem) ? DragDropEffects.Move : DragDropEffects.None;
                                
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

                        //targetNode.BackColor = Color.White;
                        //targetNode.ForeColor = Color.Black;
                    }
                } else if (insertType == 3) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        treeView.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left - 20, targetNode.Bounds.Bottom, targetNode.Bounds.Left + 200, targetNode.Bounds.Bottom);

                        //targetNode.BackColor = Color.White;
                        //targetNode.ForeColor = Color.Black;
                    }
                } else {
                    using (Pen pen = new Pen(Color.LightGreen, 4)) {
                        //targetNode.BackColor = Color.LightPink;
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

        private void lvBookmarkList_DragOver(object sender, DragEventArgs e)
        {
            if (draggedItem == null && draggedNode == null) {
                return;
            }
            Point point = lvBookmarkList.PointToClient(new Point(e.X, e.Y));
            ListViewItem newTargetItem = lvBookmarkList.GetItemAt(point.X, point.Y);     // 目前目標

            // 切換目標後，畫線才會重畫，否則會一直閃爍
            if (newTargetItem != targetItem) {
                lvBookmarkList.Invalidate();
                if (targetItem != null) {
                    //targetItem.BackColor = Color.White;
                    //targetItem.ForeColor = Color.Black;
                }
                targetItem = newTargetItem;
            }

            // 如果滑鼠靠近 TreeView 的上方或下方邊界，則捲動捲軸
            int scrollMargin = 20; // 捲動邊界的大小，可以自行調整
            if (targetItem != null) {
                if (point.Y < scrollMargin + 10) {
                    // 捲動到上一個可見節點
                    if (targetItem.Index > 0) {
                        ListViewItem prevItem = lvBookmarkList.Items[targetItem.Index - 1];
                        prevItem.EnsureVisible();
                        lvBookmarkList.Invalidate();
                    }
                    //e.Effect = DragDropEffects.Scroll;
                } else if (point.Y > lvBookmarkList.Height - scrollMargin) {
                    // 捲動到下一個可見節點
                    if (targetItem.Index < lvBookmarkList.Items.Count - 1) {
                        ListViewItem nextItem = lvBookmarkList.Items[targetItem.Index + 1];
                        nextItem.EnsureVisible();
                        lvBookmarkList.Invalidate();
                    }
                    //e.Effect = DragDropEffects.Scroll;
                }
            }
            
            if ((draggedNode != null || draggedItem != null) && targetItem != null && newTargetItem != draggedItem) {
                // 判斷拖動節點是否可以移動到目標位置（同一層或下一層）
                //e.Effect = CanMoveToTargetNode(draggedNode, newTargetItem) ? DragDropEffects.Move : DragDropEffects.None;

                // 判斷拖放提示線位置
                int newInsertType = 0;
                BookmarkItem bookmark = (BookmarkItem)targetItem.Tag;
                if (bookmark.IsFolder) {
                    // 目錄節點
                    if (point.Y - targetItem.Bounds.Top < 8) {
                        // 插入在上方
                        newInsertType = 1;
                    } else if (targetItem.Bounds.Bottom - point.Y < 8) {
                        // 插入在下方
                        newInsertType = 3;
                    } else {
                        // 放入目錄中
                        newInsertType = 2;
                    }
                } else {
                    // 一般節點
                    if (point.Y - targetItem.Bounds.Top < targetItem.Bounds.Height / 2) {
                        // 插入在上方
                        newInsertType = 1;
                    } else {
                        // 插入在下方
                        newInsertType = 3;
                    }
                }

                // 拖放線有改變才要重整畫面
                if (newInsertType != insertType) {
                    lvBookmarkList.Invalidate();
                    insertType = newInsertType;
                }

                // 繪製拖放提示線
                if (insertType == 1) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        lvBookmarkList.CreateGraphics().DrawLine(pen, targetItem.Bounds.Left - 20, targetItem.Bounds.Top, targetItem.Bounds.Left + 200, targetItem.Bounds.Top);

                        //targetItem.BackColor = Color.White;
                        //targetItem.ForeColor = Color.Black;
                    }
                } else if (insertType == 3) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        lvBookmarkList.CreateGraphics().DrawLine(pen, targetItem.Bounds.Left - 20, targetItem.Bounds.Bottom, targetItem.Bounds.Left + 200, targetItem.Bounds.Bottom);

                        //targetItem.BackColor = Color.White;
                        //targetItem.ForeColor = Color.Black;
                    }
                } else {
                    using (Pen pen = new Pen(Color.LightGreen, 4)) {
                        //targetItem.BackColor = Color.LightPink;
                        //targetItem.ForeColor = Color.Red;
                        lvBookmarkList.CreateGraphics().DrawRectangle(pen, targetItem.Bounds);
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
            //TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            Point point = treeViewFolder.PointToClient(new Point(e.X, e.Y));
            TreeNode newTargetNode = treeViewFolder.GetNodeAt(point);     // 目前目標

            // 切換目標後，畫線才會重畫，否則會一直閃爍
            if (newTargetNode != targetNode) {
                treeViewFolder.Invalidate();
                if (targetNode != null) {
                    //targetNode.BackColor = Color.White;
                    //targetNode.ForeColor = Color.Black;
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
                        treeViewFolder.Invalidate();
                    }
                    //e.Effect = DragDropEffects.Scroll;
                } else if (point.Y > treeViewFolder.Height - scrollMargin) {
                    // 捲動到下一個可見節點
                    TreeNode nextNode = targetNode.NextVisibleNode;
                    if (nextNode != null) {
                        nextNode.EnsureVisible();
                        treeViewFolder.Invalidate();
                    }
                    //e.Effect = DragDropEffects.Scroll;
                }
            }

            if ((draggedNode != null || draggedItem != null) && targetNode != null && newTargetNode != draggedNode) {
                // 判斷拖動節點是否可以移動到目標位置（同一層或下一層）
                //e.Effect = CanMoveToTargetNode(draggedNode, newTargetItem) ? DragDropEffects.Move : DragDropEffects.None;

                // 如果目標節點是收合的，則在停頓一段時間後自動展開
                if (!targetNode.IsExpanded) {
                    var timer = new Timer();
                    timer.Interval = 1000; // 設定停頓時間，這裡設為1秒（1000毫秒）
                    timer.Tick += (s, args) => {
                        if (targetNode != null) {
                            targetNode.Expand();
                            //treeViewFolder.Invalidate();
                        }
                        timer.Stop();
                    };
                    timer.Start();
                }

                // 判斷拖放提示線位置
                int newInsertType = 0;
                BookmarkItem bookmark = (BookmarkItem)targetNode.Tag;
                // 其實底下一定是目錄節點
                if (bookmark.IsFolder) {
                    if (targetNode != treeViewFolder.Nodes[0] && point.Y - targetNode.Bounds.Top < 8) {
                        // 插入在上方，前提是目標節點不可以是根目錄
                        newInsertType = 1;
                    } else if (targetNode != treeViewFolder.Nodes[0] && targetNode.Bounds.Bottom - point.Y < 8) {
                        // 插入在下方，前提是目標節點不可以是根目錄
                        newInsertType = 3;
                    } else {
                        // 放入目錄中
                        newInsertType = 2;
                    }
                } else {
                    // 一般節點 (這裡應該不會發生）
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
                    treeViewFolder.Invalidate();
                    insertType = newInsertType;
                }

                // 繪製拖放提示線
                if (insertType == 1) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        treeViewFolder.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left - 20, targetNode.Bounds.Top, targetNode.Bounds.Left + 200, targetNode.Bounds.Top);

                        //targetNode.BackColor = Color.White;
                        //targetNode.ForeColor = Color.Black;
                    }
                } else if (insertType == 3) {
                    using (Pen pen = new Pen(Color.LightBlue, 3)) {
                        treeViewFolder.CreateGraphics().DrawLine(pen, targetNode.Bounds.Left - 20, targetNode.Bounds.Bottom, targetNode.Bounds.Left + 200, targetNode.Bounds.Bottom);

                        //targetNode.BackColor = Color.White;
                        //targetNode.ForeColor = Color.Black;
                    }
                } else {
                    using (Pen pen = new Pen(Color.LightGreen, 4)) {
                        //targetNode.BackColor = Color.LightPink;
                        //targetNode.ForeColor = Color.Red;
                        treeViewFolder.CreateGraphics().DrawRectangle(pen, targetNode.Bounds);
                    }
                }
                e.Effect = DragDropEffects.Move;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        // =========================================

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

                    //targetNode.BackColor = Color.White;
                    //targetNode.ForeColor = Color.Black;
                    // 更新書籤管理器中的書籤結構
                    // BookmarkTreeFolder.updateTreeView();
                }
            }

            // 重置目標節點和拖放提示線
            draggedNode = null;
            targetNode = null;
            treeView.Invalidate();
        }

        // 拖曳到達目標

        private void lvBookmarkList_DragDrop(object sender, DragEventArgs e)
        {
            // 來源是 listview 本身
            if (draggedItem != null) {
                if (targetItem != null) {
                    // 插入節點上方
                    if (insertType == 1) {
                        draggedItem.Remove();
                        int targetIndex = targetItem.Index;
                        lvBookmarkList.Items.Insert(targetIndex, draggedItem);
                        // 書籤的位置也要處理
                        BookmarkTreeFolder.moveA2BTop((BookmarkItem)draggedItem.Tag, (BookmarkItem)targetItem.Tag);
                        // listView 的目錄移動了，所以目錄樹也要移動
                        if (((BookmarkItem)draggedItem.Tag).IsFolder) {
                            BookmarkTreeFolder.noteUpdateFolder(treeviewFoldrSelected);
                        }
                    }
                    // 放入目錄中
                    if (insertType == 2) {
                        draggedItem.Remove();
                        // 書籤的位置也要處理
                        BookmarkTreeFolder.moveAIntoB((BookmarkItem)draggedItem.Tag, (BookmarkItem)targetItem.Tag);

                        // 書籤的位置也要處理
                        /*
                        BookmarkItem parentBookmark = (BookmarkItem) treeviewFoldrSelected.Tag;
                        BookmarkItem bookmark = (BookmarkItem) draggedItem.Tag;
                        BookmarkItem targetBookmark = (BookmarkItem) targetItem.Tag;
                        parentBookmark.Children.Remove(bookmark);
                        targetBookmark.Children.Add(bookmark);
                        bookmark.Parent = targetBookmark;
                        */
                        // listView 的目錄移動了，所以目錄樹也要移動
                        if (((BookmarkItem)draggedItem.Tag).IsFolder) {
                            // 先找出原來目錄樹的來源節點和目的節點
                            TreeNode source = null;
                            TreeNode target = null;
                            foreach (TreeNode node in treeviewFoldrSelected.Nodes) {
                                if (node.Tag == draggedItem.Tag) {
                                    source = node;
                                }
                                if (node.Tag == targetItem.Tag) {
                                    target = node;
                                }
                            }
                            if (source != null && target != null) {
                                source.Remove();
                                target.Nodes.Add(source);
                            }
                        }
                    }
                    // 插入節點下方
                    if (insertType == 3) {
                        draggedItem.Remove();
                        int targetIndex = targetItem.Index;
                        lvBookmarkList.Items.Insert(targetIndex + 1, draggedItem);
                        // 書籤的位置也要處理
                        BookmarkTreeFolder.moveA2BBottom((BookmarkItem)draggedItem.Tag, (BookmarkItem)targetItem.Tag);
                        // listView 的目錄移動了，所以目錄樹也要移動
                        if (((BookmarkItem)draggedItem.Tag).IsFolder) {
                            BookmarkTreeFolder.noteUpdateFolder(treeviewFoldrSelected);
                        }
                    }

                    //targetItem.BackColor = Color.White;
                    //targetItem.ForeColor = Color.Black;
                    // 更新書籤管理器中的書籤結構
                    // BookmarkTreeFolder.updateTreeView();
                }
            }
            // 來源是目錄樹
            else if (draggedNode != null) {
                if (targetItem != null) {
                    // 確認目標節點不是要移動節點的子節點，避免出現無窮迴圈
                    if (!IsNodeAncestor(draggedNode, treeviewFoldrSelected)) {
                        // 建立新的 listviewItem
                        ListViewItem newItem = new ListViewItem();
                        newItem.Tag = draggedNode.Tag;
                        newItem.Text = "🗁" + draggedNode.Text;
                        newItem.SubItems.Add("");
                        // 插入節點上方
                        if (insertType == 1) {
                            draggedNode.Remove();
                            int targetIndex = targetItem.Index;
                            
                            lvBookmarkList.Items.Insert(targetIndex, newItem);
                            
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveA2BTop((BookmarkItem)draggedNode.Tag, (BookmarkItem)targetItem.Tag);
                            // listView 的目錄移動了，所以目錄樹也要移動
                            treeviewFoldrSelected.Nodes.Add(draggedNode);   // 先把來源丢入，底下再排序
                            BookmarkTreeFolder.noteUpdateFolder(treeviewFoldrSelected);
                        }
                        // 放入目錄中
                        if (insertType == 2) {
                            draggedNode.Remove();
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveAIntoB((BookmarkItem)draggedNode.Tag, (BookmarkItem)targetItem.Tag);

                            // 移動目錄樹
                            // 先找出原來目錄樹的目的節點
                            TreeNode target = null;
                            foreach (TreeNode node in treeviewFoldrSelected.Nodes) {
                                if (node.Tag == targetItem.Tag) {
                                    target = node;
                                    target.Nodes.Add(draggedNode);
                                    break;
                                }
                            }                            
                        }
                        // 插入節點下方
                        if (insertType == 3) {
                            draggedNode.Remove();
                            int targetIndex = targetItem.Index;
                            lvBookmarkList.Items.Insert(targetIndex + 1, newItem);
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveA2BBottom((BookmarkItem)draggedNode.Tag, (BookmarkItem)targetItem.Tag);
                            // listView 的目錄移動了，所以目錄樹也要移動
                            treeviewFoldrSelected.Nodes.Add(draggedNode);   // 先把來源丢入，底下再排序
                            BookmarkTreeFolder.noteUpdateFolder(treeviewFoldrSelected);
                        }
                    }
                    //targetItem.BackColor = Color.White;
                    //targetItem.ForeColor = Color.Black;
                    // 更新書籤管理器中的書籤結構
                    // BookmarkTreeFolder.updateTreeView();
                }
            }
            // 重置目標節點和拖放提示線
            draggedItem = null;
            targetItem = null;
            lvBookmarkList.Invalidate();
        }

        // 拖曳到達目標

        private void treeViewFolder_DragDrop(object sender, DragEventArgs e)
        {
            //TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            //TreeNode targetNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

            bool bUpdateListView = false;

            // 來源是同一棵目錄樹
            if (draggedNode != null) {
                // 如果拖曳的來源或目標是在已選擇的節點中，則 listview 要重整
                if ((draggedNode.Parent == treeviewFoldrSelected) || (targetNode.Parent == treeviewFoldrSelected)) {
                    bUpdateListView = true;
                }

                if (targetNode != null) {
                    // 確認目標節點不是要移動節點的子節點，避免出現無窮迴圈
                    if (!IsNodeAncestor(draggedNode, targetNode)) {
                        TreeNodeCollection parentNodes;

                        if (targetNode.Parent == null) {
                            parentNodes = treeViewFolder.Nodes;
                        } else {
                            parentNodes = targetNode.Parent.Nodes;
                        }
                        // 插入節點上方
                        if (insertType == 1) {
                            draggedNode.Remove();
                            int i = parentNodes.IndexOf(targetNode);
                            parentNodes.Insert(i, draggedNode);
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveA2BTop((BookmarkItem)draggedNode.Tag, (BookmarkItem)targetNode.Tag);
                        }
                        // 放入目錄中
                        if (insertType == 2) {
                            draggedNode.Remove();
                            targetNode.Nodes.Add(draggedNode);
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveAIntoB((BookmarkItem)draggedNode.Tag, (BookmarkItem)targetNode.Tag);
                            if (targetNode == treeviewFoldrSelected) {
                                bUpdateListView = true;
                            }
                        }
                        // 插入節點下方
                        if (insertType == 3) {
                            draggedNode.Remove();
                            int i = parentNodes.IndexOf(targetNode);
                            parentNodes.Insert(i + 1, draggedNode);
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveA2BBottom((BookmarkItem)draggedNode.Tag, (BookmarkItem)targetNode.Tag);
                        }

                        //targetNode.BackColor = Color.White;
                        //targetNode.ForeColor = Color.Black;

                        // 如果拖曳的來源或目標是在已選擇的節點中，則 listview 要重整

                        if (bUpdateListView) {
                            treeViewFolder_AfterSelect(this, null);
                        }
                    }
                }
            } 
            // 來源是 ListView
            else if (draggedItem != null) {

                // 如果拖曳的來源或目標是在已選擇的節點中，則 listview 要重整

                // 來源節點的書籤
                BookmarkItem draggedBook = draggedItem.Tag as BookmarkItem;
                // 目的節點的書籤
                BookmarkItem targetBook = targetNode.Tag as BookmarkItem;
                // 來源節點在目錄樹中的節點位置
                TreeNode draggedItem2Node = null;
                if(draggedBook.IsFolder) {
                    foreach(TreeNode node in treeviewFoldrSelected.Nodes) {
                        if (node.Tag == draggedBook) {
                            // 找到來源節點
                            draggedItem2Node = node; 
                            break;
                        }
                    }
                    if(draggedItem2Node == null) {
                        // 有問題，沒找到來源
                        return;
                    }
                }

                if (targetNode != null) {
                    // 確認目標節點不是要移動節點的子節點，避免出現無窮迴圈
                    if (!IsBookAncestor(draggedBook, targetBook)) {
                        TreeNodeCollection parentNodes;

                        if (targetNode.Parent == null) {
                            parentNodes = treeViewFolder.Nodes;
                        } else {
                            parentNodes = targetNode.Parent.Nodes;
                        }
                        // 插入節點上方
                        if (insertType == 1) {
                            draggedItem.Remove();
                            // 如果節點是目錄
                            if (draggedItem2Node != null) {
                                draggedItem2Node.Remove();
                                int i = parentNodes.IndexOf(targetNode);
                                parentNodes.Insert(i, draggedItem2Node);
                            }
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveA2BTop(draggedBook, targetBook);
                            // 目標在所選的節點，則要更新 listview
                            if(targetNode.Parent == treeviewFoldrSelected) {
                                bUpdateListView = true;
                            }
                        }
                        // 放入目錄中
                        if (insertType == 2) {
                            draggedItem.Remove();
                            // 如果節點是目錄
                            if (draggedItem2Node != null) {
                                draggedItem2Node.Remove();
                                targetNode.Nodes.Add(draggedItem2Node);
                            }
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveAIntoB(draggedBook, targetBook);
                            // 目標在所選的節點，則要更新 listview
                            if (targetNode == treeviewFoldrSelected) {
                                bUpdateListView = true;
                            }
                        }
                        // 插入節點下方
                        if (insertType == 3) {
                            draggedItem.Remove();
                            // 如果節點是目錄
                            if (draggedItem2Node != null) {
                                draggedItem2Node.Remove();
                                int i = parentNodes.IndexOf(targetNode);
                                parentNodes.Insert(i + 1, draggedItem2Node);
                            }
                            // 書籤的位置也要處理
                            BookmarkTreeFolder.moveA2BBottom(draggedBook, targetBook);
                            // 目標在所選的節點，則要更新 listview
                            if (targetNode.Parent == treeviewFoldrSelected) {
                                bUpdateListView = true;
                            }
                        }

                        //targetNode.BackColor = Color.White;
                        //targetNode.ForeColor = Color.Black;

                        // 如果拖曳的來源或目標是在已選擇的節點中，則 listview 要重整

                        if (bUpdateListView) {
                            treeViewFolder_AfterSelect(this, null);
                        }
                    }
                }
            }

            // 重置目標節點和拖放提示線
            draggedItem = null;
            draggedNode = null;
            targetNode = null;
            treeViewFolder.Invalidate();
        }

        // =========================================

        private void treeViewFolder_DragLeave(object sender, EventArgs e)
        {
            treeViewFolder.Invalidate();
        }

        private void lvBookmarkList_DragLeave(object sender, EventArgs e)
        {
            lvBookmarkList.Invalidate();
        }

        // =========================================

        // 輔助函式：檢查節點是否是目標節點的祖先
        private bool IsNodeAncestor(TreeNode node, TreeNode targetNode)
        {
            if(node.Nodes.Count == 0) { 
                // 既然我沒有子節點，我就不會是目標的祖先
                return false; 
            }
            TreeNode parentNode = targetNode;
            while (parentNode != null) {
                if (parentNode == node) {
                    return true;
                }
                parentNode = parentNode.Parent;
            }
            return false;
        }

        // 輔助函式：檢查節點是否是目標節點的祖先
        private bool IsBookAncestor(BookmarkItem book, BookmarkItem targetBook)
        {
            if (book.IsFolder == false) {
                // 既然我沒有子節點，我就不會是目標的祖先
                return false;
            }
            BookmarkItem parentBook = targetBook;
            while (parentBook != null) {
                if (parentBook == book) {
                    return true;
                }
                parentBook = parentBook.Parent;
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
