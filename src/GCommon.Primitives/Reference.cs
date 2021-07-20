namespace GCommon.Primitives
{
    public class Reference
    {
        public const string DefaultReference = "---";
        
        public Reference()
        {
            FullReference = DefaultReference;
            ObjectReference = DefaultReference;
            AttributeReference = DefaultReference;
        }
        
        public string FullReference { get; set; }
        public string ObjectReference { get; set; }
        public string AttributeReference { get; set; }
    }
}