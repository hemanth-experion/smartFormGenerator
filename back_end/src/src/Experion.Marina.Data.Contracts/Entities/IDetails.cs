using System;

namespace Experion.Marina.Data.Contracts.Entities
{
    public interface IDetails
    {
        long Id { get; set; }
        long TemplateId { get; set; }
        string Label { get; set; }
        string ControlType { get; set; }
        Boolean IsReadOnly { get; set; }
        Boolean IsVisible { get; set; }
        int Order { get; set; }
        string TemplateName { get; set; }
        int UserId { get; set; }


    }
}
