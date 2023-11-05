﻿using CommonTypesCoreProj.Contracts;
using CSharpGameSynchProg.Contracts;
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

        public static List<T> OrderByDateProperty<T>(this List<T> inputList) where T : IHasDate
        {
            var result = inputList.OrderByDescending(num => num, new SpecialComparer()).ToList();
            return result;
        }

        public class SpecialComparer : IComparer<IHasDate>
        {
            public int Compare(IHasDate d1, IHasDate d2)
            {
                var s1 = d1.Date.ToString();
                var s2 = d2.Date.ToString();

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
}
