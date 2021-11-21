using System.Dynamic;
using System.Net.Http;
using System.Collections;
using System;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

namespace L06.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        private IStudentRepository _studentRepository;
        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            return await _studentRepository.GetAllStudents();
        }
        [HttpGet("{university}/{cnp}")] // Search
        public async Task<IActionResult> Find(string university, string cnp)
        {
            var status = await _studentRepository.FindStudent(university, cnp);
            if (status == null)
                return NotFound();
            return Ok(status);

        }
        [HttpPost]
        public async Task Post([FromBody] StudentEntity student)
        {
            await _studentRepository.CreateStudent(student);
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] StudentEntity student)
        {
            return await _studentRepository.EditStudent(student).Match(
                studentEntity => Ok(student),
                error => (IActionResult)BadRequest());
        }
        [HttpDelete("{university}/{cnp}")]
        public async Task<IActionResult> Delete(string university, string cnp)
        {
            return await _studentRepository.DeleteStudent(university, cnp).Match(
                unit => Ok(),
                error => (IActionResult)BadRequest());
        }
    }
}