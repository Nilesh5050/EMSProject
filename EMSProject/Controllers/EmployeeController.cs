using EMSProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Operations.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EMSDBContext dbContext;

        [Obsolete]
        private IHostingEnvironment Environment { get; }

        [Obsolete]
        public EmployeeController(EMSDBContext _dbContext, IHostingEnvironment environment)
        {
            dbContext = _dbContext;
            Environment = environment;
        }

        public IActionResult Index()
        {


            var Employees = dbContext.Employees.ToList();
            return View(Employees);




        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Obsolete]
        public IActionResult Create(Employees emp)
        {

            var files = Request.Form.Files;
            string dbpath = string.Empty;
            if (files.Count > 0)
            {
                string path = Environment.WebRootPath;
                string fullpath = Path.Combine(path, "images", files[0].FileName);
                dbpath = "images/" + files[0].FileName;

                FileStream stream = new FileStream(fullpath, FileMode.Create);
                files[0].CopyTo(stream);
            }
            emp.Image = dbpath;
            dbContext.Employees.Add(emp);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return RedirectToAction("Index");
            }
            var empdetails = await dbContext.Employees.FindAsync(id);
            return View(empdetails);



        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employees nc)
        {

            if (ModelState.IsValid)
            {
                dbContext.Update(nc);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(nc);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var empdetails = await dbContext.Employees.FindAsync(id);
            return View(empdetails);



        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var empdetails = await dbContext.Employees.FindAsync(id);
            return View(empdetails);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
           
            var empdetails = await dbContext.Employees.FindAsync(id);
            dbContext.Employees.Remove(empdetails);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");



        }

    }
}
