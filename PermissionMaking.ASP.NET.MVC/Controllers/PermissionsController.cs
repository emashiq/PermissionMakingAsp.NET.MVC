using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PermissionMaking.ASP.NET.MVC.Library;
using PermissionMaking.ASP.NET.MVC.Models;

namespace PermissionMaking.ASP.NET.MVC.Controllers
{
    public class PermissionsController : Controller
    {
        // GET: Permissions
        public ActionResult Permission(string Id)
        {
            var list = AreaControllerActionTreeItems.GetSubClasses<Controller>();
            ViewBag.UserId = Id;
            // Get all controllers with their actions
            var getAllcontrollers = (from item in list
                let name = item.Name
                where !item.Name.StartsWith("MVC") // I'm using T4MVC
                select new MyController()
                {
                    Name = name.Replace("Controller", ""),
                    Namespace = item.Namespace,
                    MyActions = AreaControllerActionTreeItems.GetListOfAction(item)
                }).ToList();

            // Now we will get all areas that has been registered in route collection
            var getAllAreas = RouteTable.Routes.OfType<Route>()
                .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                .Select(
                    r =>
                        new MyArea
                        {
                            Name = r.DataTokens["area"].ToString(),
                            Namespace = r.DataTokens["Namespaces"] as IList<string>,
                        }).Where(x => x.Name != "Company").ToList()
                .Distinct().ToList();


            // Add a new area for default controllers
            getAllAreas.Insert(0, new MyArea()
            {
                Name = "Main",
                Namespace = new List<string>()
                {
                    typeof (Controllers.PermissionsController).Namespace
                }
            });
            string MainMvcControllerNamespace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name +
                                                ".Controllers";
            foreach (var area in getAllAreas)
            {

                var temp = new List<MyController>();
                foreach (var item in area.Namespace)
                {
                    temp.AddRange(getAllcontrollers.Where(x => x.Namespace == (item == MainMvcControllerNamespace ? item : item.Remove(item.Length - 1) + "Controllers")).ToList());
                }
                area.MyControllers = temp;
            }

            return View(getAllAreas);
        }
    }
}