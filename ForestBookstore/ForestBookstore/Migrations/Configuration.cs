namespace ForestBookstore.Migrations
{
    using System;
    using System.CodeDom;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Http.Routing;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.DbContext;

    internal sealed class Configuration : DbMigrationsConfiguration<ForestBookstore.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.ContextKey = "MVCBlog.Models.ApplicationDbContext";
        }

        protected override void Seed(ForestBookstore.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                this.CreateUser(context, "admin", "admin@gmail.com", "123", "System Admin", "BlaBla", 3, "BlaBlaBla");
                this.CreateUser(context, "pesho", "pesho@gmail.com", "321", "Pesho Peshev", "BlaBla1", 20, "BlaBlaBla1");

                this.CreateRole(context, "Admin");
                this.AddUserToRole(context, "admin", "Admin");

                this.CreateRole(context, "User");
                this.AddUserToRole(context, "pesho", "User");

                this.CreateBook(
                    context, 
                    "Wild: From Lost to Found on the Pacific Crest Trail", 
                    "Cheryl Strayed", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/wild.jpg"), 
                    "At twenty-two, Cheryl Strayed thought she had lost everything. In the wake of her mother’s death, her family scattered and her own marriage was soon destroyed. Four years later, with nothing more to lose, she made the most impulsive decision of her life. With no experience or training, driven only by blind will, she would hike more than a thousand miles of the Pacific Crest Trail from the Mojave Desert through California and Oregon to Washington State — and she would do it alone. Told with suspense and style, sparkling with warmth and humor, Wild powerfully captures the terrors and pleasures of one young woman forging ahead against all odds on a journey that maddened, strengthened, and ultimately healed her.",
                    12.00M, 
                    20);

                this.CreateBook(
                    context, 
                    "The Curious Incident of the Dog in the Night-Time", 
                    "Mark Haddon", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/the-curios-incident-with-the-dog-in-the-nightime.jpg"), "Christopher John Francis Boone knows all the countries of the world and their capitals and every prime number up to 7,057. He relates well to animals but has no understanding of human emotions. He cannot stand to be touched. And he detests the color yellow. Although gifted with a superbly logical brain, for fifteen - year - old Christopher everyday interactions and admonishments have little meaning. He lives on patterns, rules, and a diagram kept in his pocket.Then one day, a neighbor's dog, Wellington, is killed and his carefully constructive universe is threatened. Christopher sets out to solve the murder in the style of his favourite (logical) detective, Sherlock Holmes. What follows makes for a novel that is funny, poignant and fascinating in its portrayal of a person whose curse and blessing are a mind that perceives the world entirely literally.", 
                    10.00M, 
                    25);

                this.CreateBook(
                    context, 
                    "Women in Science: 50 Fearless Pioneers Who Changed the World", 
                    "Rachel Ignotofsky", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/women_in_science.jpg"), 
                    "<p>A charmingly illustrated and educational book, Women in Science highlights the contributions of fifty notable women to the fields of science, technology, engineering, and mathematics (STEM) from the ancient to the modern world. Full of striking, singular art, this fascinating collection also contains infographics about relevant topics such as lab equipment, rates of women currently working in STEM fields, and an illustrated scientific glossary. The trailblazing women profiled include well-known figures like primatologist Jane Goodall, as well as lesser-known pioneers such as Katherine Johnson, the African-American physicist and mathematician who calculated the trajectory of the 1969 Apollo 11 mission to the moon.</p><br><p>Women in Science celebrates the achievements of the intrepid women who have paved the way for the next generation of female engineers, biologists, mathematicians, doctors, astronauts, physicists, and more!</p>",
                    15.00M,
                    15);

                this.SaveChanges(context);
            }
        }

        private void CreateUser(ApplicationDbContext context,
            string username, string email, string password, string fullName, string town, int phone, string address)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                PersonName = fullName,
                Town = town,
                Phone = phone,
                Address = address
            };

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private void CreateBook(ApplicationDbContext context,
            string title, string authorName, byte[] image, string description, decimal price, int countInStock)
        {
            var book = new Book();
            book.Name = title;
            book.Author = new Author{ Name = authorName };
            book.Image = image;
            book.Description = description;
            book.Price = price;
            book.CurrentCount = countInStock;
            context.Books.Add(book);
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private void AddUserToRole(ApplicationDbContext context, string userName, string roleName)
        {
            var user = context.Users.First(u => u.UserName == userName);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var addAdminRoleResult = userManager.AddToRole(user.Id, roleName);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
