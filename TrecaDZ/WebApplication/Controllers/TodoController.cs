using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApplication.Models.AddTodoViewModels;
using ClassLibrary;
using WebApplication.Models.TodoItemsViewModels;

namespace WebApplication.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<Models.ApplicationUser> _userManager;


        // Inject user manager into constructor
        public TodoController(ITodoRepository repository,
        UserManager<Models.ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
          
     
        }

        public async Task<IActionResult> Index()
        {
            Models.ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            var todos = _repository.GetActive(userId);

            return View(todos);
        }

        public IActionResult Add()
        {          
            return View("AddTodo");
        }
        
        public async Task<IActionResult> Completed()
        {
            Models.ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            var todos = _repository.GetCompleted(userId);

            return View("Completed", todos);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(AddTodoViewModel source)
        {
            if (ModelState.IsValid)
            {
                string todoMessage = source.TodoInput;
                Models.ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid g = new Guid(currentUser.Id);
                ClassLibrary.TodoItem item = new ClassLibrary.TodoItem(todoMessage, g);

                _repository.Add(item);
                return RedirectToAction("Index");
            }
                return View(source);
            
        }

        
        public async Task<IActionResult> Remove()
        {
            Models.ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            _repository.Remove(new Guid("a4e33bc0-a393-4dc5-9bf7-0a17e05a260e"), userId);
            _repository.Remove(new Guid("9265ecb4-6180-4353-ac31-28d0e714f747"), userId);
            _repository.Remove(new Guid("a6e9bfa8-dcc0-48c8-8501-5be77c93b380"), userId);
            _repository.Remove(new Guid("83c71c6d-a0fa-4a8c-b761-6b75c1ceb16d"), userId);
            _repository.Remove(new Guid("5fb8d389-edaa-4cbe-b72b-aaa762d7f603"), userId);
            _repository.Remove(new Guid("9a86083e-5676-4438-b711-d2e985ff9f06"), userId);
            _repository.Remove(new Guid("e5d1c622-b788-4a65-8c90-1d962ad478ae"), userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            Models.ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            _repository.MarkAsCompleted(todoId, userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            Models.ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            List<TodoItem> activeTodos = _repository.GetActive(userId);
            TodoItems.activeTodos = activeTodos;
            return View(activeTodos);
        }

    }
}