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
    /// 剖面
    /// </summary>
    public partial class SectionView
    {
        [Display(Name = "主键")]
        public string S_ID { get; set; }
        [Display(Name = "添加人Id")]
        public string User_ID { get; set; }
        [Display(Name = "剖面名称")]
        [Required(ErrorMessage = "剖面名称必填")]
        public string SectionName { get; set; }
        [Display(Name = "文献Id")]
        public string RID1 { get; set; }
        [Display(Name = "文献Id")]
        public string RID2 { get; set; }
        [Display(Name = "文献Id")]
        public string RID3 { get; set; }
        [Display(Name = "时代")]
        public string Time { get; set; }
        [Display(Name = "子时代")]
        public string SubTime { get; set; }
        [Display(Name = "时代（百万年）")]
        public Nullable<int> Time2 { get; set; }
        [Display(Name = "核准时间")]
        public Nullable<System.DateTime> EnterTime { get; set; }
        [Required(ErrorMessage = "核准人不能为空")]
        [Display(Name = "核准人")]
        public string Authorizer { get; set; }
        [Required(ErrorMessage = "国家不能为空")]
        [Display(Name = "国家")]
        public string Country { get; set; }
        [Required(ErrorMessage = "省份不能为空")]
        [Display(Name = "省份")]
        public string Province { get; set; }
        [Required(ErrorMessage = "城市不能为空")]
        [Display(Name = "城市")]
        public string City { get; set; }
        [Display(Name = "县市")]
        public string County { get; set; }
        [Display(Name = "村镇")]
        public string Village { get; set; }
        [Display(Name = "经度")]
        [Required(ErrorMessage = "GPS不能为空")]
        public Nullable<int> LonDegrees { get; set; }
        [Display(Name = "经度")]
        public Nullable<int> LonMinutes { get; set; }
        [Display(Name = "经度")]
        public Nullable<int> LonSeconds { get; set; }
        [Required(ErrorMessage = "GPS不能为空")]
        [Display(Name = "纬度")]
        public Nullable<int> LatDegrees { get; set; }
        [Display(Name = "纬度")]
        public Nullable<int> LatMinutes { get; set; }
        [Display(Name = "纬度")]
        public Nullable<int> LatSeconds { get; set; }
        [Display(Name = "构造板块")]
        public string TectonicPlate { get; set; }
        [Display(Name = "备注")]
        public string Notes { get; set; }
        [Display(Name = "评论")]
        public string Comments { get; set; }
        [Display(Name = "海拔")]
        public Nullable<double> Altitude { get; set; }
        [Display(Name="到时代（百万年）")]
        public Nullable<int> Time2End { get; set; }
        [Display(Name="到时代")]
        public string TimeEnd { get; set; }
        [Display(Name = "到子时代")]
        public string SubTimeEnd { get; set; }
        [Display(Name = "厚度")]
        public Nullable<int> Sthickness { get; set; }

    }
}
