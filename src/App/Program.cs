﻿using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App
{
    public class Program
    {
        public static void Main()
        {
            var result5 = Execute(x => x.AddAnnotation());
            var result6 = Execute(x => x.UpdateAnnotation());
            //string result5 = Execute(x => x.EditPersonalInfo(13, "Carl 2", "Carlson 2", 2, "carl1@gmail.com", 1));
            //string result4 = Execute(x => x.RegisterStudent("Carl", "carl@gmail.com", 2, Grade.B));
            //string result3 = Execute(x => x.DisenrollStudent(1, 2));
            //string result = Execute(x => x.CheckStudentFavoriteCourse(1, 2));
            //string result2 = Execute(x => x.EnrollStudent(1, 2, Grade.A));
        }
        
        private static string Execute(Func<AnnotationsController, string> func)
        {
            string connectionString = GetConnectionString();

            IBus bus = new Bus();
            var messageBus = new MessageBus(bus);
            var eventDispatcher = new EventDispatcher(messageBus);

            using (var context = new SchoolContext(connectionString, true, eventDispatcher))
            {
                var controller = new AnnotationsController(context);
                return func(controller);
            }
        }

        //private static string Execute(Func<StudentController, string> func)
        //{
        //    string connectionString = GetConnectionString();

        //    IBus bus = new Bus();
        //    var messageBus = new MessageBus(bus);
        //    var eventDispatcher = new EventDispatcher(messageBus);

        //    using (var context = new SchoolContext(connectionString, true, eventDispatcher))
        //    {
        //        var controller = new StudentController(context);
        //        return func(controller);
        //    }
        //}

        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration["ConnectionString"];
        }
    }
}
