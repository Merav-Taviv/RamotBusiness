using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common;
using Repository;
using Repository.Models;

namespace Repository.Mapper
{
   public class FeedbackProfile : Profile
    {
        public FeedbackProfile() 
        {
            CreateMap<Feedback, FeedbackCommon>().ReverseMap();
            CreateMap<List<Feedback>, List<FeedbackCommon>>().ReverseMap();
        }

    }
}
