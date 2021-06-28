using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        RamotBusinessContext context;

        public CategoryRepository(RamotBusinessContext context)
        {
            this.context = context;
        }

        public bool AddCategory(Category category)
        {
            if (context.Category.Contains(category))
            {
                return false;
            }
            context.Category.Add(category);
            return context.SaveChanges() > 0;
        }
        public bool UpdateCategory(Category category)
        {
            Category c = context.Category.Where(a => a.CategoryId == category.CategoryId).First();
            c.CategoryName = category.CategoryName;
            c.ProviderId = category.ProviderId;
            context.Category.Update(c);
            return context.SaveChanges() > 0;
        }
        public bool DeleteCategory(int categoryId)
        {
            foreach (Category item in context.Category)
            {
                if (item.CategoryId == categoryId)
                {
                    context.Category.Remove(item);
                }
            }
            return context.SaveChanges() > 0;

        }

    }
}



