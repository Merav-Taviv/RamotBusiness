using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Repository.Mapper
{
    public class SubCategoryProfile:Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategoryProfile, SubCategoryCommon>().ReverseMap();
            CreateMap<List<SubCategoryProfile>, List<SubCategoryCommon>>().ReverseMap();
        }
    }
}
