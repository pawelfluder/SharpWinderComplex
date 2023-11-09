using CSharpGameSynchProg.Register;
using SharpFileServiceProg.Service;
using SharpSheetToObjProg.HasProperty;

namespace SharpSheetToObjProg.CorrectnessCheck
{
    public static class CommonObjectsExtensions
    {
        private static IFileService fileService = MyBorder.Container.Resolve<IFileService>();

        public static List<CommonObject> ToCommonObjectsList<T>(this List<T> inputList) where T : CommonObject
        {
            var convertedList = inputList.ConvertAll(x => (CommonObject)x);
            return null;
        }

        public static IOrderedEnumerable<PkdObj<T, T2>> OrderByDateProperty<T>(
            this List<PkdObj<T, T2>> inputList) where T : class
        {
            var result = inputList.OrderByDescending(num => num, new DateComparer<T>());
            return result;
        }

        public static IEnumerable<PkdObj<T, T2>> OrderByDate<T>(
            this IEnumerable<PkdObj<T, T2>> inputList) where T : class
        {
            var result = inputList.OrderByDescending(num => num, new DateComparer<T>());
            return result;
        }

        public static IEnumerable<PkdObj<T1, T2>>
            OrderByDateId<T1, T2>(
                this IEnumerable<PkdObj<T1, T2>> inputList)
            where T1 : class
            where T2 : class
        {
            var result = inputList
                .OrderByDescending(num => num, new PkdObjComparer<T1, T2>(fileService));
            return result;
        }
    }

    public class DateIdComparer<T1> : IComparer<PkdObj<T1, T2>> where T1 : class
    {
        private IFileService fileService;

        public DateIdComparer(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public int Compare(
            PkdObj<T1, T2> obj01,
            PkdObj<T1, T2> obj02)
        {
            var propNames = fileService.Reflection.GetPropNames<T2>();



            foreach (var pName in propNames)
            {
                
            }


            var s1 = obj01.Target.Date.ToString();
            var s2 = obj02.Target.Date.ToString();

            var id1 = obj01.Target.Id.ToString();
            var id2 = obj02.Target.Id.ToString();

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

    
    public class DateComparer<T> : IComparer<PkdObj<T, T2>> where T : class
    {
        public int Compare(PkdObj<T, T2> d1, PkdObj<T, T2> d2)
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
