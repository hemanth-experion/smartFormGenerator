using System;

namespace Experion.Marina.Dto
{
    public class TemplateDTO
    {
        public long Id { get; set; }
        public long TemplateId { get; set; }
        public string Label { get; set; }
        public string ControlType { get; set; }
        public Boolean IsReadOnly { get; set; }
        public Boolean IsVisible { get; set; }
        public int Order { get; set; }
        public int UserId { get; set; }
        public string TemplateName { get; set; }
    }
}
