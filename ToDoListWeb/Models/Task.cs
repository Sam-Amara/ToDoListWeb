using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoListWeb.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool isComplete { get; set; }
    }
}