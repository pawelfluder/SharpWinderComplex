using SharpFileServiceProg.Service;
using System.Reflection;

namespace SharpSheetToObjProg.CorrectnessCheck
{
    public class PkdObjEqualBySource<T1, T2>
        where T1 : class
        where T2 : class
    {
        private readonly IFileService fileService;
        private PropertyInfo[] propList01;
        private PropertyInfo[] propList02;
        private Type type01;
        private Type type02;

        public PkdObjEqualBySource(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public bool Equal(PkdObj<T1, T2> obj01, PkdObj<T1, T2> obj02)
        {
            GetProp01<T1>();
            GetProp02<T1>();

            var thisTuples = GetTuples(propList01, obj01.Target);
            var objTuples = GetTuples(propList02, obj02.Target);

            if (thisTuples.Length != objTuples.Length)
            {
                return false;
            }

            for (int i = 0; i < thisTuples.Length; i++)
            {
                var tuple01 = thisTuples[i];
                var tuple02 = objTuples[i];
                if (tuple01 != tuple02)
                {
                    return false;
                }
            }

            return true;
        }

        private void GetProp01<T>()
        {
            if (type01 != typeof(T))
            {
                propList01 = fileService.Reflection.GetPropList<T2>().ToArray();
                type01 = typeof(T);
            }
        }

        private void GetProp02<T>()
        {
            if (type02 != typeof(T))
            {
                propList02 = fileService.Reflection.GetPropList<T2>().ToArray();
                type02 = typeof(T);
            }
        }

        public (string, string)[] GetTuples(PropertyInfo[] properties, object obj)
        {
            var tuples = properties.Select(x => (x.Name, x.GetValue(obj).ToString())).ToArray();
            return tuples;
        }
    }
}
