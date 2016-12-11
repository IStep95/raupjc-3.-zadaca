using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.AddTodoViewModels
{
    public class AddTodoViewModel
    {

        [Required]
        public string TodoInput { get; set; }

    }
}
