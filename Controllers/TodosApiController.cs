using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetCoreSqlDb.Controllers;

[ApiController]
public class TodosApiController : ControllerBase
{
    private readonly MyDatabaseContext _context;

    public TodosApiController(MyDatabaseContext context)
    {
        _context = context;
    }

    // GET: api/todos
    [HttpGet("api/todos")]
    [SwaggerOperation(
        Summary = "Gets all Todo items",
        OperationId = "GetTodos")]
    [ProducesResponseType(
        typeof(IEnumerable<Todo>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        var todos = await _context.Todo
            .OrderBy(todo => todo.ID)
            .ToListAsync();

        return Ok(todos);
    }

    // GET: api/todos/5
    [HttpGet("api/todos/{id:int}")]
    [SwaggerOperation(
        Summary = "Gets a Todo item by ID",
        OperationId = "GetTodoById")]
    [ProducesResponseType(
        typeof(Todo),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Todo>> GetTodoById(int id)
    {
        var todo = await _context.Todo.FindAsync(id);

        if (todo == null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    // POST: api/todos
    [HttpPost("api/todos")]
    [SwaggerOperation(
        Summary = "Creates a new Todo item",
        Description = "Creates a new Todo item and returns it.",
        OperationId = "CreateTodo")]
    [ProducesResponseType(
        typeof(Todo),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Todo>> CreateTodo(
        [FromBody] Todo todo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Todo.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoById),
            new { id = todo.ID },
            todo);
    }

    // PUT: api/todos/5
    [HttpPut("api/todos/{id:int}")]
    [SwaggerOperation(
        Summary = "Updates a Todo item",
        Description = "Updates an existing Todo item by ID.",
        OperationId = "UpdateTodo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTodo(
        int id,
        [FromBody] Todo todo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingTodo = await _context.Todo.FindAsync(id);

        if (existingTodo == null)
        {
            return NotFound();
        }

        existingTodo.Description = todo.Description;
        existingTodo.CreatedDate = todo.CreatedDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/todos/5
    [HttpDelete("api/todos/{id:int}")]
    [SwaggerOperation(
        Summary = "Deletes a Todo item",
        Description = "Deletes a Todo item by ID.",
        OperationId = "DeleteTodo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.Todo.FindAsync(id);

        if (todo == null)
        {
            return NotFound();
        }

        _context.Todo.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
