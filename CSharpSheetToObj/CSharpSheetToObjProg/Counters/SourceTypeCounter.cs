using CommonTypesCoreProj.Objects;
using System.Collections.Generic;
using System.Linq;

namespace GameStatisticsCoreProj.Counters
{
   public class SourceTypeCounter
   {
      private int contactsCount;
      private int contactsFromTinderCount;
      private int contactsFromDayGameCount;
      private int contactsFromNightGameCount;
      private int contactsFromFriendsCount;

      public IEnumerable<(int, string)> GetData(List<Approaches> namedApproachesList)
      {
         contactsCount = namedApproachesList.CountContacts();
         contactsFromTinderCount = namedApproachesList.CountTinderWithContact();
         contactsFromDayGameCount = namedApproachesList.CountDayGameContacts();
         contactsFromNightGameCount = namedApproachesList.CountNightGameContacts();
         contactsFromFriendsCount = namedApproachesList.CountFriendsContacts();

         CheckCalculations();

         var result = new List<(int, string)>
         {
            (contactsCount, HeaderNames.AllContacts),
            (contactsFromTinderCount, HeaderNames.ContactsFromTinder),
            (contactsFromDayGameCount, HeaderNames.ContactsFromDayGame),
            (contactsFromNightGameCount, HeaderNames.ContactsFromNightGame),
            (contactsFromFriendsCount, HeaderNames.ContactsFromFriends),
         };

         return result;
      }

      private void CheckCalculations()
      {
         var sum = (contactsFromTinderCount + contactsFromDayGameCount + contactsFromNightGameCount + contactsFromFriendsCount);
         var ch1 = contactsCount == sum;

         if (!ch1)
         {
            //throw new System.Exception();
         }
      }
   }
}
