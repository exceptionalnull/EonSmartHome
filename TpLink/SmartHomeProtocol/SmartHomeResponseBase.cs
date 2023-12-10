using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    internal class SmartHomeResponseBase
    {
        [JsonProperty(PropertyName = "err_code")]
        public int ErrorCode { get; set; }
    }
}
