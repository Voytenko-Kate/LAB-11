using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DocumentFormat.OpenXml.EMMA;
using System.Runtime.InteropServices;

namespace WpfDatabase
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var db = (Application.Current as App).db;
            db.Students.Load();
            db.Groups.Load();

            foreach(Model.Student s in db.Students.Local)
            {
                if (s.Group == null || s.GroupId == null)
                    if(db.Groups.Local.Count > 0)
                        s.Group = db.Groups.Local[0];
            }

            var studentViewSource = ((CollectionViewSource)(this.FindResource("studentViewSource")));
            studentViewSource.Source = db.Students.Local;

            var groupViewSource = ((CollectionViewSource)(this.FindResource("groupViewSource")));
            groupViewSource.Source = db.Groups.Local;

        }

        private void StudentsFilter(object sender, FilterEventArgs e)
        {
            var student = e.Item as Model.Student;
            var group = groupSelectList.SelectedItem as Model.Group;
            if (group != null)
                e.Accepted = student.GroupId == group.Id;
            else
                e.Accepted = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var db = (Application.Current as App).db;
            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var db = (Application.Current as App).db;
            if(db.Groups.Local.Count < 1)
            {
                MessageBox.Show("Please, add some groups", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var s = new Model.Student
            {
                Name = "Student",
                Admission = new DateTime(2020, 1, 9),
                Group = groupSelectList.SelectedItem as Model.Group,
                GroupId = (groupSelectList.SelectedItem as Model.Group).Id
            };
            db.Students.Add(s);
            (this.Resources["studentViewSource"] as CollectionViewSource).View.Refresh();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var db = (Application.Current as App).db;
            Model.Student student = listBox.SelectedItem as Model.Student;
            if(student != null)
                db.Students.Remove(student);
        }

        private void GroupAddButtonClick(object sender, RoutedEventArgs e)
        {
            var db = (Application.Current as App).db;
            Model.Group g = new Model.Group
            {
                Number = 1,
                Course = 1,
                Track = "POKS"
            };
            db.Groups.Add(g);
            (this.Resources["groupViewSource"] as CollectionViewSource).View.Refresh();
        }

        private void RemoveGroupButton_Click(object sender, RoutedEventArgs e)
        {
            var db = (Application.Current as App).db;

            Model.Group group = groupsListBox.SelectedItem as Model.Group;
            if (group != null)
            {
                var studentViewSource = ((CollectionViewSource)(this.FindResource("studentViewSource")));
                var studentsCollection = studentViewSource.Source as ObservableCollection<Model.Student>;

                foreach (Model.Student s in studentsCollection.ToArray<Model.Student>())
                    if (s.GroupId == group.Id)
                        db.Students.Remove(s);
                db.Groups.Remove(group);
                (this.Resources["groupViewSource"] as CollectionViewSource).View.Refresh();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var studentViewSource = ((CollectionViewSource)(this.FindResource("studentViewSource")));
            studentViewSource.Filter -= StudentsFilter;
            studentViewSource.Filter += StudentsFilter;
        }

        private void GroupFocusLost(object sender, EventArgs e)
        {
            (this.Resources["groupViewSource"] as CollectionViewSource).View.Refresh();
        }

        private void StudentFocusLost(object sender, EventArgs e)
        {
            (this.Resources["studentViewSource"] as CollectionViewSource).View.Refresh();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var studentViewSource = ((CollectionViewSource)(this.FindResource("studentViewSource")));
            var studentsCollection = studentViewSource.Source as ObservableCollection<Model.Student>;

            var currentGroup = groupSelectList.SelectedItem as Model.Group;

            var data = from student in studentsCollection
                       where student.Group == currentGroup
                       select student;

            var export = new XLSExportProvider(data);

            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() == true)
            {
                export.ExportTo(saveDialog.FileName);
            }
        }
    }
}
