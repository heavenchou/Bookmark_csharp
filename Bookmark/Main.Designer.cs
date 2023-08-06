namespace Bookmark
{
    partial class MainForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem105 = new System.Windows.Forms.ListViewItem(new string[] {
            "111",
            "1-2",
            "1-3",
            "1-4"}, -1);
            System.Windows.Forms.ListViewItem listViewItem106 = new System.Windows.Forms.ListViewItem("222");
            System.Windows.Forms.ListViewItem listViewItem107 = new System.Windows.Forms.ListViewItem("333");
            System.Windows.Forms.ListViewItem listViewItem108 = new System.Windows.Forms.ListViewItem("444");
            System.Windows.Forms.ListViewItem listViewItem109 = new System.Windows.Forms.ListViewItem("555");
            System.Windows.Forms.ListViewItem listViewItem110 = new System.Windows.Forms.ListViewItem("666");
            System.Windows.Forms.ListViewItem listViewItem111 = new System.Windows.Forms.ListViewItem("777");
            System.Windows.Forms.ListViewItem listViewItem112 = new System.Windows.Forms.ListViewItem("888");
            this.treeView = new System.Windows.Forms.TreeView();
            this.addBookmark = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btDelBookmark = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(273, 482);
            this.treeView.TabIndex = 0;
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this.treeView.DragLeave += new System.EventHandler(this.treeView_DragLeave);
            // 
            // addBookmark
            // 
            this.addBookmark.Location = new System.Drawing.Point(656, 26);
            this.addBookmark.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addBookmark.Name = "addBookmark";
            this.addBookmark.Size = new System.Drawing.Size(143, 62);
            this.addBookmark.TabIndex = 1;
            this.addBookmark.Text = "新增初值";
            this.addBookmark.UseVisualStyleBackColor = true;
            this.addBookmark.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(273, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 482);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Items.AddRange(new object[] {
            "item1",
            "item2",
            "item3"});
            this.listBox1.Location = new System.Drawing.Point(365, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(271, 179);
            this.listBox1.TabIndex = 3;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            listViewItem105.Checked = true;
            listViewItem105.StateImageIndex = 1;
            listViewItem106.StateImageIndex = 0;
            listViewItem107.StateImageIndex = 0;
            listViewItem108.StateImageIndex = 0;
            listViewItem109.StateImageIndex = 0;
            listViewItem110.StateImageIndex = 0;
            listViewItem111.StateImageIndex = 0;
            listViewItem112.StateImageIndex = 0;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem105,
            listViewItem106,
            listViewItem107,
            listViewItem108,
            listViewItem109,
            listViewItem110,
            listViewItem111,
            listViewItem112});
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(365, 245);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(409, 211);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "H1";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "H2";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "H3";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "H4";
            // 
            // btDelBookmark
            // 
            this.btDelBookmark.Location = new System.Drawing.Point(656, 115);
            this.btDelBookmark.Name = "btDelBookmark";
            this.btDelBookmark.Size = new System.Drawing.Size(143, 52);
            this.btDelBookmark.TabIndex = 5;
            this.btDelBookmark.Text = "刪除";
            this.btDelBookmark.UseVisualStyleBackColor = true;
            this.btDelBookmark.Click += new System.EventHandler(this.btDelBookmark_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 482);
            this.Controls.Add(this.btDelBookmark);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.addBookmark);
            this.Controls.Add(this.treeView);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button addBookmark;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btDelBookmark;
    }
}

