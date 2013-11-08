namespace CoffeeAppWebRole.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CoffeeAppWebRole.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CoffeeAppWebRole.Models.CoffeeAppWebRoleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CoffeeAppWebRole.Models.CoffeeAppWebRoleContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Courses.AddOrUpdate(p => p.Name,
       new Course
       {
           CourseID = 123456,
           Name = "Taste Coffee",
           Description = "Description"
       });
        }
    }
}
