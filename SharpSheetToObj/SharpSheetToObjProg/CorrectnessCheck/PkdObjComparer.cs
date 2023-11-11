using SharpFileServiceProg.Service;
using SharpSheetToObjProg.HasProperty;

namespace SharpSheetToObjProg.CorrectnessCheck
{
    public class PkdObjComparer<T1, T2> : IComparer<PkdObj<T1, T2>>
    {
        private readonly IFileService fileService;
        private string[] propNames;
        private Type propNamesType;

        public PkdObjComparer(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public int Compare(PkdObj<T1, T2> x, PkdObj<T1, T2> y)
        {
            var value01 = x.GetKey();
            var value02 = y.GetKey();
            var number = GetNumber(value01, value02);
            return number;
        }

        private string[] GetPropNames01<T>()
        {
            if (propNamesType != typeof(T))
            {
                propNames = fileService.Reflection.GetPropNames<T2>().ToArray();
                propNamesType = typeof(T);
                return propNames;
            }

            return propNames;
        }

        private int? CompareDate(PkdObj<T1, T2>? x, PkdObj<T1, T2>? y)
        {
            var value01 = ((IHasDate)x.Target).Date.ToString();
            var value02 = ((IHasDate)y.Target).Date.ToString();

            var number = GetNumber(value01, value02);
            return number;
        }

        private int? CompareKey(PkdObj<T1, T2>? x, PkdObj<T1, T2>? y)
        {
            var value01 = x.GetKey();
            var value02 = y.GetKey();
            var number = GetNumber(value01, value02);
            return number;
        }

        //private int? CompareId(PkdObj<T1, T2>? x, PkdObj<T1, T2>? y)
        //{
        //    var value01 = ((IHasId)x.Target).Id.ToString();
        //    var value02 = ((IHasId)y.Target).Id.ToString();

        //    var number = GetNumber(value01, value02);
        //    return number;
        //}
        
        //private int? CompareName(PkdObj<T1, T2>? x, PkdObj<T1, T2>? y)
        //{
        //    var value01 = ((IHasName)x.Target).Name.ToString();
        //    var value02 = ((IHasName)y.Target).Name.ToString();

        //    var number = GetNumber(value01, value02);
        //    return number;
        //}

        private static int GetNumber(string value01, string value02)
        {
            if (value01 == value02)
            {
                return 0;
            }

            var temp = new List<string>() { value01, value02 }.OrderByDescending(x => x);

            if (temp.ElementAt(0) == value01)
            {
                return 1;
            }

            return -1;
        }
    }
}
