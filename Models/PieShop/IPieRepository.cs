using System.Collections.Generic;

namespace Vega.Models.PieShop {
    public interface IPieRepository {
        IEnumerable<Pie> GetAllPies ();
        Pie GetPieById (int pieId);
    }
}