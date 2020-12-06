using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using FtpApp.Models.Utilities;
using FTPApp.Models;



namespace FTPApp
{
    class Program
    {
        private static string directory;

        public static object JSONConvert { get; private set; }
        public static object Response { get; private set; }

        static void Main(string[] args)
        {
            //byte[] filetobytes = FTP.GetStreamBytes("C:\\Users\\MY PC\\Desktop\\myimage.jpg");

            List<string> errors = new List<string>();
            List<string> directories = FTP.GetDirectory(Constants.FTP.BaseUrl);
            List<Student> studnets = new List<Student>();
            Student std = new Student();
            string imagesOutputFolder = @" C:\Users\MY PC\Desktop\Images\";

            foreach (var directory in directories)
            {
                Console.WriteLine(directory);
            }

            Student student = new Student();

            //student.FromDirectory(directory);

            //studnets.Add(student);
            //string[] directoryPart = directory.Split(" ", StringSplitOptions.None);
                
            try
            {
                Console.WriteLine(Constants.FTP.BaseUrl + "/" + directory + "/");

                //Path to the remote file you will download
                string remoteDownloadFilePath = "/200464602 Dixit Ohri/info.csv";
                //Path to a valid folder and the new file to be saved
                string localDownloadFileDestination = @"C:\Users\MY PC\Desktop\Encoding\info2.csv";
                //Path to download myimage file 
                string DownloadFilePath = "/200464602 Dixit Ohri/myimage.jpg";
                //Path to a valid folder and the new file to be saved
      
                string DownloadFileDestination = @"C:\Users\MY PC\Desktop\Encoding\myimage.jpg";

                Console.WriteLine(FTP.DownloadFile(Constants.FTP.BaseUrl + remoteDownloadFilePath, localDownloadFileDestination));

                Console.WriteLine(FTP.DownloadFile(Constants.FTP.BaseUrl + DownloadFilePath, DownloadFileDestination));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //student class
            if (FTP.FileExists(Constants.FTP.BaseUrl + "/200464602 Dixit Ohri/info.csv"))
            {
                Console.WriteLine("   info.csv exist  ");
            }
            else
            {
                errors.Add(directory + "  info.csv does not exist ");
                Console.WriteLine("   info.csv doest not exist ");
            }

            if (FTP.FileExists(Constants.FTP.BaseUrl + "/200464602 Dixit Ohri/info.html"))
            {
                Console.WriteLine("   info.html exist  ");
            }
            else
            {
                errors.Add(directory + "  info.html does not exist ");
                Console.WriteLine("   info.html doest not exist ");
            }
            if (FTP.FileExists(Constants.FTP.BaseUrl + "/200464602 Dixit Ohri/myimage.jpg"))
            {
                Console.WriteLine("   myimage.jpg exist  ");
                string downloadFileResult = FTP.DownloadFile(Constants.FTP.BaseUrl + "/200464602 Dixit Ohri/myimage.jpg  ", imagesOutputFolder + "myimage.jpg");
                Console.WriteLine(downloadFileResult);
            }
            else
            {
                errors.Add(directory + "   myimage.jpg does not exist ");
                Console.WriteLine("   myimage.jpg doest not exist ");
            }


            if (errors.Count > 0)
            {
                Console.WriteLine($"Found {errors.Count} errors");
                foreach (var er in errors)
                {
                    Console.WriteLine(er);
                }
            }
            if (FTP.FileExists(Constants.FTP.BaseUrl + "/200464602 Dixit Ohri/" + "info.csv"))
            {
                var fileBytes = FTP.DownloadFileBytes(Constants.FTP.BaseUrl + "/200464602 Dixit Ohri/" + "info.csv");
               
                string infoCsvData = Encoding.UTF8.GetString(fileBytes, 0, fileBytes.Length);
                string[] lines = infoCsvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                
                Student studenta = new Student();
                studenta.FromCSV(lines[1]);
                string dataToString = studenta.ToString();
                Console.WriteLine(dataToString);
                string csvData = studenta.ToCSV();
                Console.WriteLine(csvData);
            }
            else
            {
                Console.WriteLine("File not found");
            }
            List<Student> students = new List<Student>();
            
            List<string> alldirectories = FTP.GetDirectory(Constants.FTP.BaseUrl);
            foreach (var directory in alldirectories)
            {
                Console.WriteLine(directory);
                Student st = new Student();
                if(FTP.FileExists(Constants.FTP.BaseUrl + "/" + directory + "/info.csv"))
                {
                    var fileBytes = FTP.DownloadFileBytes(Constants.FTP.BaseUrl + "/" + directory + "/info.csv");
                    string CsvData = Encoding.UTF8.GetString(fileBytes, 0, fileBytes.Length);
                    string[] lines = CsvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    st.FromCSV(lines[1]);
                    string dataToString = st.ToString();
                    Console.WriteLine(dataToString);
                    string csvData = st.ToCSV();
                    Console.WriteLine(csvData);
                    students.Add(st);
                }
            }
            //Agregate Functions to find max and min, average age and count students
            Student me = new Student
            {
                StudentId = "<200464602>",
                FirstName = "<Dixit>",
                LastName = "<Ohri>",
                DateOfBirth = "1995-02-04",
            };
            int count = students.Count();
            Console.WriteLine($"The list contain {count} students");
            int highestMax = students.Max(x => x.Age);
            Console.WriteLine($"The highest Age in the list is {highestMax}");
            int lowestMax = students.Min(x => x.Age);
            Console.WriteLine($"The lowest Age in the list is {lowestMax}");
            double averageAge = students.Average(x => x.Age);
            Console.WriteLine($"The average age is => {averageAge.ToString("0")}");
            Student student1 = students.Find(x => x.StudentId == me.StudentId);

            if (student1 != null)
            {
                Console.WriteLine($"Found Student {student1}");
            }
            else
            {
                Console.WriteLine($"Could not find Student {student1}");
            }
            Student studentSingleOrDefault = students.SingleOrDefault(x => x.StudentId == me.StudentId);

            if (studentSingleOrDefault != null)
            {
                Console.WriteLine($"Found Student {studentSingleOrDefault}");
            }
            else
            {
                Console.WriteLine($"Could not find Student {studentSingleOrDefault}");
            }
            
        }
    }
}
