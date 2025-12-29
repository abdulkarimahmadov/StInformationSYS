# Student Information System - Project Documentation

## Table of Contents
1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [System Architecture](#system-architecture)
4. [Project Structure](#project-structure)
5. [Database Design](#database-design)
6. [Implementation Guide](#implementation-guide)
7. [Development Phases](#development-phases)
8. [Security Considerations](#security-considerations)
9. [Testing Strategy](#testing-strategy)
10. [Deployment Guide](#deployment-guide)

---

## 1. Project Overview

### 1.1 Purpose
The Student Information System (SIS) is a web-based application designed to manage student data, courses, enrollments, grades, and academic records for educational institutions. 

### 1.2 Key Features
- **Student Management**: Registration, profile management, academic records
- **Course Management**: Course creation, scheduling, capacity management
- **Enrollment System**: Course registration, waitlist management
- **Grade Management**: Grade entry, transcript generation, GPA calculation
- **User Management**: Role-based access control (Admin, Teacher, Student)
- **Attendance Tracking**: Class attendance recording and reporting
- **Reports & Analytics**: Academic performance reports, enrollment statistics

### 1.3 Goals
- Centralize student information
- Streamline administrative processes
- Improve data accuracy and accessibility
- Enhance communication between students, teachers, and administrators

---

## 2. Technology Stack

### 2.1 Backend
- **Framework**: ASP. NET Core MVC 8.0
- **Language**: C# 12
- **Architecture**: Onion Architecture (Clean Architecture)
- **ORM**: Entity Framework Core 8.0
- **Database**: Microsoft SQL Server 2022

### 2.2 Frontend
- **View Engine**: Razor Pages
- **CSS Framework**: Bootstrap 5.3
- **JavaScript**:  jQuery, DataTables
- **Icons**: Font Awesome

### 2.3 Additional Libraries
- **Authentication**: ASP.NET Core Identity
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI (if needed)

---

## 3. System Architecture

### 3.1 Onion Architecture Overview

The Onion Architecture consists of concentric layers, with dependencies pointing inward.  The core business logic is independent of external concerns.

```
┌─────────────────────────────────────────────┐
│         Presentation Layer (Web UI)         │
│         - Controllers, Views, ViewModels    │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Infrastructure Layer                │
│         - Data Access, External Services    │
│         - Identity, Email, File Storage     │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Application Layer                   │
│         - Business Logic, Services          │
│         - DTOs, Interfaces, Validators      │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Domain Layer (Core)                 │
│         - Entities, Value Objects           │
│         - Domain Events, Interfaces         │
└─────────────────────────────────────────────┘
```

### 3.2 Layer Responsibilities

#### 3.2.1 Domain Layer (Core)
- **Purpose**: Contains enterprise business rules and entities
- **Dependencies**: None (independent)
- **Components**:
  - Domain Entities (Student, Course, Enrollment, Grade, etc.)
  - Value Objects (Address, Email, PhoneNumber, etc.)
  - Domain Interfaces
  - Domain Events
  - Enumerations

#### 3.2.2 Application Layer
- **Purpose**: Contains application business rules
- **Dependencies**: Domain Layer only
- **Components**:
  - Service Interfaces
  - DTOs (Data Transfer Objects)
  - Mapping Profiles
  - Validators
  - Application Exceptions
  - Business Logic Services

#### 3.2.3 Infrastructure Layer
- **Purpose**: Implements external concerns
- **Dependencies**: Application Layer, Domain Layer
- **Components**: 
  - DbContext (Entity Framework)
  - Repository Implementations
  - Identity Implementation
  - Email Services
  - File Storage Services
  - External API Integrations

#### 3.2.4 Presentation Layer (Web UI)
- **Purpose**: User interface and request handling
- **Dependencies**: Application Layer, Infrastructure Layer
- **Components**:
  - Controllers
  - Views (Razor)
  - ViewModels
  - Filters
  - Middleware
  - Dependency Injection Configuration

---

## 4. Project Structure

### 4.1 Solution Structure

```
StudentInformationSystem/
│
├── src/
│   ├── SIS.Domain/                          # Domain Layer
│   │   ├── Entities/
│   │   │   ├── Student.cs
│   │   │   ├── Course. cs
│   │   │   ├── Enrollment.cs
│   │   │   ├── Grade.cs
│   │   │   ├── Teacher.cs
│   │   │   ├── Department.cs
│   │   │   ├── Semester.cs
│   │   │   └── Attendance.cs
│   │   ├── ValueObjects/
│   │   │   ├── Address.cs
│   │   │   ├── Email.cs
│   │   │   └── PhoneNumber.cs
│   │   ├── Enums/
│   │   │   ├── EnrollmentStatus.cs
│   │   │   ├── GradeLevel.cs
│   │   │   └── UserRole.cs
│   │   ├── Interfaces/
│   │   │   └── IEntity.cs
│   │   └── Common/
│   │       └── BaseEntity.cs
│   │
│   ├── SIS.Application/                     # Application Layer
│   │   ├── Interfaces/
│   │   │   ├── IStudentService.cs
│   │   │   ├── ICourseService.cs
│   │   │   ├── IEnrollmentService.cs
│   │   │   ├── IGradeService.cs
│   │   │   └── IUnitOfWork.cs
│   │   ├── Services/
│   │   │   ├── StudentService.cs
│   │   │   ├── CourseService.cs
│   │   │   ├── EnrollmentService.cs
│   │   │   └── GradeService.cs
│   │   ├── DTOs/
│   │   │   ├── StudentDto.cs
│   │   │   ├── CourseDto.cs
│   │   │   ├── EnrollmentDto.cs
│   │   │   └── GradeDto.cs
│   │   ├── Validators/
│   │   │   ├── StudentValidator.cs
│   │   │   ├── CourseValidator.cs
│   │   │   └── EnrollmentValidator.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs
│   │   └── Exceptions/
│   │       ├── BusinessException.cs
│   │       └── NotFoundException.cs
│   │
│   ├── SIS. Infrastructure/                  # Infrastructure Layer
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   ├── StudentConfiguration.cs
│   │   │   │   ├── CourseConfiguration.cs
│   │   │   │   └── EnrollmentConfiguration.cs
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   │   ├── GenericRepository.cs
│   │   │   ├── StudentRepository.cs
│   │   │   ├── CourseRepository. cs
│   │   │   └── UnitOfWork.cs
│   │   ├── Identity/
│   │   │   ├── ApplicationUser.cs
│   │   │   └── IdentityService.cs
│   │   └── Services/
│   │       ├── EmailService.cs
│   │       └── FileStorageService.cs
│   │
│   └── SIS.Web/                             # Presentation Layer
│       ├── Controllers/
│       │   ├── HomeController.cs
│       │   ├── StudentController.cs
│       │   ├── CourseController.cs
│       │   ├── EnrollmentController.cs
│       │   ├── GradeController.cs
│       │   └── AccountController.cs
│       ├── Views/
│       │   ├── Shared/
│       │   │   ├── _Layout.cshtml
│       │   │   └── _ValidationScriptsPartial.cshtml
│       │   ├── Home/
│       │   ├── Student/
│       │   ├── Course/
│       │   ├── Enrollment/
│       │   └── Grade/
│       ├── ViewModels/
│       │   ├── StudentViewModel.cs
│       │   ├── CourseViewModel.cs
│       │   └── EnrollmentViewModel.cs
│       ├── wwwroot/
│       │   ├── css/
│       │   ├── js/
│       │   └── images/
│       ├── Program.cs
│       └── appsettings.json
│
└── tests/
    ├── SIS.Domain.Tests/
    ├── SIS.Application.Tests/
    ├── SIS.Infrastructure.Tests/
    └── SIS.Web.Tests/
```

---

## 5. Database Design

### 5.1 Database Schema

#### 5.1.1 Core Tables

**Students Table**
```sql
CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    StudentNumber NVARCHAR(20) UNIQUE NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber NVARCHAR(20),
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10),
    Address NVARCHAR(200),
    City NVARCHAR(50),
    Country NVARCHAR(50),
    PostalCode NVARCHAR(10),
    EnrollmentDate DATE NOT NULL,
    GradeLevel INT,
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive, Graduated
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

**Courses Table**
```sql
CREATE TABLE Courses (
    CourseId INT PRIMARY KEY IDENTITY(1,1),
    CourseCode NVARCHAR(20) UNIQUE NOT NULL,
    CourseName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Credits INT NOT NULL,
    DepartmentId INT NOT NULL,
    TeacherId INT,
    MaxCapacity INT NOT NULL,
    SemesterId INT NOT NULL,
    Schedule NVARCHAR(100), -- e.g., "Mon/Wed 10:00-11:30"
    Room NVARCHAR(50),
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive, Completed
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId),
    FOREIGN KEY (TeacherId) REFERENCES Teachers(TeacherId),
    FOREIGN KEY (SemesterId) REFERENCES Semesters(SemesterId)
);
```

**Enrollments Table**
```sql
CREATE TABLE Enrollments (
    EnrollmentId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    EnrollmentDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- Enrolled, Dropped, Completed, Waitlisted
    Grade DECIMAL(5,2),
    LetterGrade NVARCHAR(2), -- A+, A, B+, B, etc.
    AttendancePercentage DECIMAL(5,2),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    UNIQUE(StudentId, CourseId)
);
```

**Grades Table**
```sql
CREATE TABLE Grades (
    GradeId INT PRIMARY KEY IDENTITY(1,1),
    EnrollmentId INT NOT NULL,
    AssignmentName NVARCHAR(100) NOT NULL,
    Score DECIMAL(5,2) NOT NULL,
    MaxScore DECIMAL(5,2) NOT NULL,
    Weight DECIMAL(5,2), -- Percentage weight in final grade
    GradeDate DATE NOT NULL,
    Comments NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EnrollmentId) REFERENCES Enrollments(EnrollmentId)
);
```

**Teachers Table**
```sql
CREATE TABLE Teachers (
    TeacherId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeNumber NVARCHAR(20) UNIQUE NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber NVARCHAR(20),
    DepartmentId INT NOT NULL,
    HireDate DATE NOT NULL,
    Specialization NVARCHAR(100),
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive, Retired
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);
```

**Departments Table**
```sql
CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    DepartmentCode NVARCHAR(10) UNIQUE NOT NULL,
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    HeadOfDepartment INT,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0
);
```

**Semesters Table**
```sql
CREATE TABLE Semesters (
    SemesterId INT PRIMARY KEY IDENTITY(1,1),
    SemesterName NVARCHAR(50) NOT NULL, -- e.g., "Fall 2024"
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsCurrentSemester BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
```

**Attendance Table**
```sql
CREATE TABLE Attendance (
    AttendanceId INT PRIMARY KEY IDENTITY(1,1),
    EnrollmentId INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- Present, Absent, Late, Excused
    Notes NVARCHAR(200),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EnrollmentId) REFERENCES Enrollments(EnrollmentId)
);
```

### 5.2 Entity Relationships

```
Students 1---N Enrollments N---1 Courses
Courses N---1 Teachers
Courses N---1 Departments
Teachers N---1 Departments
Courses N---1 Semesters
Enrollments 1---N Grades
Enrollments 1---N Attendance
```

---

## 6. Implementation Guide

### 6.1 Step 1: Create Solution Structure

```bash
# Create solution
dotnet new sln -n StudentInformationSystem

# Create Domain Layer (Class Library)
dotnet new classlib -n SIS.Domain -o src/SIS.Domain
dotnet sln add src/SIS.Domain

# Create Application Layer (Class Library)
dotnet new classlib -n SIS.Application -o src/SIS.Application
dotnet sln add src/SIS.Application

# Create Infrastructure Layer (Class Library)
dotnet new classlib -n SIS.Infrastructure -o src/SIS.Infrastructure
dotnet sln add src/SIS.Infrastructure

# Create Presentation Layer (MVC Web App)
dotnet new mvc -n SIS.Web -o src/SIS.Web
dotnet sln add src/SIS.Web

# Add project references
cd src/SIS.Application
dotnet add reference ../SIS.Domain

cd ../SIS.Infrastructure
dotnet add reference ../SIS.Domain
dotnet add reference ../SIS.Application

cd ../SIS.Web
dotnet add reference ../SIS.Application
dotnet add reference ../SIS.Infrastructure
```

### 6.2 Step 2: Install Required NuGet Packages

**SIS.Domain** (no external dependencies)

**SIS.Application**
```bash
cd src/SIS.Application
dotnet add package AutoMapper
dotnet add package FluentValidation
```

**SIS.Infrastructure**
```bash
cd src/SIS.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

**SIS.Web**
```bash
cd src/SIS.Web
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Serilog.AspNetCore
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### 6.3 Step 3:  Implement Domain Layer

#### 6.3.1 Base Entity

```csharp
// src/SIS.Domain/Common/BaseEntity.cs
namespace SIS.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
```

#### 6.3.2 Student Entity

```csharp
// src/SIS.Domain/Entities/Student.cs
namespace SIS.Domain.Entities;

public class Student : BaseEntity
{
    public string StudentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string. Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string?  PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string?  Gender { get; set; }
    public string?  Address { get; set; }
    public string? City { get; set; }
    public string?  Country { get; set; }
    public string? PostalCode { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int GradeLevel { get; set; }
    public string Status { get; set; } = "Active";

    // Navigation properties
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public string FullName => $"{FirstName} {LastName}";
}
```

#### 6.3.3 Course Entity

```csharp
// src/SIS. Domain/Entities/Course.cs
namespace SIS.Domain. Entities;

public class Course : BaseEntity
{
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string?  Description { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
    public int?  TeacherId { get; set; }
    public int MaxCapacity { get; set; }
    public int SemesterId { get; set; }
    public string?  Schedule { get; set; }
    public string? Room { get; set; }
    public string Status { get; set; } = "Active";

    // Navigation properties
    public Department Department { get; set; } = null!;
    public Teacher?  Teacher { get; set; }
    public Semester Semester { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
```

#### 6.3.4 Enrollment Entity

```csharp
// src/SIS.Domain/Entities/Enrollment.cs
namespace SIS.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string Status { get; set; } = "Enrolled";
    public decimal?  Grade { get; set; }
    public string? LetterGrade { get; set; }
    public decimal? AttendancePercentage { get; set; }

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();
}
```

### 6.4 Step 4: Implement Application Layer

#### 6.4.1 Student Service Interface

```csharp
// src/SIS.Application/Interfaces/IStudentService.cs
namespace SIS.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto? > GetStudentByIdAsync(int id);
    Task<StudentDto> CreateStudentAsync(StudentDto studentDto);
    Task<StudentDto> UpdateStudentAsync(int id, StudentDto studentDto);
    Task<bool> DeleteStudentAsync(int id);
    Task<IEnumerable<StudentDto>> SearchStudentsAsync(string searchTerm);
}
```

#### 6.4.2 Student DTO

```csharp
// src/SIS.Application/DTOs/StudentDto.cs
namespace SIS.Application.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int GradeLevel { get; set; }
    public string Status { get; set; } = "Active";
    public string FullName => $"{FirstName} {LastName}";
}
```

### 6.5 Step 5: Implement Infrastructure Layer

#### 6.5.1 DbContext

```csharp
// src/SIS.Infrastructure/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using SIS.Domain.Entities;

namespace SIS.Infrastructure. Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base. SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                   (e.State == EntityState.Added || e.State == EntityState. Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;
            
            if (entityEntry.State == EntityState.Added)
            {
                entity. CreatedAt = DateTime.UtcNow;
            }
            
            entity.UpdatedAt = DateTime. UtcNow;
        }
    }
}
```

#### 6.5.2 Generic Repository

```csharp
// src/SIS.Infrastructure/Repositories/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using SIS.Domain.Common;
using SIS.Infrastructure.Data;
using System.Linq.Expressions;

namespace SIS.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context. Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e. Id == id && !e.IsDeleted);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet. AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).Where(e => !e.IsDeleted).ToListAsync();
    }
}
```

### 6.6 Step 6: Configure Dependency Injection

```csharp
// src/SIS.Web/Program.cs
using Microsoft.EntityFrameworkCore;
using SIS.Infrastructure.Data;
using SIS.Application.Interfaces;
using SIS.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

## 7. Development Phases

### Phase 1: Project Setup (Week 1)
- [ ] Create solution structure with all layers
- [ ] Install required NuGet packages
- [ ] Set up database connection
- [ ] Configure dependency injection
- [ ] Create base entities and interfaces

### Phase 2: Domain & Database (Week 2)
- [ ] Define all domain entities
- [ ] Create entity configurations
- [ ] Design database schema
- [ ] Create and run initial migration
- [ ] Seed initial data (departments, semesters)

### Phase 3: Core Features - Students (Week 3)
- [ ] Implement student repository
- [ ] Implement student service
- [ ] Create student controller and views
- [ ] Add student CRUD operations
- [ ] Implement student search functionality

### Phase 4: Core Features - Courses (Week 4)
- [ ] Implement course repository
- [ ] Implement course service
- [ ] Create course controller and views
- [ ] Add course CRUD operations
- [ ] Implement course capacity management

### Phase 5: Enrollment System (Week 5-6)
- [ ] Implement enrollment repository
- [ ] Implement enrollment service with business rules
- [ ] Create enrollment controller and views
- [ ] Add course registration functionality
- [ ] Implement waitlist management
- [ ] Add enrollment validation

### Phase 6: Grades & Attendance (Week 7)
- [ ] Implement grade repository and service
- [ ] Create grade entry interface
- [ ] Implement GPA calculation
- [ ] Implement attendance tracking
- [ ] Create transcript generation

### Phase 7: Authentication & Authorization (Week 8)
- [ ] Configure ASP.NET Core Identity
- [ ] Implement role-based access control
- [ ] Create login/register pages
- [ ] Add user management
- [ ] Implement authorization policies

### Phase 8: Reports & Analytics (Week 9)
- [ ] Create academic performance reports
- [ ] Implement enrollment statistics
- [ ] Add data export functionality (PDF, Excel)
- [ ] Create dashboard with charts

### Phase 9: Testing & Refinement (Week 10)
- [ ] Write unit tests
- [ ] Write integration tests
- [ ] Perform user acceptance testing
- [ ] Fix bugs and optimize performance

### Phase 10: Deployment (Week 11-12)
- [ ] Prepare production database
- [ ] Configure production settings
- [ ] Deploy to hosting environment
- [ ] Create user documentation
- [ ] Train end users

---

## 8. Security Considerations

### 8.1 Authentication
- Use ASP.NET Core Identity for user management
- Implement password policies (minimum length, complexity)
- Enable two-factor authentication (optional)
- Implement account lockout after failed login attempts

### 8.2 Authorization
- Role-based access control (Admin, Teacher, Student)
- Implement authorization policies
- Restrict sensitive operations to authorized users
- Use `[Authorize]` attributes on controllers/actions

### 8.3 Data Protection
- Encrypt sensitive data in database
- Use HTTPS for all communications
- Implement CSRF protection (automatic in ASP.NET Core)
- Validate all user inputs
- Sanitize data to prevent XSS attacks
- Use parameterized queries (Entity Framework handles this)

### 8.4 Privacy
- Implement data retention policies
- Allow students to view/export their data
- Restrict access to personal information
- Log access to sensitive data

---

## 9. Testing Strategy

### 9.1 Unit Tests
- Test domain entities business logic
- Test service layer methods
- Test validators
- Mock dependencies using Moq

### 9.2 Integration Tests
- Test database operations
- Test API endpoints
- Test authentication/authorization

### 9.3 UI Tests
- Test critical user workflows
- Test form validations
- Test responsive design

---

## 10. Deployment Guide

### 10.1 Prerequisites
- Windows Server or Linux server
- IIS (Windows) or Nginx/Apache (Linux)
- SQL Server 2019 or later
- . NET 8 Runtime

### 10.2 Deployment Steps

1. **Publish Application**
```bash
dotnet publish src/SIS.Web -c Release -o publish
```

2. **Configure Database**
- Create production database
- Update connection string in `appsettings.Production.json`
- Run migrations: 
```bash
dotnet ef database update --project src/SIS.Infrastructure --startup-project src/SIS. Web
```

3. **Configure IIS (Windows)**
- Install ASP.NET Core Runtime
- Create new website in IIS
- Point to published folder
- Configure application pool (. NET CLR Version:  No Managed Code)

4. **Configure Nginx (Linux)**
- Install Nginx and configure reverse proxy
- Configure systemd service for the application
- Enable and start the service

### 10.3 Post-Deployment
- Test all critical functionalities
- Monitor application logs
- Set up regular database backups
- Configure SSL certificate

---

## Appendix

### A. Connection String Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection":  "Server=localhost;Database=StudentInformationSystem;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### B. Useful Commands

```bash
# Create migration
dotnet ef migrations add InitialCreate --project src/SIS.Infrastructure --startup-project src/SIS. Web

# Update database
dotnet ef database update --project src/SIS.Infrastructure --startup-project src/SIS. Web

# Remove last migration
dotnet ef migrations remove --project src/SIS.Infrastructure --startup-project src/SIS. Web

# Build solution
dotnet build

# Run application
dotnet run --project src/SIS.Web
```

### C. Recommended VS Code Extensions
- C# Dev Kit
- NuGet Gallery
- REST Client
- SQL Server (mssql)
- GitLens

### D. Additional Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

**Document Version**:  1.0  
**Last Updated**: 2025-12-29  
**Project Lead**: @abdulkarimahmadov
```