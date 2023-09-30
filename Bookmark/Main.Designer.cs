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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.addBookmark = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lvBookmarkList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btDelBookmark = new System.Windows.Forms.Button();
            this.treeViewFolder = new System.Windows.Forms.TreeView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.edURL = new System.Windows.Forms.TextBox();
            this.lbURL = new System.Windows.Forms.Label();
            this.edTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.ImageIndex = 2;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(273, 481);
            this.treeView.TabIndex = 0;
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder_open.ico");
            this.imageList1.Images.SetKeyName(1, "folder_close.ico");
            this.imageList1.Images.SetKeyName(2, "16x16.png");
            // 
            // addBookmark
            // 
            this.addBookmark.Location = new System.Drawing.Point(7, 14);
            this.addBookmark.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addBookmark.Name = "addBookmark";
            this.addBookmark.Size = new System.Drawing.Size(105, 52);
            this.addBookmark.TabIndex = 1;
            this.addBookmark.Text = "新增初值";
            this.addBookmark.UseVisualStyleBackColor = true;
            this.addBookmark.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(273, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 481);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // lvBookmarkList
            // 
            this.lvBookmarkList.AllowDrop = true;
            this.lvBookmarkList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvBookmarkList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvBookmarkList.FullRowSelect = true;
            this.lvBookmarkList.GridLines = true;
            this.lvBookmarkList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvBookmarkList.HideSelection = false;
            this.lvBookmarkList.Location = new System.Drawing.Point(0, 0);
            this.lvBookmarkList.Name = "lvBookmarkList";
            this.lvBookmarkList.Size = new System.Drawing.Size(453, 339);
            this.lvBookmarkList.TabIndex = 4;
            this.lvBookmarkList.UseCompatibleStateImageBehavior = false;
            this.lvBookmarkList.View = System.Windows.Forms.View.Details;
            this.lvBookmarkList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvBookmarkList_ItemDrag);
            this.lvBookmarkList.SelectedIndexChanged += new System.EventHandler(this.lvBookmarkList_SelectedIndexChanged);
            this.lvBookmarkList.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvBookmarkList_DragDrop);
            this.lvBookmarkList.DragOver += new System.Windows.Forms.DragEventHandler(this.lvBookmarkList_DragOver);
            this.lvBookmarkList.DragLeave += new System.EventHandler(this.lvBookmarkList_DragLeave);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名稱";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "網址";
            this.columnHeader2.Width = 240;
            // 
            // btDelBookmark
            // 
            this.btDelBookmark.Location = new System.Drawing.Point(7, 74);
            this.btDelBookmark.Name = "btDelBookmark";
            this.btDelBookmark.Size = new System.Drawing.Size(105, 52);
            this.btDelBookmark.TabIndex = 5;
            this.btDelBookmark.Text = "刪除";
            this.btDelBookmark.UseVisualStyleBackColor = true;
            this.btDelBookmark.Click += new System.EventHandler(this.btDelBookmark_Click);
            // 
            // treeViewFolder
            // 
            this.treeViewFolder.AllowDrop = true;
            this.treeViewFolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewFolder.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.treeViewFolder.HideSelection = false;
            this.treeViewFolder.ImageIndex = 2;
            this.treeViewFolder.ImageList = this.imageList1;
            this.treeViewFolder.Location = new System.Drawing.Point(276, 0);
            this.treeViewFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewFolder.Name = "treeViewFolder";
            this.treeViewFolder.SelectedImageIndex = 0;
            this.treeViewFolder.Size = new System.Drawing.Size(273, 481);
            this.treeViewFolder.TabIndex = 6;
            this.treeViewFolder.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewFolder_ItemDrag);
            this.treeViewFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFolder_AfterSelect);
            this.treeViewFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewFolder_DragDrop);
            this.treeViewFolder.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewFolder_DragOver);
            this.treeViewFolder.DragLeave += new System.EventHandler(this.treeViewFolder_DragLeave);
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(549, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 481);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // splitter3
            // 
            this.splitter3.Location = new System.Drawing.Point(552, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(3, 481);
            this.splitter3.TabIndex = 8;
            this.splitter3.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.addBookmark);
            this.panel1.Controls.Add(this.btDelBookmark);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1008, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(122, 481);
            this.panel1.TabIndex = 9;
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(555, 339);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(453, 3);
            this.splitter4.TabIndex = 10;
            this.splitter4.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.edURL);
            this.panel2.Controls.Add(this.lbURL);
            this.panel2.Controls.Add(this.edTitle);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(555, 342);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(453, 139);
            this.panel2.TabIndex = 11;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(53, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 32);
            this.button4.TabIndex = 11;
            this.button4.Text = "新增書籤";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(167, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 32);
            this.button3.TabIndex = 10;
            this.button3.Text = "新增目錄";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(281, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 9;
            this.button2.Text = "刪除";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(362, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "儲存";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // edURL
            // 
            this.edURL.Location = new System.Drawing.Point(83, 93);
            this.edURL.Name = "edURL";
            this.edURL.Size = new System.Drawing.Size(354, 34);
            this.edURL.TabIndex = 3;
            // 
            // lbURL
            // 
            this.lbURL.AutoSize = true;
            this.lbURL.Location = new System.Drawing.Point(7, 94);
            this.lbURL.Name = "lbURL";
            this.lbURL.Size = new System.Drawing.Size(52, 25);
            this.lbURL.TabIndex = 2;
            this.lbURL.Text = "位址";
            // 
            // edTitle
            // 
            this.edTitle.Location = new System.Drawing.Point(83, 44);
            this.edTitle.Name = "edTitle";
            this.edTitle.Size = new System.Drawing.Size(354, 34);
            this.edTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "名稱";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lvBookmarkList);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(555, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(453, 339);
            this.panel3.TabIndex = 12;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 481);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.treeViewFolder);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.treeView);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button addBookmark;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView lvBookmarkList;
        private System.Windows.Forms.Button btDelBookmark;
        private System.Windows.Forms.TreeView treeViewFolder;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox edURL;
        private System.Windows.Forms.Label lbURL;
        private System.Windows.Forms.TextBox edTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel3;
    }
}

