﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 层
    /// </summary>
    public partial class UnitView
    {
        [Display(Name = "主键")]
        public string U_ID { get; set; }
        [Display(Name = "岩石组Id")]
        public string F_ID { get; set; }
        [Display(Name = "厚度")]
        public Nullable<double> Thickness { get; set; }
        [Display(Name = "基准点")]
        public string ContactBase { get; set; }
        [Display(Name = "外观")]
        public string Facies { get; set; }
        [Display(Name = "沉积环境")]
        public string Environments { get; set; }
        [Display(Name = "备注")]
        public string Details { get; set; }
    }
}
