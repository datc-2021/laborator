using L05.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace L05
{
    public interface IStudentsRepository
    {
        Task<List<StudentEntity>> GetAllStudents();
    }
}
