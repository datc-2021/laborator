using System.Collections.Generic;
using Models;


namespace Repositories
{
    public class StudentsRepo
    {
        public static List<Student> Students = new List<Student>{
            new Student() {Id = 1, Nume = "Popescu", Prenume = "Ion", Facultate = "AC", An = 2},
            new Student() {Id = 2, Nume = "Ionescu", Prenume = "Alin", Facultate = "ETC", An = 3},
            new Student() {Id = 3, Nume = "Basescu", Prenume = "Traian", Facultate = "Vietii", An = 5}
        };
    }

}