using SharpSheetToObjProg.HasProperty;

namespace SharpSheetToObjProg
{
    public class PkdObj<T1, T2> : IGetKey
    {
        public PkdObj(
            T1 source,
            T2 target,
            Func<string> getKeyFunc)
        {
            Source = source;
            Target = target;
            this.getKeyFunc = getKeyFunc;
        }

        public T1 Source { get; set; }

        public T2 Target { get; set; }

        private readonly Func<string> getKeyFunc;

        public string GetKey()
        {
            return getKeyFunc();
        }
    }
}
