using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trias.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class UserView
    {
        [Display(Name = "主键")]
        public string User_ID { get; set; }
        [MinLength(2, ErrorMessage = "用户名长度必须大于2个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [Display(Name = "密码")]
        public string UserPwd { get; set; }
        [Display(Name = "单位")]
        public string UserUnit { get; set; }
        [Display(Name = "邮件地址")]
        public string UserEmail { get; set; }
        [Display(Name = "研究方向")]
        public string ResearchField { get; set; }
        [Display(Name="是否通过验证")]
        public Nullable<bool> isPass { get; set; }
    }
}