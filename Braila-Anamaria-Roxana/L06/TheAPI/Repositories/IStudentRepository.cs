using System.Runtime.CompilerServices;
using System.Dynamic;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

public interface IStudentRepository
{
    Task<List<StudentEntity>> GetAllStudents();
    Task<StudentEntity> FindStudent(string university, string cnp);
    Task CreateStudent(StudentEntity student);
    TryAsync<StudentEntity> EditStudent(StudentEntity student);
    TryAsync<Unit> DeleteStudent(string university, string cnp);
}