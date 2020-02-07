namespace WebLMS.Services
{
    public static class ObjectExtensions
    {
        public static string ToHumanView(this object obj)
        {
            if (obj == null) return string.Empty;

            string humanView = obj.ToString();
            var objType = obj.GetType();
            if (objType.IsArray)
            {
                if (objType.GetElementType() == typeof(int))
                {
                    var array = obj as int[];
                    humanView = $"[{string.Join(", ", array)}]";
                }
                else if (objType.GetElementType() == typeof(double))
                {
                    var array = obj as double[];
                    humanView = $"[{string.Join(", ", array)}]";
                }
                else if (objType.GetElementType() == typeof(decimal))
                {
                    var array = obj as decimal[];
                    humanView = $"[{string.Join(", ", array)}]";
                }
            }
            return humanView;
        }
    }
}
