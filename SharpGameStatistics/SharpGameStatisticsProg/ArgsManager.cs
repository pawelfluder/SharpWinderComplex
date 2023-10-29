using CSharpSheetToObjProg.Register;
using SharpRepoServiceProg.Service;

namespace GameStatisticsCoreProj
{
    public class ArgsManager
    {
        private readonly IRepoService repoService;
        private readonly StatisticsWorker workerStatistics;

        public ArgsManager()
        {
            repoService = MyBorder.Container.Resolve<IRepoService>();
            workerStatistics = new StatisticsWorker();
        }

        public void Resolve(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "2022" };
            }

            var adrTuple01 = ("Persistency", "");

            if (args.Length == 1)
            {
                var report = ResolveForYear(adrTuple01, args[0]);
                Console.Write(report);
                return;
            }

            if (args.Length == 2)
            {
                var report = ResolveForDataRange(args);
                Console.Write(report);
                return;
            }

            HandleWrongArgumentsError();
        }

        private string ResolveForYear(
            (string Repo, string Loca) adrTuple,
            string year)
        {
            Console.WriteLine("Year: " + year);
            if (year.Count() != 4)
            {
                throw new Exception();
            }

            var adrTuple02 = repoService.Methods
                .GetAdrTupleByName(adrTuple, year);

            var statistics = workerStatistics.GetStatistics(adrTuple02);
            var report = workerStatistics.CreateReportText(statistics);
            return report;
        }

        private string ResolveForDataRange(string[] args)
        {
            var start = args[0];
            var stop = args[1];
            Console.WriteLine("Start: " + start);
            Console.WriteLine("Stop: " + stop);

            var workerStatistics = new StatisticsWorker();
            //var statistics2020 = workerStatistics.GetStatistics(new SheetInfo2021(), start, stop);
            //var report = workerStatistics.CreateReportText(statistics2020);
            //return report;
            return null;
        }

        private void HandleWrongArgumentsError()
        {
            var msg1 = "Sorry - You put wrong arguments";
            var msg2 = "Example: GameStat 2021";
            Console.WriteLine(msg1);
            Console.WriteLine(msg2);
        }
    }
}
