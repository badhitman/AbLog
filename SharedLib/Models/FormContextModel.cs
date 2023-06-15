namespace SharedLib
{
    /// <summary>
    /// 
    /// </summary>
    public class FormContextModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Uri FormAction { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> DataContext { get; set; } = new Dictionary<string, object>();
    }
}