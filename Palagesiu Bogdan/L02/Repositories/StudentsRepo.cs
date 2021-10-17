using System.Collections.Generic;
using Models;
namespace Repositories{
    public static class StudentsRepo{

        public static List<Students> Students = new List<Students>(){
            new Students(){Id=1, Faculty="AC", Name = "Cristi", Year = 4},
            new Students(){Id=2, Faculty="Sport", Name = "Florin", Year = 2}
        };
    }
}