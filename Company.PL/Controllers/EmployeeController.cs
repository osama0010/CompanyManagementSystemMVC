using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,// Ask CLR for creating object from class implementing IUnitOfWork
                        IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchValue))
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(SearchValue);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);
        }

        public async Task<IActionResult> Search(string SearchValue)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchValue))
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(SearchValue);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return PartialView("PartialViews/EmployeeTablePartialView", mappedEmployees);

        }


        public IActionResult Create()
        {
            //ViewBag.departments = _departmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validations
            {
                try
                {

                    string FileName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    employeeVM.ImageName = FileName;

                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
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
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(viewName, mappedEmployee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int? id)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    if(employeeVM.Image is not null)
                    {
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    }

                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
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
            ViewBag.departments = _unitOfWork.EmployeeRepository.GetAllAsync();
            return View(employeeVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                    var result = await _unitOfWork.CompleteAsync();

                    if (result > 0 && employeeVM.ImageName is not null)
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

    }
}
