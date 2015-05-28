using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public  class ProfessorContext :DbContext
    { 
        public DbSet< ModelEntityFrm.ProfessorClass> professors{get;set;}
        public DbSet<ModelEntityFrm.StudentClass> students { get; set; }
        public DbSet<ModelEntityFrm.InformationClass> users { get; set; }
        public DbSet<ModelEntityFrm.State> states { get; set; }

    }
}
