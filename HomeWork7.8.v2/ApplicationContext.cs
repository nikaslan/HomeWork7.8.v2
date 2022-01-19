using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HomeWork7._8.v2
{
    class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public ApplicationContext() : base("DefaultConnection") { }
    }
}
