namespace TestRunner.CommonTypes.Interfaces
{
    public interface ITestInfo
    {
        int Id { get; set; }
        string Name { get; set; }
        object ExpectedResult { get; set; }
        bool IsCompilation { get; set; }
    }
}
