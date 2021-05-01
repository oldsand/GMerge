namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveTemplate
    {
        protected ArchiveTemplate()
        {
        }

        public ArchiveTemplate(int templateId, string templateName)
        {
            TemplateId = templateId;
            TemplateName = templateName;
        }

        public int TemplateId { get; private set; }
        public string TemplateName { get; private set; }
    }
}