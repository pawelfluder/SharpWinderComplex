using CSharpGameSynchProg.Register;
using SharpFileServiceProg.Service;

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

        //public static IOrderedEnumerable<PkdObj<T, T2>>
        //    OrderByDateProperty<T, T2>(
        //        this List<PkdObj<T, T2>> inputList)
        //    where T : class
        //{
        //    var result = inputList.OrderByDescending(num => num, new DateComparer<T>());
        //    return result;
        //}

        //public static IEnumerable<PkdObj<T, T2>>
        //    OrderByDate<T, T2>(
        //        this IEnumerable<PkdObj<T, T2>> inputList)
        //    where T : class
        //{
        //    var result = inputList.OrderByDescending(num => num, new DateComparer<T>());
        //    return result;
        //}

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
}
