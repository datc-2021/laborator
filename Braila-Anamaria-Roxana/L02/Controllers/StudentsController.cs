using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Repositories;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet] //READ operation
        public IEnumerable<Student> Get()
        {
            return StudentsRepo.Students.OrderBy(x => x.Id);
        }
        [HttpGet("{id}")] //READ operation
        public IActionResult Find(int id)
        {
            var studentsList = StudentsRepo.Students;
            Student student = studentsList.Find(x => x.Id.Equals(id));
            if (student == null)
                return NotFound();
            return Ok(student);
        }
        [HttpDelete("{id}")] //DELETE operation
        public IActionResult Delete(int id)
        {
            var studentsList = StudentsRepo.Students;
            Student student = studentsList.Find(x => x.Id.Equals(id));
            if (student == null)
                return NotFound();
            return Ok(studentsList.Remove(student));
        }
        [HttpPut] //UPDATE operation
        public IActionResult Edit([FromBody] Student student)
        {
            var studentsList = StudentsRepo.Students;
            Student status = studentsList.Find(x => x.Id.Equals(student.Id));
            if (status != null)
            {
                studentsList.RemoveAt(student.Id - 1);
                studentsList.Add(student);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost] //CREATE operation
        public IActionResult Add([FromBody] Student student)
        {
            var studentsList = StudentsRepo.Students;
            Student status = studentsList.Find(x => x.Id.Equals(student.Id));
            if (status == null)
            {
                studentsList.Add(student);
                return Ok();
            }
            return BadRequest();
        }
    }
}
