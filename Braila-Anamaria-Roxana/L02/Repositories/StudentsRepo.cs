using Models;
using System.Collections.Generic;

namespace Repositories
{
    public static class StudentsRepo
    {
        public static List<Student> Students = new List<Student>()
        {
            new Student(){Id=1, Name="Clarkson", Faculty="AC", Year=3},
            new Student(){Id=2, Name="Thompson", Faculty="MTP", Year=4},
            new Student(){Id=3, Name="Johnson", Faculty="EE", Year=3},
            new Student(){Id=4, Name="Bieber", Faculty="AC", Year=1}
        };
    }
}