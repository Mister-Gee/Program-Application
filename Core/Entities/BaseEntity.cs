using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public abstract class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
