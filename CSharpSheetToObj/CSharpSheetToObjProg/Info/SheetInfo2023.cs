namespace CSharpSheetToObjProg.Info
{
    public class SheetInfo2023 : SheetInfoBase
    {
        public SheetInfo2023()
            : base(("GameStatistics", "1Ap1QGiDrl0Fce64cSuDCgERo9EkknPqj"), "2023")
        {
            PreviousSheetInfo = new SheetInfoBase[]
            {
                new SheetInfo2022(),
                new SheetInfo2021(),
            };
        }
    }
}
