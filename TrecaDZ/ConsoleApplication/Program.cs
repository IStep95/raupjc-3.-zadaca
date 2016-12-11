using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        private const string ConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=TodoDbContext;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static void Main(string[] args)
        {
            Demo();
        }

        private static void Demo()
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                var user1 = Guid.NewGuid();
                var user2 = Guid.NewGuid();
                TodoItem item = new TodoItem("Obavi konzum", user1);
                db.TodoItem.Add(item);
                db.SaveChanges();

                TodoSqlRepository repository = new TodoSqlRepository(db);
                repository.MarkAsCompleted(item.Id, user1);
                repository.Get(item.Id, user1);
                
            }
        }
    }
}
