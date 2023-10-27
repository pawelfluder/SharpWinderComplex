using CommonTypesCoreProj.Interfaces;
using CommonTypesCoreProj.Objects;
using System.Collections.Generic;
using System.Linq;

namespace GameStatisticsCoreProj.Counters
{
   public static class ATypeHelper
   {
      public static int CountContacts(this IEnumerable<IHasTypeProp> aTypeObjList)
      {
         var num = 0;
         foreach (var obj in aTypeObjList)
         {
            if (obj.IsContact())
            {
               num++;
            }
         }

         return num;
      }

      public static int CountEmptyTinder(this IEnumerable<IHasTypeProp> aTypeObjList)
      {
         var num = 0;
         foreach (var obj in aTypeObjList)
         {
            if (obj.IsRest())
            {
               num++;
            }
         }

         return num;
      }

      public static bool IsRest(this IHasTypeProp aTypeObj)
      {
         if (aTypeObj.Type.StartsWith(ATypes.tx))
         {
            return true;
         }

         return false;
      }

      public static bool IsContact(this IHasTypeProp aTypeObj)
      {
         if (aTypeObj.Type.StartsWith(ATypes.pn) ||
             aTypeObj.Type.StartsWith(ATypes.pf) ||
             aTypeObj.Type.StartsWith(ATypes.pi) ||

             aTypeObj.Type.StartsWith(ATypes.nn) ||
             aTypeObj.Type.StartsWith(ATypes.nf) ||
             aTypeObj.Type.StartsWith(ATypes.ni) ||

             aTypeObj.Type.StartsWith(ATypes.zn) ||
             aTypeObj.Type.StartsWith(ATypes.zf) ||
             aTypeObj.Type.StartsWith(ATypes.zi) ||

             aTypeObj.Type.StartsWith(ATypes.tn) ||
             aTypeObj.Type.StartsWith(ATypes.tf) ||
             aTypeObj.Type.StartsWith(ATypes.ti))
         {
            return true;
         }

         return false;
      }

      public static int CountFacebookContact(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[1] == 'f').Count();
         return num;
      }

      public static int CountTinderWithContact(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[0] == 't' && x.Type[1] != 'x').Count();
         return num;
      }

      public static int CountDayGameContacts(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[0] == 'p' && x.Type[1] != 'z' && x.Type[1] != 'x').Count();
         return num;
      }

      public static int CountNightGameContacts(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[0] == 'n' && x.Type[1] != 'x').Count();
         return num;
      }

      public static int CountFriendsContacts(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[1] == 'z' && x.Type[1] != 'x').Count();
         return num;
      }

      public static int CountRest(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[0] == 't').Count();
         return num;
      }

      public static int CountInstagramContact(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[1] == 'i').Count();
         return num;
      }

      public static int CountNumberContact(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[1] == 'n').Count();
         return num;
      }

      public static int CountFriendContact(this IEnumerable<IHasTypeProp> cTypeObj)
      {
         var num = cTypeObj.Where(x => x.Type[0] == 'z').Count();
         return num;
      }

      public static int CountRejections(this IEnumerable<IHasCountProp> aTypeObjList)
      {
         var sum = 0;
         foreach (var dataRejection in aTypeObjList)
         {
            sum += int.Parse(dataRejection.Count);
         }

         return sum;
      }

      public static int CountRejections(this IEnumerable<IHasTypeProp> aTypeObjList)
      {
         var namedRejectionsList = aTypeObjList.Where(x => IsRejection(x));
         var rejectionsCount = namedRejectionsList.Count();
         return rejectionsCount;
      }

      public static bool IsRejection(this IHasTypeProp aTypeObj)
      {
         if (aTypeObj.Type.StartsWith(ATypes.pz) ||
             aTypeObj.Type.StartsWith(ATypes.nz))
         {
            return true;
         }

         return false;
      }
   }
}
