using CommonTypesCoreProj.Objects;
using System.Collections.Generic;
using System.Linq;

namespace GameStatisticsCoreProj.Counters
{
   public class ContactsCounter
   {
      private int contactsCount;
      private int facebookContantsCount;
      private int numberContantsCount;
      private int instagramContantsCount;
      private int friendsContantsCount;
      private int restCount;

      public IEnumerable<(int, string)> GetData(List<Approaches> namedApproachesList)
      {
         contactsCount = namedApproachesList.CountContacts();
         facebookContantsCount = namedApproachesList.CountFacebookContact();
         numberContantsCount = namedApproachesList.CountNumberContact();
         instagramContantsCount = namedApproachesList.CountInstagramContact();
         restCount = namedApproachesList.Where(x => x.Type[1] != 'f' && x.Type[1] != 'n' && x.Type[1] != 'i' && x.Type[1] != 'x' && x.Type[1] != 'z').Count();

         CheckCalculations();

         var result = new List<(int, string)>
         {
            (facebookContantsCount, HeaderNames.FacebookContacts),
            (numberContantsCount, HeaderNames.PhoneNumberContacts),
            (instagramContantsCount, HeaderNames.InstagramContacts),
         };

         return result;
      }

      //friendsContantsCount = namedApproachesList.CountFriendContact();

      private void CheckCalculations()
      {
         var sum = (facebookContantsCount + numberContantsCount + instagramContantsCount + restCount);
         var ch1 = contactsCount == sum;

         if (!ch1)
         {
            //throw new System.Exception();
         }
      }
   }
}
