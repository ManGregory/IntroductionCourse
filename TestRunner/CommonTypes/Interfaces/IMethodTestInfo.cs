namespace TestRunner.CommonTypes.Interfaces
{
    public interface IMethodTestInfo
    {
        string Name { get; set; }
        object[] InputParameters { get; set; }
        object ExpectedResult { get; set; }
        bool IsCompilation { get; set; }
    }
}
