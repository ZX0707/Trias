using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trias.Models.ViewModel
{
    public class UserView
    {
        public string User_ID { get; set; }
        [MinLength(2, ErrorMessage="用户名长度必须大于2个字符")]
        [Display(Name="用户名")]
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string UserUnit { get; set; }
        public string UserEmail { get; set; }
        public string ResearchField { get; set; }
    }
}