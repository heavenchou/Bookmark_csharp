using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark
{
    internal class BookmarkItem
    {
        public string Title { get; set; }   // 書籤標題
        public string URL { get; set; }     // 書籤網址
        // public string Keyword { get; set; } // 關鍵詞
        public bool IsFolder { get; set; } // 是否是目錄
        // public List<string> Tag { get; set; }     // 標籤
        public List<BookmarkItem> Children { get; set; } // 子書籤列表
        public BookmarkItem Parent { get; set; } // 父書籤

        // 宣告一個網址
        public BookmarkItem(string title, string url)
        {
            Title = title;
            URL = url;
            // Keyword = "";
            IsFolder = false;
            // Tag = new List<string>();
            Children = new List<BookmarkItem>();
            Parent = null;
        }

        // 宣告一個目錄
        public BookmarkItem(string title)
        {
            Title = title;
            URL = "";
            // Keyword = "";
            IsFolder = true;
            // Tag = null;
            Children = new List<BookmarkItem>();
            Parent = null;
        }

        // 加入書籤到指定的父書籤
        public void addBookmark(BookmarkItem newBookmark)
        {
            if (Children == null) { return; }
            Children.Add(newBookmark);
            newBookmark.Parent = this; // 設定新書籤的父書籤
        }
    }
}
