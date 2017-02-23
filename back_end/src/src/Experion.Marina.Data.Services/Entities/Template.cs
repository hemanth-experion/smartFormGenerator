using Experion.Marina.Data.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.Marina.Data.Services.Entities
{
    public class Template:ITemplate,IEntity<long>
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }
        public long UserId { get; set; }
        public virtual UserCredential user { get; set; }
    }
}
