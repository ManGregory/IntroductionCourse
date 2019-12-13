namespace TestRunner.CommonTypes.Interfaces
{
    public interface IMethodTestInfo : ITestInfo
    {
        object[] InputParameters { get; set; }
    }
}
