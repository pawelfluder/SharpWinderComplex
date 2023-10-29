using GameStatisticsCoreProj;

namespace GoogleApiV4CoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var argsManager = new ArgsManager();
            argsManager.Resolve(args);
        }
    }
}