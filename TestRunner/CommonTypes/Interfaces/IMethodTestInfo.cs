namespace TestRunner.CommonTypes.Interfaces
{
    public interface IMethodTestInfo
    {
        int Id { get; set; }
        string Name { get; set; }
        object[] InputParameters { get; set; }
        object ExpectedResult { get; set; }
        bool IsCompilation { get; set; }
    }
}
