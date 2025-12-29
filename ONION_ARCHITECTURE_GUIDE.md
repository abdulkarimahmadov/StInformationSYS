# Onion Architecture Implementation Guide

## Understanding Onion Architecture

### Core Principles

1. **Dependency Rule**: Dependencies point inward.  Inner layers never depend on outer layers. 
2. **Domain-Centric**: Business logic is at the center, independent of UI, database, or frameworks.
3. **Testability**: Inner layers can be tested without outer layers.
4. **Flexibility**: Easy to change UI, database, or external services without affecting business logic.

---

## Layer-by-Layer Implementation

### 1. Domain Layer (Core) - The Heart

**Purpose**: Contains enterprise business rules and entities.  Has NO dependencies. 

#### What Goes Here:
- ✅ Entities (Student, Course, etc.)
- ✅ Value Objects (Email, Address)
- ✅ Enums (Status types)
- ✅ Domain Interfaces
- ✅ Domain Events
- ✅ Business rule validations

#### What DOESN'T Go Here:
- ❌ Database code
- ❌ API calls
- ❌ UI concerns
- ❌ External library dependencies

#### Example Structure: 

```
SIS. Domain/
├── Common/
│   ├── BaseEntity.cs              # Base class for all entities
│   ├── IEntity.cs                 # Entity interface
│   └── IAggregateRoot.cs          # For DDD patterns
├── Entities/
│   ├── Student.cs                 # Student entity
│   ├── Course. cs                  # Course entity
│   ├── Enrollment.cs              # Enrollment entity
│   └── Grade.cs                   # Grade entity
├── ValueObjects/
│   ├── Email.cs                   # Email value object
│   ├── Address.cs                 # Address value object
│   └── PhoneNumber.cs             # Phone number value object
├── Enums/
│   ├── EnrollmentStatus.cs        # Enrollment statuses
│   ├── StudentStatus.cs           # Student statuses
│   └── GradeLevel.cs              # Grade levels
└── Interfaces/
    └── IDateTimeProvider.cs       # For testability
```

#### Key Implementation Details:

**BaseEntity. cs** - Foundation for all entities
```csharp
namespace SIS.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    public bool IsDeleted { get; protected set; } = false;

    // Domain events (optional, for advanced scenarios)
    private List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents. AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents. Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
```

**Student.cs** - Rich domain model
```csharp
namespace SIS.Domain.Entities;

public class Student : BaseEntity
{
    // Private setters to protect invariants
    public string StudentNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public PhoneNumber?  PhoneNumber { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string?  Gender { get; private set; }
    public Address? Address { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public int GradeLevel { get; private set; }
    public StudentStatus Status { get; private set; }

    // Navigation properties
    public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

    // Computed properties
    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.UtcNow.Year - DateOfBirth.Year;

    // Private constructor for EF Core
    private Student() { }

    // Factory method for creating new students
    public static Student Create(
        string studentNumber,
        string firstName,
        string lastName,
        Email email,
        DateTime dateOfBirth,
        DateTime enrollmentDate,
        int gradeLevel)
    {
        // Validation
        if (string. IsNullOrWhiteSpace(studentNumber))
            throw new ArgumentException("Student number is required", nameof(studentNumber));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));

        if (dateOfBirth > DateTime.UtcNow. AddYears(-5))
            throw new ArgumentException("Student must be at least 5 years old");

        var student = new Student
        {
            StudentNumber = studentNumber,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            DateOfBirth = dateOfBirth,
            EnrollmentDate = enrollmentDate,
            GradeLevel = gradeLevel,
            Status = StudentStatus.Active
        };

        return student;
    }

    // Business methods
    public void UpdatePersonalInfo(string firstName, string lastName, PhoneNumber?  phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }

    public void PromoteGrade()
    {
        GradeLevel++;
    }

    public void Suspend()
    {
        Status = StudentStatus.Suspended;
    }

    public void Reactivate()
    {
        Status = StudentStatus.Active;
    }

    public void Graduate()
    {
        Status = StudentStatus. Graduated;
    }

    public bool CanEnrollInCourse(Course course)
    {
        if (Status != StudentStatus.Active)
            return false;

        if (Enrollments.Any(e => e.CourseId == course.Id && e.Status == EnrollmentStatus.Enrolled))
            return false;

        return true;
    }
}
```

**Email.cs** - Value Object
```csharp
namespace SIS.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format");

        return new Email(email. ToLowerInvariant());
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail. MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Value object equality
    protected bool Equals(Email other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is Email other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static implicit operator string(Email email) => email.Value;
}
```

---

### 2. Application Layer - Business Logic Orchestration

**Purpose**: Contains application-specific business rules and orchestrates the flow of data. 

**Dependencies**: Domain Layer ONLY

#### What Goes Here: 
- ✅ Service Interfaces
- ✅ Service Implementations
- ✅ DTOs (Data Transfer Objects)
- ✅ Mapping profiles (AutoMapper)
- ✅ Validators (FluentValidation)
- ✅ Application exceptions
- ✅ Use case implementations

#### What DOESN'T Go Here:
- ❌ Database implementation details
- ❌ UI concerns
- ❌ HTTP request/response objects

#### Example Structure:

```
SIS.Application/
├── Interfaces/
│   ├── IStudentService.cs
│   ├── ICourseService.cs
│   ├── IEnrollmentService.cs
│   └── IUnitOfWork.cs
├── Services/
│   ├── StudentService.cs
│   ├── CourseService.cs
│   └── EnrollmentService.cs
├── DTOs/
│   ├── StudentDto.cs
│   ├── CreateStudentDto.cs
│   ├── UpdateStudentDto.cs
│   ├── CourseDto.cs
│   └── EnrollmentDto.cs
├── Mappings/
│   └── MappingProfile.cs
├── Validators/
│   ├── CreateStudentValidator.cs
│   ├── UpdateStudentValidator. cs
│   └── EnrollmentValidator.cs
└── Exceptions/
    ├── NotFoundException.cs
    ├── BusinessRuleException.cs
    └── ValidationException.cs
```

#### Key Implementation: 

**IStudentService.cs**
```csharp
namespace SIS.Application. Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto? > GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task<StudentDto> UpdateAsync(int id, UpdateStudentDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<StudentDto>> SearchAsync(string searchTerm);
    Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync(int studentId);
}
```

**StudentService.cs**
```csharp
namespace SIS.Application. Services;

public class StudentService :  IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        // Create domain entity using factory method
        var email = Email.Create(dto.Email);
        var student = Student.Create(
            dto.StudentNumber,
            dto.FirstName,
            dto.LastName,
            email,
            dto. DateOfBirth,
            DateTime.UtcNow,
            dto.GradeLevel
        );

        // Set optional properties
        if (! string.IsNullOrEmpty(dto.PhoneNumber))
        {
            var phoneNumber = PhoneNumber.Create(dto.PhoneNumber);
            student.UpdatePersonalInfo(dto.FirstName, dto.LastName, phoneNumber);
        }

        // Save to repository
        await _unitOfWork. Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();

        // Map and return DTO
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        return _mapper.Map<StudentDto>(student);
    }

    // ... other methods
}
```

**CreateStudentValidator.cs**
```csharp
namespace SIS.Application. Validators;

public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentValidator()
    {
        RuleFor(x => x.StudentNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(BeAtLeast5YearsOld)
            .WithMessage("Student must be at least 5 years old");

        RuleFor(x => x.GradeLevel)
            .InclusiveBetween(1, 12);
    }

    private bool BeAtLeast5YearsOld(DateTime dateOfBirth)
    {
        return dateOfBirth <= DateTime.UtcNow. AddYears(-5);
    }
}
```

---

### 3. Infrastructure Layer - External Concerns

**Purpose**:  Implements interfaces defined in Application layer.  Handles database, external APIs, file system, etc.

**Dependencies**: Application Layer, Domain Layer

#### What Goes Here: 
- ✅ DbContext
- ✅ Entity configurations
- ✅ Repository implementations
- ✅ Identity implementation
- ✅ External service implementations (email, file storage)
- ✅ Migrations

#### Example Structure:

```
SIS.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── StudentConfiguration.cs
│   │   ├── CourseConfiguration.cs
│   │   └── EnrollmentConfiguration.cs
│   └── Migrations/
├── Repositories/
│   ├── GenericRepository.cs
│   ├── StudentRepository.cs
│   ├── CourseRepository. cs
│   └── UnitOfWork.cs
├── Identity/
│   ├── ApplicationUser.cs
│   └── ApplicationDbContext.Identity.cs
└── Services/
    ├── EmailService.cs
    └── FileStorageService.cs
```

#### Key Implementation:

**StudentConfiguration.cs**
```csharp
namespace SIS.Infrastructure. Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder. ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StudentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(s => s.StudentNumber)
            .IsUnique();

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(50);

        // Value object mapping
        builder. OwnsOne(s => s. Email, email =>
        {
            email. Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.OwnsOne(s => s. PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20);
        });

        builder. OwnsOne(s => s. Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street");
            address. Property(a => a.City).HasColumnName("City");
            address.Property(a => a. Country).HasColumnName("Country");
            address.Property(a => a. PostalCode).HasColumnName("PostalCode");
        });

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasQueryFilter(s => !s.IsDeleted);

        // Relationships
        builder.HasMany(s => s.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior. Restrict);
    }
}
```

**UnitOfWork.cs**
```csharp
namespace SIS. Infrastructure. Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IStudentRepository?  _students;
    private ICourseRepository? _courses;
    private IEnrollmentRepository?  _enrollments;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IStudentRepository Students => 
        _students ??= new StudentRepository(_context);

    public ICourseRepository Courses => 
        _courses ??= new CourseRepository(_context);

    public IEnrollmentRepository Enrollments => 
        _enrollments ??= new EnrollmentRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

---

### 4. Presentation Layer (Web) - User Interface

**Purpose**: Handles HTTP requests, renders views, and presents data to users. 

**Dependencies**: Application Layer, Infrastructure Layer (for DI only)

#### What Goes Here: 
- ✅ Controllers
- ✅ Views
- ✅ ViewModels
- ✅ Filters
- ✅ Middleware
- ✅ Static files (CSS, JS)
- ✅ DI configuration

#### Example Structure:

```
SIS.Web/
├── Controllers/
│   ├── HomeController.cs
│   ├── StudentController.cs
│   ├── CourseController.cs
│   └── EnrollmentController.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Student/
│   │   ├── Index.cshtml
│   │   ├── Details.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   └── ... 
├── ViewModels/
│   ├── StudentViewModel.cs
│   └── CourseViewModel.cs
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
└── Program.cs
```

**StudentController.cs**
```csharp
namespace SIS.Web. Controllers;

public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;

    public StudentController(IStudentService studentService, IMapper mapper)
    {
        _studentService = studentService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllAsync();
        var viewModels = _mapper.Map<IEnumerable<StudentViewModel>>(students);
        return View(viewModels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null)
            return NotFound();

        var viewModel = _mapper.Map<StudentViewModel>(student);
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStudentViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        try
        {
            var dto = _mapper.Map<CreateStudentDto>(viewModel);
            await _studentService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState. AddModelError("", ex.Message);
            return View(viewModel);
        }
    }
}
```

---

## Benefits of This Architecture

1. **Testability**: Each layer can be tested independently
2. **Maintainability**: Clear separation of concerns
3. **Flexibility**: Easy to change database or UI without affecting business logic
4. **Scalability**: Can add new features without modifying existing code
5. **Team Collaboration**: Different teams can work on different layers

---

## Common Mistakes to Avoid

1. ❌ **Putting database code in Domain layer**
2. ❌ **Referencing Infrastructure from Application layer**
3. ❌ **Putting business logic in Controllers**
4. ❌ **Using Entity Framework entities directly in views**
5. ❌ **Skipping DTOs and exposing domain entities**

---

## Quick Reference:  What Goes Where? 

| Concern | Layer |
|---------|-------|
| Student Entity | Domain |
| IStudentService Interface | Application |
| StudentService Implementation | Application |
| IStudentRepository Interface | Application |
| StudentRepository Implementation | Infrastructure |
| DbContext | Infrastructure |
| StudentController | Presentation |
| Student Views | Presentation |
| StudentDto | Application |
| StudentViewModel | Presentation |

---

**Document Version**: 1.0  
**Last Updated**: 2025-12-29
```