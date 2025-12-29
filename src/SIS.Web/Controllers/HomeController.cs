using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SIS.Application.Interfaces;
using SIS.Web.Models;

namespace SIS.Web.Controllers;

public class HomeController : Controller
{
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;
    private readonly IEnrollmentService _enrollmentService;
    private readonly ISemesterService _semesterService;

    public HomeController(
        IStudentService studentService,
        ICourseService courseService,
        IEnrollmentService enrollmentService,
        ISemesterService semesterService)
    {
        _studentService = studentService;
        _courseService = courseService;
        _enrollmentService = enrollmentService;
        _semesterService = semesterService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllStudentsAsync();
        var courses = await _courseService.GetAllCoursesAsync();
        var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
        var currentSemester = await _semesterService.GetCurrentSemesterAsync();

        ViewBag.TotalStudents = students.Count();
        ViewBag.TotalCourses = courses.Count();
        ViewBag.TotalEnrollments = enrollments.Count();
        ViewBag.CurrentSemester = currentSemester?.SemesterName ?? "No Active Semester";

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
