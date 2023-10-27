using SharpContainerProg.Public;

namespace SharpSetupProg24Private.Repetition
{
    internal static class MyBorder
    {
        private static IContainer container = new Registration().Start();
        public static IContainer Container => container;
    }
}
