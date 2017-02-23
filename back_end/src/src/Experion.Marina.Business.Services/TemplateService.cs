using Autofac;
using Experion.Marina.Business.Services.Contracts;
using Experion.Marina.Core;
using Experion.Marina.Data.Contracts.Entities;
using Experion.Marina.Data.Contracts.Services;
using Experion.Marina.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Experion.Marina.Business.Services
{
    public class TemplateService:BusinessService,ITemplateService
    {
        private IComponentContext _iocContext;
        public TemplateService(IComponentContext iocContext)
        {
            _iocContext = iocContext;
        }

        //Insert template to db
        public UserValidDto CreateTemplate(TemplateDTO templateDto)
        {
            var templateDataService = _iocContext.Resolve<ITemplateDataService>();
            var template = _iocContext.Resolve<IDetails>();
            template.TemplateName = templateDto.TemplateName;
            template.UserId = templateDto.UserId;
            template.Label = templateDto.Label;
            template.ControlType = templateDto.ControlType;
            template.IsReadOnly = templateDto.IsReadOnly;
            template.IsVisible = templateDto.IsVisible;
            template.Order = templateDto.Order;
            templateDataService.saveTemplate(template);
            return new UserValidDto
            {

                IsValid = true
            };
        }

        //Get template Name of specified templateId
        public IEnumerable<TemplateNameDTO> GetTemplateName(long userId)
        {
            var template = _iocContext.Resolve<ITemplateDataService>();
            var templateName = template.GetTemplateName(userId);
            return templateName.Select(c => new TemplateNameDTO { Id = c.Id, TemplateName = c.TemplateName });
        }

        //Set Template Name
        public long SetTemplateName(TemplateNameDTO templateDto)
        {
            var templateDataService = _iocContext.Resolve<ITemplateDataService>();
            var template = _iocContext.Resolve<ITemplate>();
            template.TemplateName = templateDto.TemplateName;
            template.UserId = templateDto.UserId;            
            return templateDataService.SaveTemplateName(template);
        }

        //Get Template of specified Id
        public IEnumerable<TemplateDTO> GetTemplate(long templateId)
        {
            var template = _iocContext.Resolve<ITemplateDataService>();
            var templateDetails = template.GetTemplate(templateId);
            return templateDetails.Select(c => new TemplateDTO {
                Label = c.Label,
            ControlType = c.ControlType,
            IsReadOnly = c.IsReadOnly,
            IsVisible = c.IsVisible,
            Order = c.Order
        });


        }
    }
}
