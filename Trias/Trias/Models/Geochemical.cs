//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trias.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Geochemical
    {
        public string G_ID { get; set; }
        public string C_ID { get; set; }
        public string Authorizer { get; set; }
        public string isotope { get; set; }
        public Nullable<double> Isotopepersents { get; set; }
        public string Element { get; set; }
        public Nullable<double> Elementpersents { get; set; }
        public string Time { get; set; }
        public string SubTime { get; set; }
        public Nullable<int> Time2 { get; set; }
        public string Position { get; set; }
        public string Notes { get; set; }
    }
}
