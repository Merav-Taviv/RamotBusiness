using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common ;
using AutoMapper;

namespace Repository
{
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryCommon>().ReverseMap();
            CreateMap<List<Category>, List<CategoryCommon>>().ReverseMap();
        }
    }
}
