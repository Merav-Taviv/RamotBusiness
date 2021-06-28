using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.Models;

namespace Repository.Repository
{
     public  class FeedbackRepository : IFeedbackRepository
    {
        private RamotBusinessContext context;
        public FeedbackRepository(RamotBusinessContext context)
        {
            this.context = context;
        }
        public bool AddFeedback(Feedback feedback)
        {
            if (context.Feedback.Contains(feedback))
            {
                return false;
            }

            context.Feedback.Add(feedback);
            return context.SaveChanges() > 0;
        }
        public bool UpdateFeedback(Feedback feedback)
        {
            Feedback f = context.Feedback.Where(a => a.FeekbackId == feedback.FeekbackId).First();
            f.Star = feedback.Star;
            f.CustomerId = feedback.CustomerId;
            f.ProviderId = feedback.ProviderId;
            context.Feedback.Update(f);
          return  context.SaveChanges()>0;
        }
        public bool DeleteFeedback(int feedbackId)
        {

            foreach (Feedback item in context.Feedback)
            {
                if (item.FeekbackId == feedbackId)
                {
                    context.Feedback.Remove(item);
                }
            }
            return context.SaveChanges() > 0;

        }
    }
}
