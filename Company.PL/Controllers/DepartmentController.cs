using Microsoft.AspNetCore.Mvc;
using Company.BLL.Repositories;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Company.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)// Asking CLR to create object from class that implements IDepRepo (Dependency Injection)
        {
            _departmentRepository = departmentRepository;
        }
        // BaseUrl/Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
        //[HttpGet] /*By default*/
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid) // Server Side Validations
            {
                try
                {
                    var result = _departmentRepository.Add(department);

                    if(result >  0)
                        TempData["message"] = "Department is created";

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1.log exception
                    // 2.View on form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
        // baseUrl/Department/Details/100
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if(id is null)
                return BadRequest();

            var department = _departmentRepository.GetById(id.Value);

            if(department is null)
                return NotFound();

            return View(ViewName, department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();

            //var department = _departmentRepository.GetById(id.Value);

            //if (department is null)
            //    return NotFound();

            return Details(id, "Edit");
        }

        [HttpPost]
        public IActionResult Edit(Department department,[FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1.log exception
                    // 2.View on form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department,[FromRoute] int id)
        {
            if(id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRepository.Delete(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception EX)
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                }
            }
            return View(department);
        }

    }
}
