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
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // BaseUrl/Department/Index
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
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
                     _unitOfWork.DepartmentRepository.Add(department);
                    var result = _unitOfWork.Complete();
                    // 3. TempData Dictionary Object
                    // Transfer Data From Action To Action
                    if (result >  0)
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

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);

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
                    _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.Complete();
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
                    _unitOfWork.DepartmentRepository.Delete(department);
                    _unitOfWork.Complete();
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
