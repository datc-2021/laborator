using Microsoft.AspNetCore.Mvc;

namespace L04.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly StudentsService _studentService;

    public StudentsController(StudentsService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _studentService.GetAll());

    [HttpGet("{university}/{id}")]
    public async Task<IActionResult> Get([FromRoute] string university, string id)
        => await _studentService.Get(university.Trim(), id.Trim()).Match(
            student => Ok(student),
            exception => (IActionResult)NotFound());

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] StudentEntity student)
        => (await _studentService.Add(student)).GetRawResponse().Status switch
        {
            201 => Ok(),
            _ => BadRequest()
        };

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] StudentEntity student)
        => await _studentService.Edit(student).Match(
            unit => Ok(),
            exception => (IActionResult)BadRequest());

    [HttpDelete("{university}/{id}")]
    public async Task<IActionResult> Delete([FromRoute]string university, string id)
        => await _studentService.Delete(university.Trim(), id.Trim()).Match(
            unit => Ok(),
            exception => (IActionResult)BadRequest());
}
