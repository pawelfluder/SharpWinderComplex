using CommonTypesCoreProj.Objects;
using System.Collections.Generic;
using System.Linq;

namespace GameStatisticsCoreProj.Counters
{
   public class ConversionCounter
   {
      public IEnumerable<(int, string)> GetData(IEnumerable<(int, string)> input)
      {
         var result = new List<(int, string)>();
         var countOfAllContacts = input.FirstOrDefault(x => x.Item2 == HeaderNames.AllContacts).Item1;
         var countOfAllApproaches = input.FirstOrDefault(x => x.Item2 == HeaderNames.AllApproaches).Item1;

        var tmp = (100 * 100 * countOfAllContacts);
            if (countOfAllApproaches == 0)
            {
                result.Add((0, HeaderNames.ConverstionApproachesToContact));
                return result;
            }
         var countOfConverstionApproachesToContact = (int)(tmp / countOfAllApproaches);

         // Todo - add next conversion FirstMeetings/Contacts
         // Konwersja podejscie -> kontakt
         // Konwersja kontakt -> randki
         // Konwersja randki -> spotkania
         // Todo
         // Randki
         // Spotkanie
         // Sex

         result.Add((countOfConverstionApproachesToContact, HeaderNames.ConverstionApproachesToContact));

         return result;
      }

      private bool IsTinderWithContact(Approaches da)
      {
         if (da.Type.StartsWith("tn") ||
             da.Type.StartsWith("tf"))
         {
            return true;
         }

         return false;
      }
   }
}
