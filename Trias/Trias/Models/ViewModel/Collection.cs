﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Trias.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 采样位置
    /// </summary>
    public partial class CollectionView
    {
        [Display(Name = "主键")]
        public string C_ID { get; set; }
        [Display(Name = "层Id")]
        public string U_ID { get; set; }
        [Display(Name = "精确度")]
        public string Precision { get; set; }
        [Display(Name = "备注")]
        public string Details { get; set; }
        [Display(Name = "开始位置")]
        public Nullable<double> Depth1 { get; set; }
        [Display(Name = "结束位置")]
        public Nullable<double> Depth2 { get; set; }
        public Nullable<int> sort { get; set; }
    }
}
