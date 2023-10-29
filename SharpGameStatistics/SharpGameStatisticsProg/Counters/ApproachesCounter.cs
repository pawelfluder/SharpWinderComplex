using CommonTypesCoreProj.Objects;
using System.Collections.Generic;
using System.Linq;

namespace GameStatisticsCoreProj.Counters
{
   public class ApproachesCounter
   {
      private int namedRejectionsCount;
      private int rejectionsCount;
      private int allRejectionsCount;
      private int contactsCount;
      private int approachesCount;
      private int namedEntriesCount;
      private int emptyTinderCount;

      public IEnumerable<(int, string)> GetData(List<Approaches> namedApproachesList, List<Rejections> rejectionsList)
      {
         namedEntriesCount = namedApproachesList.Count();
         namedRejectionsCount = namedApproachesList.CountRejections();
         emptyTinderCount = namedApproachesList.CountEmptyTinder();
         //restCount = namedApproachesList.CountRest();

         rejectionsCount = rejectionsList.CountRejections();
         allRejectionsCount = namedRejectionsCount + rejectionsCount;

         contactsCount = namedApproachesList.CountContacts();         
         approachesCount = contactsCount + allRejectionsCount;

         CheckCalculations();

         var result = new List<(int, string)>
         {
            (namedEntriesCount, HeaderNames.NamedEntries),
            (namedRejectionsCount, HeaderNames.NamedRejections),
            (contactsCount, HeaderNames.Contacts),
            (emptyTinderCount, HeaderNames.EmptyTinder),

            (allRejectionsCount, HeaderNames.AllRejections),
            (approachesCount, HeaderNames.AllApproaches),
         };       

         return result;
      }

      private void CheckCalculations()
      {
         var ch1 = namedEntriesCount == (contactsCount + namedRejectionsCount + emptyTinderCount);
         var ch2 = approachesCount == (contactsCount + namedRejectionsCount + rejectionsCount);

         if (!ch1 || !ch2)
         {
            //throw new System.Exception();
         }
      }
   }
}
