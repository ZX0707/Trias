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
    /// 字典
    /// </summary>
    public partial class DictView
    {
        [Display(Name = "主键")]
        public string Id { get; set; }
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "建值")]
        public string KeyName { get; set; }
        [Display(Name = "文本")]
        public string ValueName { get; set; }
        [Display(Name = "排序")]
        public int Sort { get; set; }
        [Display(Name = "上级Id")]
        public string Pid { get; set; }
    }
}
