using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Business.Interfaces;

namespace Trendyol.Business
{
    public class Category
    {
        public string Title { get; private set; }
        public Category ParentCategory { get; set; }

        public Category(string title)
        {
            Title = title;
        }

        public Category(string title, Category parentCategory) : this(title)
        {
            ParentCategory = parentCategory;
        }
    }
}
