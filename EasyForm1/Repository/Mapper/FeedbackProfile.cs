using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Repository;

namespace Repository.Mapper
{
   public class FeedbackProfile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackCommon>().ReverseMap();
            CreateMap<List<Feedback>, List<FeedbackCommon>>().ReverseMap();
        }

    }
}
