namespace TestRunner.CommonTypes.Interfaces
{
    public interface IMethodTestInfo
    {
        string AdditionalMessage { get; set; }
        object ExpectedResult { get; set; }
        object[] InputParameters { get; set; }
        string Name { get; set; }
        bool IsCompilation { get; set; }
    }
}
