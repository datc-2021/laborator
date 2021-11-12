using L02.Models;
using L02.Services;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentRepo repo;

        public StudentsController(StudentRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(repo.Get());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            [FromRoute] int id)
            => repo.Get(a => a.Id == a.Id).Case switch
            {
                SomeCase<Student> s => Ok(s.Value),
                NoneCase<Student> _ => NotFound()
            };

        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] Student student)
            => repo.Add(student) switch
            {
                true => Ok(),
                false => BadRequest($"Student with id:{{{student.Id}}} already exists.")
            };

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] Student student)
            => Ok(repo.Update(student));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int id)
            => Ok(repo.Delete(a => a.Id == id));
    }
}
