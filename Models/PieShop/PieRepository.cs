using System.Collections.Generic;
using System.Linq;

namespace Vega.Models.PieShop {
    public class PieRepository : IPieRepository {

        private readonly AppDbContext _appDbContext;

        public PieRepository (AppDbContext context) {
            _appDbContext = context;
        }
        public IEnumerable<Pie> GetAllPies () {
            return _appDbContext.Pies;
        }

        public Pie GetPieById (int pieId) {
            return _appDbContext.Pies.FirstOrDefault (p => p.Id == pieId);
        }
    }
}