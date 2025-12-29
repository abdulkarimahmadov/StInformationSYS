using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIS.Application.DTOs;
using SIS.Application.Interfaces;
using SIS.Web.ViewModels;

namespace SIS.Web.Controllers;

public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IDepartmentService _departmentService;
    private readonly ITeacherService _teacherService;
    private readonly ISemesterService _semesterService;
    private readonly IMapper _mapper;

    public CourseController(
        ICourseService courseService,
        IDepartmentService departmentService,
        ITeacherService teacherService,
        ISemesterService semesterService,
        IMapper mapper)
    {
        _courseService = courseService;
        _departmentService = departmentService;
        _teacherService = teacherService;
        _semesterService = semesterService;
        _mapper = mapper;
    }

    // GET: Course
    public async Task<IActionResult> Index(string? searchTerm)
    {
        IEnumerable<CourseDto> courses;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            courses = await _courseService.SearchCoursesAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
        }
        else
        {
            courses = await _courseService.GetAllCoursesAsync();
        }

        var viewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        return View(viewModels);
    }

    // GET: Course/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<CourseViewModel>(course);
        var enrollments = await _courseService.GetCourseEnrollmentsAsync(id);
        ViewBag.Enrollments = enrollments;

        return View(viewModel);
    }

    // GET: Course/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropDownLists();
        return View(new CreateCourseViewModel());
    }

    // POST: Course/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCourseViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropDownLists();
            return View(viewModel);
        }

        try
        {
            var dto = _mapper.Map<CreateCourseDto>(viewModel);
            await _courseService.CreateCourseAsync(dto);
            TempData["SuccessMessage"] = "Course created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateDropDownLists();
            return View(viewModel);
        }
    }

    // GET: Course/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<EditCourseViewModel>(course);
        await PopulateDropDownLists(viewModel.DepartmentId, viewModel.TeacherId, viewModel.SemesterId);
        return View(viewModel);
    }

    // POST: Course/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditCourseViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropDownLists(viewModel.DepartmentId, viewModel.TeacherId, viewModel.SemesterId);
            return View(viewModel);
        }

        try
        {
            var dto = _mapper.Map<UpdateCourseDto>(viewModel);
            await _courseService.UpdateCourseAsync(id, dto);
            TempData["SuccessMessage"] = "Course updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateDropDownLists(viewModel.DepartmentId, viewModel.TeacherId, viewModel.SemesterId);
            return View(viewModel);
        }
    }

    // GET: Course/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<CourseViewModel>(course);
        return View(viewModel);
    }

    // POST: Course/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        if (result)
        {
            TempData["SuccessMessage"] = "Course deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete course.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropDownLists(int? selectedDepartment = null, int? selectedTeacher = null, int? selectedSemester = null)
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        var teachers = await _teacherService.GetAllTeachersAsync();
        var semesters = await _semesterService.GetAllSemestersAsync();

        ViewBag.Departments = new SelectList(departments, "Id", "DepartmentName", selectedDepartment);
        ViewBag.Teachers = new SelectList(teachers, "Id", "FullName", selectedTeacher);
        ViewBag.Semesters = new SelectList(semesters, "Id", "SemesterName", selectedSemester);
    }
}
