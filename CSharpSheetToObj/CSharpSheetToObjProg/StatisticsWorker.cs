using CommonTypesCoreProj.Objects;
using System.Text;
using GameStatisticsCoreProj.Counters;
using CSharpSheetToObjProg.Repet;
using SharpRepoServiceProg.Service;
using CSharpSheetToObjProg.Info;
using CommonTypesCoreProj.Extensions;

namespace GameStatisticsCoreProj
{
    public class StatisticsWorker
    {
        //private readonly IPersistencyService persistency;
        private readonly ApproachesCounter approachesCounter;
        private readonly ContactsCounter contactsCounter;
        private readonly SourceTypeCounter typeCounter;
        private readonly ConversionCounter conversionCounter;
        private readonly DatesCounter datesCounter;
        private readonly IRepoService repoService;

        public StatisticsWorker()
        {
            approachesCounter = new ApproachesCounter();
            contactsCounter = new ContactsCounter();
            typeCounter = new SourceTypeCounter();
            conversionCounter = new ConversionCounter();
            datesCounter = new DatesCounter();
            repoService = MyBorder.Container.Resolve<IRepoService>();
        }

        //public List<(int, string)> GetStatistics(SheetInfoBase info, string start, string stop)
        //{
        //    (int startYear, int startMonth, int startDay) = GetYearMonthDay(start);
        //    (int stopYear, int stopMonth, int stopDay) = GetYearMonthDay(stop);

        //    var action = new Func<Approaches, bool>((x) => { return IsDateBetween((startYear, startMonth, startDay), (stopYear, stopMonth, stopDay), x.Date); });

        //    var statistics = GetStatistics(info, action);
        //    return statistics;
        //}

        //public List<(int, string)> GetStatistics(
        //    (string Repo, string Loca) adrTuple)
        //{
        //    //int year = GetTwoDigitsYearFromInfo(adrTuple.Loca);
        //    //var action = new Func<Approaches, bool>((x) => { return IsDateBetween((year, 1), (year, 12), x.Date); });

        //    var statistics = GetStatistics(adrTuple);
        //    return statistics;
        //}

        public List<(int, string)> GetStatistics(
            (string Repo, string Loca) mainAdrTuple)
        {
            var approachesAdrTuple = repoService
                .GetAdrTuple<Approaches>(mainAdrTuple);
            var rejectionsAdrTuple = repoService
                .GetAdrTuple<Rejections>(mainAdrTuple);
            var datesAdrTuple = repoService
                .GetAdrTuple<Dates>(mainAdrTuple);

            var approaches = repoService.GetItemList<Approaches>(approachesAdrTuple);
            var rejections = repoService.GetItemList<Rejections>(rejectionsAdrTuple);
            var dates = repoService.GetItemList<Dates>(datesAdrTuple);

            var result = new List<(int, string)>();
            result.AddRange(approachesCounter.GetData(approaches, rejections));
            result.AddRange(typeCounter.GetData(approaches));
            result.AddRange(contactsCounter.GetData(approaches));
            result.AddRange(conversionCounter.GetData(result));
            result.AddRange(datesCounter.GetData(dates));

            return result;
        }

        public string CreateReportText(List<(int, string)> input)
        {
            var newLine = '\n';
            var stringBuilder = new StringBuilder();

            foreach (var item in input)
            {
                var number = PrepareNumberString(item);
                stringBuilder.Append(number + " - " + item.Item2 + newLine);
            }

            return stringBuilder.ToString();
        }

        private int GetTwoDigitsYearFromInfo(SheetInfoBase info)
        {
            var twoLastChar = info.Year.Substring(2, 2);
            int year = int.Parse(twoLastChar);
            return year;
        }

        private string PrepareNumberString((int, string) item)
        {
            if (item.Item2.StartsWith("Conversion"))
            {
                return ((double)item.Item1 / 100).ToString();
            }

            var digits = item.Item1 == 0 ? 1 : (int)Math.Floor(Math.Log10(item.Item1) + 1);
            var pre = new string('_', 5 - digits);

            return pre + item.Item1;
        }

        private bool IsDateBetween((int, int) start, (int, int) stop, string date)
        {
            (int year, int month, int _) = GetYearMonthDay(date);

            if (year >= start.Item1 &&
                year <= stop.Item1 &&
                month >= start.Item2 &&
                month <= stop.Item2)
            {
                return true;
            }

            return false;
        }

        private bool IsDateBetween((int, int, int) start, (int, int, int) stop, string date)
        {
            (int year, int month, int day) = GetYearMonthDay(date);

            if (year >= start.Item1 &&
                year <= stop.Item1 &&
                month >= start.Item2 &&
                month <= stop.Item2 &&
                day >= start.Item3 &&
                day <= stop.Item3)
            {
                return true;
            }

            return false;
        }

        public static (int year, int month, int day) GetYearMonthDay(string dateString)
        {
            try
            {
                if (dateString.Length == 8)
                {
                    var year = int.Parse(dateString.Substring(0, 2));
                    int month = int.Parse(dateString.Substring(3, 2));
                    var day = int.Parse(dateString.Substring(6, 2));

                    return (year, month, day);
                }
            }
            catch
            { }

            return (0, 0, 0);
        }
    }
}
