using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SIS.Application.DTOs;
using SIS.Application.Interfaces;
using SIS.Web.ViewModels;

namespace SIS.Web.Controllers;

public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;

    public StudentController(IStudentService studentService, IMapper mapper)
    {
        _studentService = studentService;
        _mapper = mapper;
    }

    // GET: Student
    public async Task<IActionResult> Index(string? searchTerm)
    {
        IEnumerable<StudentDto> students;
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            students = await _studentService.SearchStudentsAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
        }
        else
        {
            students = await _studentService.GetAllStudentsAsync();
        }
        
        var viewModels = _mapper.Map<IEnumerable<StudentViewModel>>(students);
        return View(viewModels);
    }

    // GET: Student/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<StudentViewModel>(student);
        var enrollments = await _studentService.GetStudentEnrollmentsAsync(id);
        ViewBag.Enrollments = enrollments;
        
        return View(viewModel);
    }

    // GET: Student/Create
    public IActionResult Create()
    {
        return View(new CreateStudentViewModel());
    }

    // POST: Student/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStudentViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            var dto = _mapper.Map<CreateStudentDto>(viewModel);
            await _studentService.CreateStudentAsync(dto);
            TempData["SuccessMessage"] = "Student created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    // GET: Student/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<EditStudentViewModel>(student);
        return View(viewModel);
    }

    // POST: Student/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditStudentViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            var dto = _mapper.Map<UpdateStudentDto>(viewModel);
            await _studentService.UpdateStudentAsync(id, dto);
            TempData["SuccessMessage"] = "Student updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    // GET: Student/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<StudentViewModel>(student);
        return View(viewModel);
    }

    // POST: Student/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _studentService.DeleteStudentAsync(id);
        if (result)
        {
            TempData["SuccessMessage"] = "Student deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete student.";
        }
        return RedirectToAction(nameof(Index));
    }
}
