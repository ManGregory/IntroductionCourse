namespace RunAndTest.DTO
{
    public class MethodTestInfo : IMethodTestInfo
    {
        public string AdditionalMessage { get; set; }
        public object ExpectedResult { get; set; }
        public object[] InputParameters { get; set; }
        public string Name { get; set; }
        public bool IsCompilation { get; set; }
    }
}