using Microsoft.AspNetCore.Mvc;
using Company.BLL.Repositories;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // BaseUrl/Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        //[HttpGet] /*By default*/
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if(ModelState.IsValid) // Server Side Validations
            {
                try
                {
                     await _unitOfWork.DepartmentRepository.AddAsync(department);
                    var result = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if(id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if(department is null)
                return NotFound();

            return View(ViewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();

            //var department = _departmentRepository.GetById(id.Value);

            //if (department is null)
            //    return NotFound();

            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department,[FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department,[FromRoute] int id)
        {
            if(id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Delete(department);
                    await _unitOfWork.CompleteAsync();
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
