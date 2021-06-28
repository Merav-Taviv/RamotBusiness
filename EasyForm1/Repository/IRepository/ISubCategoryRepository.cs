using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface ISubCategoryRepository
    {
        bool AddSubCategory(SubCategory subCategory);
        bool UpdateSubCategory(SubCategory subCategory);
        bool DeleteSubCategory(int subCategoryId);
    }
}
