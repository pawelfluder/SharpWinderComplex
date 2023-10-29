using SharpContainerProg.AAPublic;

namespace CSharpSheetToObjProg.Register
{
    internal static class MyBorder
    {
        public static bool IsRegistered = false;
        public static Registration Registration = new Registration();
        public static IContainer Container =>
            Registration.Start(ref IsRegistered);
    }
}
