using SharpContainerProg.Public;

namespace CSharpSheetToObjProg.Repet
{
    internal static class MyBorder
    {
        public static Registration Registration = new Registration();
        public static IContainer Container => Registration.Start();
    }
}
