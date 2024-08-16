using Microsoft.AspNetCore.Mvc;
using Task_api.Models;
using Task_api.Repositories;
using Task = Task_api.Models.Task;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly TaskRepository _taskRepository;

    public TaskController(IConfiguration configuration)
    {
        _taskRepository = new TaskRepository(configuration.GetConnectionString("DefaultConnection"));
    }

    [HttpGet]
    public IActionResult GetTasks()
    {
        var tasks = _taskRepository.GetTasks();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetTask(int id)
    {
        var task = _taskRepository.GetTask(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] Task_api.Models.Task task)
    {
        _taskRepository.AddTask(task);
        return CreatedAtAction(nameof(GetTask), new { id = task.TaskID }, task);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] Task task)
    {
        if (id != task.TaskID)
        {
            return BadRequest();
        }

        _taskRepository.UpdateTask(task);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        _taskRepository.DeleteTask(id);
        return NoContent();
    }
}

