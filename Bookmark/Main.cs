using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookmark
{
    public partial class MainForm : Form
    {
        Bookmark bookmark;

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
    }
}
