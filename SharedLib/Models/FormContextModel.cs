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
        public Dictionary<string, string> DataContext { get; set; } = new ();
    }
}