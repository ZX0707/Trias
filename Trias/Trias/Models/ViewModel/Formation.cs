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
    /// 岩石组
    /// </summary>
    public partial class FormationView
    {
        [Display(Name = "主键")]
        public string F_ID { get; set; }
        [Display(Name = "剖面Id")]
        public string S_ID { get; set; }
        [Display(Name = "岩石组名")]
        public string FormationName { get; set; }
        [Display(Name = "沉积相带")]
        public string EnvironmentalZones { get; set; }
        [Display(Name = "厚度")]
        public Nullable<double> Thickness { get; set; }
        [Display(Name = "评论")]
        public string Comments { get; set; }
    }
}
