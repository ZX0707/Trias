using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trias.Models;

namespace Trias.Tool
{
    public class UserMgr
    {
        public static User CurrUserInfo()
        {
            if (HttpContext.Current.Session[Keys.Login_UserInfo] != null)
            {
                return HttpContext.Current.Session[Keys.Login_UserInfo] as User;
            }
            return new User() { };
        }
    }
}