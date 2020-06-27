using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

namespace WpfDatabase.Model
{

    public class Group
    {
        public Group()
        {
            this.Id = Guid.NewGuid().GetHashCode();
            Students = new HashSet<Student>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int Number { get; set; }
        public int Course { get; set; }
        public string Track { get; set; }
        
        public virtual ICollection<Student> Students { get; set; }

        public override string ToString()
        {
            return $"{Track}-{Course}{Number}";
        }

        public string Name
        {
            get => this.ToString();
        }

        public static object AllGroups
        {
            get
            {
                var db = (Application.Current as App).db;
                return db.Groups.Local;
            }
        }
    }

    public class Student
    {
        public Student()
        {
            this.Id = Guid.NewGuid().GetHashCode();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int? GroupId { get; set; }

        public DateTime Admission { get; set; }

        public virtual Group Group { get; set; }        
    }

    public class EFContext: DbContext
    {
        public EFContext(string connection) : base(connection)
        { }

        public  DbSet<Group> Groups { get; set; }
        public  DbSet<Student> Students { get; set; }        
    }
}
