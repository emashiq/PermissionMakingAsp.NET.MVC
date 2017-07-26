using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermissionMaking.ASP.NET.MVC.Models;

namespace PermissionMaking.ASP.NET.MVC.SubClass
{
    public class PermissionRepository 
    {
        private ConnectionContext connectionContext;

        public PermissionRepository(ConnectionContext cn)
        {
            connectionContext = cn;
        }
        public void PermissionSetUp(IEnumerable<string> Permission, string UserId)
        {
            List<UserAccountPermission> userAccountPermissions = new List<UserAccountPermission>();
            userAccountPermissions = connectionContext.UserAccountPermissions.Where(x => x.UserId == UserId).ToList();
            if (userAccountPermissions != null)
            {
                connectionContext.UserAccountPermissions.RemoveRange(userAccountPermissions);
            }
            if (Permission != null)
            {
                foreach (string item in Permission)
                {
                    string[] Permissions = item.Split('_');
                    UserAccountPermission _userAccountPermission = new UserAccountPermission();
                    _userAccountPermission.UserId = UserId;
                    _userAccountPermission.Areas = Permissions[0];
                    _userAccountPermission.Controllers = Permissions[1];
                    _userAccountPermission.Actions = Permissions[2];
                    _userAccountPermission.IsHttpPosted = (Permissions[3] == "True" ? true : false);
                    connectionContext.UserAccountPermissions.Add(_userAccountPermission);
                }
            }

            connectionContext.SaveChanges();
        }
    }
}