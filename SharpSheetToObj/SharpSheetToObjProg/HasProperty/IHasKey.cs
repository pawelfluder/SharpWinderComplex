namespace SharpSheetToObjProg.HasProperty
{
    internal interface IHasKey
    {
        Func<string> GetKeyFunc();
    }
}