using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;


        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {

            TodoItem item = _context.TodoItems.Where(e => e.Id == todoId).FirstOrDefault();
            if (item == null) return null;
            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException("You cannot get that item. This is not your item");

            }
            else
            {
                return item;
            }
            
        }
        public void Add(TodoItem todoItem)
        {
            if (_context.TodoItems.Where(e => e.Id == todoItem.Id).FirstOrDefault() == null)
            {
                //TodoItem item = new TodoItem(todoItem.Text, todoItem.UserId);
                _context.TodoItems.Add(todoItem);
                _context.SaveChanges();
                
            }
            else
            {
                throw new DuplicateTodoItemException("Todo item already in database");
            }
             
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(e => e.Id == todoId).FirstOrDefault();

            if (item == null) return false;
            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException("You cannot remove that item. This is not your item.");
            } 
            else if(_context.TodoItems.Remove(item) != null)
            {
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(e => e.Id == todoItem.Id).FirstOrDefault();
            if (item == null)
            {
                Add(todoItem);
            }
            else if (item.UserId == userId)
            {
                Remove(todoItem.Id, userId);
                Add(todoItem);

            }
            else
            {
                throw new TodoAccessDeniedException("You cannot update this item. This is not your item.");
            }
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(e => e.Id == todoId).FirstOrDefault();
            if (item == null) return false;
            if (item.UserId == userId)
            {
                item.DateCompleted = DateTime.Now;
                item.IsCompleted = true;
                Update(item, item.UserId);
                return true;
            }
            else
            {
                throw new TodoAccessDeniedException("You cannot mark it as completed. This is not your item.");
            }

        }

        public List<TodoItem> GetAll(Guid userId)
        {
           return _context.TodoItems.Where(e => e.UserId == userId)
                          .OrderByDescending(e => e.DateCreated)
                          .ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(e => (e.UserId == userId) && e.IsCompleted == false)
                                    .ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(e => (e.UserId == userId) && e.IsCompleted == true)
                           .ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(e => (e.UserId == userId) && (filterFunction(e) == true))
                                    .ToList();
        }

    }
}
