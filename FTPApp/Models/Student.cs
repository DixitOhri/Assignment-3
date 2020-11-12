using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FTPApp.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string _DateOfBirth;
        /// <summary>
        /// DateOfBirth stored in local DateTime format (see culture setting i.e. 12/31/2020 is Month/Day/Year)
        /// </summary>
        public string DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                _DateOfBirth = value;

                //Convert DateOfBirth to DateTime
                DateTime dtOut;
                DateTime.TryParse(_DateOfBirth, out dtOut);
                DOB = dtOut;
            }
        }
        public DateTime DOB { get; set; }
        public virtual int Age
        {
            get
            {
                DateTime Now = DateTime.Now;
                int Years = new DateTime(DateTime.Now.Subtract(DOB).Ticks).Year - 1;
                DateTime PastYearDate = DOB.AddYears(Years);
                int Months = 0;
                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }
                int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                int Hours = Now.Subtract(PastYearDate).Hours;
                int Minutes = Now.Subtract(PastYearDate).Minutes;
                int Seconds = Now.Subtract(PastYearDate).Seconds;
                return Years;
            }
        }
        public Boolean MyRecord { get; set; }
        private string _ImageData;

        public string Image_Data
        {
            get { return _ImageData; }
            set
            {
                _ImageData = value;
                Image = FTPApp.Models.Utilities.Converter.Base64ToImage(_ImageData);
            }
        }

        public Image Image { get; set; }

        public void FromDirectory(string directory)
        {
            string[] directoryPart = directory.Split(" ", StringSplitOptions.None);
            StudentId = directoryPart[0];
            FirstName = directoryPart[1];
            LastName = directoryPart[2];
            
        }

        public void FromCSV(string csvDataLine)
        {
            try
            {
                string[] csvDataLineParts = csvDataLine.Split(",", StringSplitOptions.None);
                StudentId = csvDataLineParts[0];
                FirstName = csvDataLineParts[1];
                LastName = csvDataLineParts[2];
                DateOfBirth = csvDataLineParts[3];
                Image_Data = csvDataLineParts[4];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public override string ToString()
        {
            return $"{StudentId} -{FirstName},{LastName} - {Age}";
        }

        public string ToCSV()
        {
            string result = $"{StudentId},{FirstName},{LastName},{DOB.ToShortDateString()},{Image_Data}";
            return result;
        }



        public void FIND_STUDENT(List<Student> stdlist, string fname)
        {
            var std = stdlist.Where(s => s.FirstName == fname).SingleOrDefault();
           
            if (std != null)
            {
                std.MyRecord = true;
            }
            else
            {
                std.MyRecord = false;
            }
        }
        public void CONTAINS_STUDENT(List<Student> stdlist, string name)
        {
            int count = 0;
            foreach (var std in stdlist)
            {
                if (std.FirstName.ToLower().Contains(name) || std.LastName.ToLower().Contains(name))
                {
                    count++;
                }
                Console.WriteLine($"There are {count}  of {std.FirstName} {std.LastName}  in list");
            }
        }

        

        

    }
}