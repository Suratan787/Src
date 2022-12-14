using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichnessSoft.Entity.Model
{
    public class ServiceUnit : BaseModel
    {
        public int companyid { get; set; } = default;
        public int ServiceId { get; set; }
        public int umid { get; set; }
        public int stdumid { get; set; }

        public decimal qty { get; set; } = 0M;
        public decimal stdprice { get; set; } = 0M;
        public decimal memberprice1 { get; set; } = 0M;
        public decimal memberprice2 { get; set; } = 0M;
        public decimal memberprice3 { get; set; } = 0M;
        public decimal memberprice4 { get; set; } = 0M;
        public decimal memberprice5 { get; set; } = 0M;
        public decimal memberprice6 { get; set; } = 0M;
        public decimal memberprice7 { get; set; } = 0M;
        public decimal memberprice8 { get; set; } = 0M;
        public int defaultsale { get; set; } = 1;
        public int defaultbuy { get; set; } = 0;
        public int seq { get; set; } = default;

        public Company Company { get; set; }
        public Service Service { get; set; }


        //#region Expression Field

        //[NotMapped]
        //[Description("หน่วยนับ")]
        //public um ExpUnit => _context?.Set<um>()?.AsNoTrackingWithIdentityResolution()?.Where(c => c.CorpCode == this.CorpCode && c.code == this.UmCode)?.FirstOrDefault() ?? null;

        //[NotMapped]
        //[Description("หน่วยนับมาตรฐาน")]
        //public um ExpStdUnit => _context?.Set<um>()?.AsNoTrackingWithIdentityResolution()?.Where(c => c.CorpCode == this.CorpCode && c.code == this.StdUmCode)?.FirstOrDefault() ?? null;
        //#endregion


    }
}
