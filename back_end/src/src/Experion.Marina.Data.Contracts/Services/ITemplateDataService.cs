using Experion.Marina.Data.Contracts.Entities;
using Experion.Marina.Dto;
using System.Collections.Generic;

namespace Experion.Marina.Data.Contracts.Services
{
    public interface ITemplateDataService
    {
        void saveTemplate(IDetails createTemplate);
        IEnumerable<ITemplate> GetTemplateName(long userId);
        long SaveTemplateName(ITemplate createTemplate);
        IEnumerable<ITemplateDetails> GetTemplate(long templateId);
    }
}
