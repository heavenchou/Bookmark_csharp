using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark
{
    internal class BookmarkItem
    {
        public string Title { get; set; } // 書籤標題
        public string URL { get; set; } // 書籤網址
        public List<BookmarkItem> Children { get; set; } // 子書籤列表
        public BookmarkItem Parent { get; set; } // 父書籤

        public BookmarkItem(string title, string url)
        {
            Title = title;
            URL = url;
            Children = new List<BookmarkItem>();
            Parent = null;
        }
    }
}
