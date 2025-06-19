using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Admin
    {
        [key]
        public int Id { get; set; }
        //Foreign Keys
        [ForeignKey("GeneralSetting")]
        public int? GeneralSettingId { get; set; }
        //Navigation Properties
        public virtual GeneralSetting? GeneralSetting { get; set; }

    }
}
