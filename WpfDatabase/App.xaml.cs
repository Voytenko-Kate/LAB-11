using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDatabase
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>    
    public partial class App : Application
    {

        public Model.EFContext db;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            db = new Model.EFContext("Database2");
            //var s = new Model.Student
            //{
            //    Id = 1,
            //    Name = "Student2",
            //    Admission = new DateTime(2020, 9, 1),
            //    Group = db.Groups.Find(0)
            //};
            //db.Students.Add(s);
            //db.SaveChanges();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Debug.WriteLine($"ExitCode ={e.ApplicationExitCode}");
            db.SaveChanges();
            base.OnExit(e);
        }
    }
}
