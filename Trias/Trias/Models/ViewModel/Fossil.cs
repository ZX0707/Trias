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
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 化石
    /// </summary>
    public partial class FossilView
    {
        [Display(Name = "主键")]
        public string H_ID { get; set; }
        [Display(Name = "采样位置Id")]
        public string C_ID { get; set; }
        [Display(Name = "时代")]
        public string Time { get; set; }
        [Display(Name = "子时代")]
        public string SubTime { get; set; }
        [Display(Name = "时代（百万年）")]
        public Nullable<int> Time2 { get; set; }
        [Display(Name = "核准人")]
        public string Authorizer { get; set; }
        [Display(Name = "化石分类")]
        public string FossilGroup { get; set; }
        [Display(Name = "分类型")]
        public string TaxonType { get; set; }
        [Display(Name = "分册号")]
        public string CollectFromTreatise { get; set; }
        [Display(Name = "描述关系（同物异名）")]
        public string RefDescribingRelation { get; set; }
        [Display(Name = "属名")]
        [Required]
        public string GenusName { get; set; }
        [Display(Name = "种名")]
        [Required]
        public string SpeciesName { get; set; }
        [Display(Name = "距离剖面底部的位置")]
        public string Position { get; set; }
        [Display(Name = "长度")]
        public string Length { get; set; }
        [Display(Name = "宽度")]
        public string width { get; set; }
        [Display(Name = "图片")]
        public string Picture { get; set; }
        [Display(Name = "备注")]
        public string Notes { get; set; }
    }
}
