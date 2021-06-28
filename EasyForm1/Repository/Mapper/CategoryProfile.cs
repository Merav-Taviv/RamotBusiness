using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common ;

namespace Repository
{
    public class CategoryProfile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryCommon>().ReverseMap();
            CreateMap<List<Category>, List<CategoryCommon>>().ReverseMap();
        }
    }
}
