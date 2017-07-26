using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PermissionMaking.ASP.NET.MVC.Models
{
    public class ConnectionContext:DbContext
    {
        public ConnectionContext():base("ConnectionName")
        {
            
        }

        public DbSet<UserAccountPermission> UserAccountPermissions { get; set; }

    }
}