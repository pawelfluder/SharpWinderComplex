using CommonTypesCoreProj.Objects;
using System.Collections.Generic;
using System.Linq;

namespace GameStatisticsCoreProj.Counters
{
   public class DatesCounter
   {
      public IEnumerable<(int, string)> GetData(List<Dates> data)
      {
         var result = new List<(int, string)>();
         var gg = data.Select(x => x.ApproachId).Distinct();

         return result;
      }
   }
}
