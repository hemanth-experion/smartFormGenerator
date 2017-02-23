using Experion.Marina.Data.Contracts.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Experion.Marina.Data.Services.Entities
{
    public class TemplateDetails:ITemplateDetails, IEntity<long>
    {
        public long Id { get; set; }
        [Required]
        public long TemplateId { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public string ControlType { get; set; }
        public Boolean IsReadOnly { get; set; }
        public Boolean IsVisible { get; set; }
        [Required]
        public int Order { get; set; }
        public virtual Template template { get; set; }
    }
}
