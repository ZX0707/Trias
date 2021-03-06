//------------------------------------------------------------------------------
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
    /// 文献
    /// </summary>
    public partial class ReferenceView
    {
        [Display(Name = "主键")]
        public string R_ID { get; set; }
        [Display(Name = "文献类型")]
        [Required(ErrorMessage = "文献类型必填")]
        public string ReferenceType { get; set; }
        [Display(Name = "第一作者")]
        [Required(ErrorMessage = "至少有一位作者")]
        public string FirstAuthor { get; set; }
        [Display(Name = "其他作者")]
        public string OtherAuthors { get; set; }
        [Display(Name = "文献年度")]
        [Required(ErrorMessage = "文献年度必填")]
        public int Year { get; set; }
        [Display(Name = "文献名称")]
        public string Title { get; set; }
        [Display(Name = "书名")]
        public string BookTitle { get; set; }
        [Display(Name = "杂志名")]
        public string Journal { get; set; }
        [Display(Name = "编辑")]
        public string Editor1 { get; set; }
        [Display(Name = "语言")]
        [Required(ErrorMessage = "语言必选")]
        public string Language { get; set; }
        [Display(Name = "出版社")]
        public string Publisher { get; set; }
        [Display(Name = "卷")]
        public string Volume { get; set; }
        [Display(Name = "册")]
        public string No { get; set; }
        [Display(Name = "起始页")]
        public string PageBegin { get; set; }
        [Display(Name = "终止页")]
        public string PageEnd { get; set; }
        [Display(Name = "文献标识")]
        public string DOI { get; set; }
        [Display(Name = "链接")]
        public string URL1 { get; set; }
        [Display(Name = "链接")]
        public string URL2 { get; set; }
        [Display(Name = "评论")]
        public string Comments { get; set; }
    }
}
