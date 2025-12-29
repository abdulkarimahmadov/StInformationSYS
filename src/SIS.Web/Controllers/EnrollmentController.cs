using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIS.Application.DTOs;
using SIS.Application.Interfaces;
using SIS.Web.ViewModels;

namespace SIS.Web.Controllers;

public class EnrollmentController : Controller
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;

    public EnrollmentController(
        IEnrollmentService enrollmentService,
        IStudentService studentService,
        ICourseService courseService,
        IMapper mapper)
    {
        _enrollmentService = enrollmentService;
        _studentService = studentService;
        _courseService = courseService;
        _mapper = mapper;
    }

    // GET: Enrollment
    public async Task<IActionResult> Index()
    {
        var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
        var viewModels = _mapper.Map<IEnumerable<EnrollmentViewModel>>(enrollments);
        return View(viewModels);
    }

    // GET: Enrollment/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<EnrollmentViewModel>(enrollment);
        return View(viewModel);
    }

    // GET: Enrollment/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropDownLists();
        return View(new CreateEnrollmentViewModel());
    }

    // POST: Enrollment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEnrollmentViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropDownLists();
            return View(viewModel);
        }

        try
        {
            var dto = _mapper.Map<CreateEnrollmentDto>(viewModel);
            await _enrollmentService.CreateEnrollmentAsync(dto);
            TempData["SuccessMessage"] = "Enrollment created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateDropDownLists();
            return View(viewModel);
        }
    }

    // GET: Enrollment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<EditEnrollmentViewModel>(enrollment);
        return View(viewModel);
    }

    // POST: Enrollment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditEnrollmentViewModel viewModel)
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
            var dto = _mapper.Map<UpdateEnrollmentDto>(viewModel);
            await _enrollmentService.UpdateEnrollmentAsync(id, dto);
            TempData["SuccessMessage"] = "Enrollment updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    // GET: Enrollment/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<EnrollmentViewModel>(enrollment);
        return View(viewModel);
    }

    // POST: Enrollment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _enrollmentService.DeleteEnrollmentAsync(id);
        if (result)
        {
            TempData["SuccessMessage"] = "Enrollment deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete enrollment.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropDownLists()
    {
        var students = await _studentService.GetAllStudentsAsync();
        var courses = await _courseService.GetAllCoursesAsync();

        ViewBag.Students = new SelectList(students, "Id", "FullName");
        ViewBag.Courses = new SelectList(courses.Select(c => new { c.Id, Name = $"{c.CourseCode} - {c.CourseName}" }), "Id", "Name");
    }
}
