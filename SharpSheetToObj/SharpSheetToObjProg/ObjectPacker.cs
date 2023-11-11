using SharpSheetToObjProg.HasProperty;
using System.Reflection;

namespace SharpSheetToObjProg
{
    internal class ObjectPacker
    {
        public List<PkdObj<T1, T2>>
            Pack<T1, T2>(
                IEnumerable<T1> objList)
            where T1 : class
            where T2 : class, IGetKeyFunc
        {
            var result = new List<PkdObj<T1, T2>>();
            if (objList == null)
            {
                return result;
            }

            var type01 = typeof(T1);
            var prop01 = type01.GetProperties();
            var type02 = typeof(T2);
            var prop02 = type02.GetProperties();

            var common = SelectCommon(prop01, prop02);

            foreach ( var obj in objList)
            {
                var args = GetArgs(common, obj).ToArray();
                var target = (T2)Activator.CreateInstance(type02, args);
                result.Add(new PkdObj<T1, T2>(obj, target, target.GetKeyFunc()));
            }

            return result;
        }

        private IEnumerable<object> GetArgs<T1>(
            IEnumerable<PropertyInfo> common,
            T1? obj)
        {
            var gg = common.Select(x => x.GetValue(obj, null));
            return gg;
        }

        private static IEnumerable<PropertyInfo> SelectCommon(
            IEnumerable<PropertyInfo> prop01,
            IEnumerable<PropertyInfo> prop02)
        {
            var common = prop01.Where(x => prop02.Any(y => y.Name == x.Name));
            return common;
        }
    }
}
