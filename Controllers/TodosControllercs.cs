using Microsoft.AspNetCore.Mvc;

namespace ASP_study.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private List<TodoItem> _todos = new List<TodoItem>();
        public TodosController()
        {                                                           //test data
            _todos.AddRange(new List<TodoItem>() {
            new TodoItem { Id = Guid.NewGuid(), Title ="homeWork", IsCompleted = false },
            new TodoItem{Id = Guid.NewGuid(), Title = "Gym", IsCompleted = true},
            new TodoItem{Id = Guid.NewGuid(), Title = "Clean room", IsCompleted = false} });
        }

        [HttpGet]
        public  IActionResult GetAll() => Ok(_todos);

        [HttpGet("{index:int}")]
        public IActionResult Get(int index)
        {
            if (index < 0 || index >= _todos.Count) return BadRequest("Invalid index");
            return Ok(_todos[index]);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            try
            {
                _todos.Add(item); return CreatedAtAction(nameof(Get), new { index = _todos.Count - 1 }, item);
            }
            catch 
            {
                return BadRequest("invalid data");
            }
        }

    }
}
