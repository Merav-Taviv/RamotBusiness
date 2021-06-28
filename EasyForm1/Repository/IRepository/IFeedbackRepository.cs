using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.IRepository
{
   public interface IFeedbackRepository
    {
        bool AddFeedback(Feedback feedback);
        bool UpdateFeedback(Feedback feedback);
        bool DeleteFeedback(int FeedbackId);
    }
}
