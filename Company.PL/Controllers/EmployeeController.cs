using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository,
                                    IDepartmentRepository departmentRepository,
                                    IMapper mapper) // Ask CLR for creating object from class implementing IEmployeeRepo and IDepartmentRepo
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchValue))
                employees = _employeeRepository.GetAll();

            else
                employees = _employeeRepository.GetEmployeeByName(SearchValue);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);

        }

        public IActionResult Create()
        {
            //ViewBag.departments = _departmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validations
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    var result = _employeeRepository.Add(mappedEmployee);

                    if (result > 0)
                        TempData["message"] = "Employee is created";

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

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeRepository.GetById(id.Value);

            if (employee is null)
                return NotFound();

            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(viewName, mappedEmployee);
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.departments = _departmentRepository.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int? id)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _employeeRepository.Update(mappedEmployee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1.log exception
                    // 2.View on form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.departments = _departmentRepository.GetAll();
            return View(employeeVM);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _employeeRepository.Delete(mappedEmployee);
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
