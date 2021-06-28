using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private RamotBusinessContext context;
        public SubCategoryRepository(RamotBusinessContext context)
        {
            this.context = context;
        }
        public bool AddSubCategory(SubCategory subCategory)
        {
            if (context.SubCategory.Contains(subCategory))
            {
                return false;
            }

            context.SubCategory.Add(subCategory);
            return context.SaveChanges() > 0;
        }
        public bool UpdateSubCategory(SubCategory subCategory)
        {
            SubCategory s = context.SubCategory.Where(a => a.SubCategoryId == subCategory.SubCategoryId).First();
            s.SubCategoryName = subCategory.SubCategoryName;
            s.CategoryId = subCategory.CategoryId;
            context.SubCategory.Update(s);
           return context.SaveChanges()>0;
        }
        public bool DeleteSubCategory(int subCategoryId)
        {

            foreach (SubCategory item in context.SubCategory)
            {
                if (item.SubCategoryId == subCategoryId)
                {
                    context.SubCategory.Remove(item);
                }
            }
            return context.SaveChanges() > 0;

        }
    }
}

