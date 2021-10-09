using L02.Models;
using LanguageExt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L02.Services
{
    public class StudentRepo
    {
        private IEnumerable<Student> students;

        public StudentRepo()
        {
            students = Enumerable.Empty<Student>();
        }

        public Option<Student> Get(Func<Student, bool> predicate)
            => students.FirstOrDefault(predicate);

        public IEnumerable<Student> Get()
            => students.OrderBy(a => a.Id);

        public bool Add(Student student)
        {
            var exists = students.Any(a => a.Id == student.Id);
            if (exists)
                return false;
            students = students.Append(student);
            return true;
        }

        public Unit Update(Student student)
        {
            students = students.Where(a => a.Id != student.Id).Append(student);
            return Unit.Default;
        }

        public Unit Delete(Func<Student, bool> predicate)
        {
            students = students.Where(a => !predicate.Invoke(a));
            return Unit.Default;
        }
    }
}
