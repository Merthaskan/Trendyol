using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Bussines.Interfaces;

namespace Trendyol.Bussines
{
    public class Category
    {
        public string Title { get; private set; }
        public Category ParentCategory { get; set; }

        public Category(string title)
        {
            Title = title;
        }
    }
}
