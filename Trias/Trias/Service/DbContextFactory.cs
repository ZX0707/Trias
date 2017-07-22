using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Trias.Models;

namespace Trias.Service
{
    public class DbContextFactory
    {
        //获取当前EF上下文的唯一实例
        public static TriasEntities GetCurrentThreadInstance()
        {
            var obj = CallContext.GetData(typeof(DbContextFactory).FullName) as TriasEntities;
            if (obj == null)
            {
                obj = new TriasEntities();
                CallContext.SetData(typeof(DbContextFactory).FullName, obj);
            }
            return obj;
        }

    }
}