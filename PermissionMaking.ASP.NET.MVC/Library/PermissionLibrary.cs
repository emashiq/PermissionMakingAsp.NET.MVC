using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using PermissionMaking.ASP.NET.MVC.Models;

namespace PermissionMaking.ASP.NET.MVC.Library
{
    public static class AreaControllerActionTreeItems
    {
        public static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public static IEnumerable<MyAction> GetListOfAction(Type controller)
        {
            var navItems = new List<MyAction>();

            // Get a descriptor of this controller
            ReflectedControllerDescriptor controllerDesc = new ReflectedControllerDescriptor(controller);

            // Look at each action in the controller
            foreach (ActionDescriptor action in controllerDesc.GetCanonicalActions())
            {
                bool validAction = true;
                bool isHttpPost = false;

                // Get any attributes (filters) on the action
                object[] attributes = action.GetCustomAttributes(false);

                // Look at each attribute
                foreach (object filter in attributes)
                {
                    // Can we navigate to the action?
                    if (filter is ChildActionOnlyAttribute)
                    {
                        validAction = false;
                        break;
                    }
                    if (filter is HttpPostAttribute)
                    {
                        isHttpPost = true;
                    }

                }

                // Add the action to the list if it's "valid"
                if (validAction)
                    navItems.Add(new MyAction()
                    {
                        Name = action.ActionName,
                        IsHttpPost = isHttpPost
                    });
            }
            return navItems;
        }
    }
}