using System.Collections.Generic;
using Models;
namespace Repositories{
    public static class StudentsRepo{

        public static List<Students> Students = new List<Students>(){
            new Students(){Id=1, Faculty="AC", Name = "Bogdan", Year = 4},
            new Students(){Id=2, Faculty="ETc", Name = "Alexa", Year = 1}
        };
    }
}