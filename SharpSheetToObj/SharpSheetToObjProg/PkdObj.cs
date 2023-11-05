namespace SharpSheetToObjProg
{
    public class PkdObj<T1, T2>
    {
        public PkdObj(T1 source, T2 target)
        {
            Source = source;
            Target = target;
        }

        public T1 Source { get; set; }

        public T2 Target { get; set; }
    }
}
