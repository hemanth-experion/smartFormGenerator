using Experion.Marina.Dto;
using System.Collections.Generic;

namespace Experion.Marina.Business.Services.Contracts
{
    public interface ITemplateService
    {
        UserValidDto CreateTemplate(TemplateDTO templateDto);
        IEnumerable<TemplateNameDTO> GetTemplateName(long userId);
        long SetTemplateName(TemplateNameDTO templateDto);
        IEnumerable<TemplateDTO> GetTemplate(long templateId);
    }
}
