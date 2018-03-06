using System.Collections.Generic;
using System.Linq;

namespace Vega.Models.PieShop {
    public class FeedbackRepository : IFeedbackRepository {

        private readonly AppDbContext _appDbContext;

        public FeedbackRepository (AppDbContext context) {
            _appDbContext = context;
        }

        public void AddFeedback (Feedback feedback) {

            _appDbContext.Feedbacks.Add (feedback);
            _appDbContext.SaveChanges ();
        }

    }
}