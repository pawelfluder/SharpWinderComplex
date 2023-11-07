using CommonTypesCoreProj.Contracts;
using CSharpGameSynchProg.Contracts;
using SharpSheetToObjProg;
using SharpSheetToObjProg.HasProperty;
using System.Collections.Generic;
using System.Linq;

namespace CSharpGameSynchProg.Extensions
{
    public static class CommonObjectsExtensions
    {
        public static List<CommonObject> ToCommonObjectsList<T>(this List<T> inputList) where T : CommonObject
        {
            var convertedList = inputList.ConvertAll(x => (CommonObject)x);
            return null;
        }

        public static IOrderedEnumerable<PkdObj<T, HasIdDate>> OrderByDateProperty<T>(
            this List<PkdObj<T, HasIdDate>> inputList) where T : class
        {
            var result = inputList.OrderByDescending(num => num, new DateComparer<T>());
            return result;
        }

        public static IEnumerable<PkdObj<T, HasIdDate>> OrderByDate<T>(
            this IEnumerable<PkdObj<T, HasIdDate>> inputList) where T : class
        {
            var result = inputList.OrderByDescending(num => num, new DateComparer<T>());
            return result;
        }

        public static IEnumerable<PkdObj<T, HasIdDate>> OrderByDateId<T>(
            this IEnumerable<PkdObj<T, HasIdDate>> inputList) where T : class
        {
            var result = inputList.OrderByDescending(num => num, new DateIdComparer<T>());
            return result;
        }
    }

    public class DateIdComparer<T> : IComparer<PkdObj<T, HasIdDate>> where T : class
    {
        public int Compare(PkdObj<T, HasIdDate> d1, PkdObj<T, HasIdDate> d2)
        {
            var s1 = d1.Target.Date.ToString();
            var s2 = d2.Target.Date.ToString();

            var id1 = d1.Target.Id.ToString();
            var id2 = d2.Target.Id.ToString();

            if (s1 == s2)
            {
                var temp2 = new List<string>() { id1, id2 }.OrderByDescending(x => x);

                if (temp2.ElementAt(0) == id1)
                {
                    return 1;
                }

                return -1;
            }

            var temp = new List<string>() { s1, s2 }.OrderByDescending(x => x);

            if (temp.ElementAt(0) == s1)
            {
                return 1;
            }

            return -1;
        }
    }

    public class DateComparer<T> : IComparer<PkdObj<T, HasIdDate>> where T : class
    {
        public int Compare(PkdObj<T, HasIdDate> d1, PkdObj<T, HasIdDate> d2)
        {
            var s1 = d1.Target.Date.ToString();
            var s2 = d2.Target.Date.ToString();

            if (s1 == s2)
            {
                return 0;
            }

            var temp = new List<string>() { s1, s2 }.OrderByDescending(x => x);

            if (temp.ElementAt(0) == s1)
            {
                return 1;
            }

            return -1;
        }

        private static string ToDateString(CommonObject obj)
        {
            var prop = obj.GetType().GetProperties();
            var date = prop.Single(x => x.Name == "Date");
            var value = date.GetValue(obj).ToString();
            return value;
        }
    }
}
