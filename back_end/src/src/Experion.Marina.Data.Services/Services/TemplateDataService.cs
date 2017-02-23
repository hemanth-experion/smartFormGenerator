using Autofac;
using Experion.Marina.Data.Contracts.Entities;
using Experion.Marina.Data.Contracts.Services;
using Experion.Marina.Data.Services.Entities;
using Experion.Marina.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Experion.Marina.Data.Services.Services
{
    public class TemplateDataService : DataService<MarinaContext>, ITemplateDataService
    {
        public TemplateDataService(IComponentContext iocContext, IRepositoryContext context) : base(iocContext, context)
        {
        }
        private IRepository<TemplateDetails, long> TemplateRepository => DataContext.GetRepository<TemplateDetails, long>();
        private IRepository<Template, long> TemplateIdRepository => DataContext.GetRepository<Template, long>();


        //Insert Template Into Database
        public void saveTemplate(IDetails template)
        {
            //find template id

            var param = new Specification<Template>(x => (x.TemplateName== template.TemplateName));
            var entity = TemplateIdRepository.GetBySpecification(param).FirstOrDefault();
            long tid = entity.Id;
            var templateDetails= new TemplateDetails
            {
                TemplateId = tid,
                Label = template.Label,
                ControlType = template.ControlType,
                IsReadOnly = template.IsReadOnly,
                IsVisible = template.IsVisible,
                Order = template.Order
            };
            TemplateRepository.Add(templateDetails);
            Save();
        }

        //Get Template Name of Specified Template Id
        public IEnumerable<ITemplate> GetTemplateName(long userId)
        {
            var param = new Specification<Template>(x => (x.UserId == userId));
            var entity = TemplateIdRepository.GetBySpecification(param);
            return entity;
        }

        //Set Template Name
        public long SaveTemplateName(ITemplate template)
        {
            var templates = new Template
            {
                TemplateName=template.TemplateName,
                UserId=template.UserId
            };
            TemplateIdRepository.Add(templates);
            Save();
            var param = new Specification<Template>(x => (x.TemplateName == template.TemplateName));
            return TemplateIdRepository.GetBySpecification(param).FirstOrDefault().Id;
            
        }

        //Get Template of specified Id
        public IEnumerable<ITemplateDetails> GetTemplate(long templateId)
        {
            var param = new Specification<TemplateDetails>(x => (x.TemplateId == templateId));
            return TemplateRepository.GetBySpecification(param).OrderBy(c => c.Order);
        }
    }
}
