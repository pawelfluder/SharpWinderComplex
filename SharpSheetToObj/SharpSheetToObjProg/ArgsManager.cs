using CSharpGameSynchProg.Info;
using System;

namespace GameSynchCoreProj
{
    public class ArgsManager
    {
        public void Resolve(string[] args)
        {
            if (args.Length == 1 &&
               args[0] == "All")
            {
                ResolveForYear("2023", string.Empty);
                ResolveForYear("2022", string.Empty);
                ResolveForYear("2021", string.Empty);
                return;
            }

            if (args.Length == 1)
            {
                PrintAssemblyPath();
                ResolveForYear(args[0], string.Empty);
                return;
            }

            if (args.Length == 2)
            {
                PrintAssemblyPath();
                ResolveForYear(args[0], args[1]);
                return;
            }

            HandleWrongArgumentsError();
        }

        private void PrintAssemblyPath()
        {
            //var backSlash = @"\";
            //var assemblyPath1 = Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //var assemblyPath2 = Process.GetCurrentProcess().MainModule.FileName;
            //var assemblyPath3 = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + backSlash + "credentials3.json"; ;

            //Console.WriteLine(assemblyPath1);
            //Console.WriteLine(assemblyPath2);
            //Console.WriteLine(assemblyPath3);
        }

        private void ResolveForYear(string year, string option)
        {
            var info = new SheetInfo2022();
            var synchService = new SychService(info);
            synchService.SyncSheet(option);
            return;
        }

        private void HandleWrongArgumentsError()
        {
            Console.WriteLine("Wrong arguments passed. Usage examples:");
            Console.WriteLine("GameSynch 2022");
            Console.WriteLine("GameSynch 2021");
            Console.WriteLine("GameSynch All");
        }
    }
}
