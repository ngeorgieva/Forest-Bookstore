namespace ForestBookstore.Migrations
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using DataLayer.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.DbContext;
    using Models.DataLayer;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
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

                this.SaveChanges(context);

                this.CreateCategory(context, "New Releases");
                this.CreateCategory(context, "Bestsellers");
                this.CreateCategory(context, "Fiction");
                this.CreateCategory(context, "Non-fiction");

                this.SaveChanges(context);

                this.CreateBook(
                    context, 
                    "Wild: From Lost to Found on the Pacific Crest Trail", 
                    "Cheryl Strayed", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/wild.jpg"), 
                    "At twenty-two, Cheryl Strayed thought she had lost everything. In the wake of her mother’s death, her family scattered and her own marriage was soon destroyed. Four years later, with nothing more to lose, she made the most impulsive decision of her life. With no experience or training, driven only by blind will, she would hike more than a thousand miles of the Pacific Crest Trail from the Mojave Desert through California and Oregon to Washington State — and she would do it alone. Told with suspense and style, sparkling with warmth and humor, Wild powerfully captures the terrors and pleasures of one young woman forging ahead against all odds on a journey that maddened, strengthened, and ultimately healed her.",
                    new DateTime(2012, 3, 20), 
                    12.00M, 
                    20, 
                    2, 3);

                this.CreateBook(
                    context, 
                    "The Curious Incident of the Dog in the Night-Time", 
                    "Mark Haddon", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/the-curios-incident-with-the-dog-in-the-nightime.jpg"), "Christopher John Francis Boone knows all the countries of the world and their capitals and every prime number up to 7,057. He relates well to animals but has no understanding of human emotions. He cannot stand to be touched. And he detests the color yellow. Although gifted with a superbly logical brain, for fifteen - year - old Christopher everyday interactions and admonishments have little meaning. He lives on patterns, rules, and a diagram kept in his pocket.Then one day, a neighbor's dog, Wellington, is killed and his carefully constructive universe is threatened. Christopher sets out to solve the murder in the style of his favourite (logical) detective, Sherlock Holmes. What follows makes for a novel that is funny, poignant and fascinating in its portrayal of a person whose curse and blessing are a mind that perceives the world entirely literally.",
                    new DateTime(2003, 7, 31),
                    10.00M,
                    25,
                    3);

                this.CreateBook(
                    context, 
                    "Women in Science: 50 Fearless Pioneers Who Changed the World", 
                    "Rachel Ignotofsky", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/women_in_science.jpg"), 
                    "<p>A charmingly illustrated and educational book, Women in Science highlights the contributions of fifty notable women to the fields of science, technology, engineering, and mathematics (STEM) from the ancient to the modern world. Full of striking, singular art, this fascinating collection also contains infographics about relevant topics such as lab equipment, rates of women currently working in STEM fields, and an illustrated scientific glossary. The trailblazing women profiled include well-known figures like primatologist Jane Goodall, as well as lesser-known pioneers such as Katherine Johnson, the African-American physicist and mathematician who calculated the trajectory of the 1969 Apollo 11 mission to the moon.</p><br><p>Women in Science celebrates the achievements of the intrepid women who have paved the way for the next generation of female engineers, biologists, mathematicians, doctors, astronauts, physicists, and more!</p>",
                    new DateTime(2016, 7, 26), 
                    15.00M,
                    15,
                    1, 4);

                this.CreateBook(context, 
                    "Harry Potter and the Cursed Child - Parts One and Two", 
                    "J.K. Rowling", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/harry-potter.jpg"), 
                    "Based on an original new story by J.K. Rowling, Jack Thorne and John Tiffany, a new play by Jack Thorne, Harry Potter and the Cursed Child is the eighth story in the Harry Potter series and the first official Harry Potter story to be presented on stage. The play will receive its world premiere in London’s West End on July 30, 2016. It was always difficult being Harry Potter and it isn’t much easier now that he is an overworked employee of the Ministry of Magic, a husband and father of three school - age children. While Harry grapples with a past that refuses to stay where it belongs, his youngest son Albus must struggle with the weight of a family legacy he never wanted.As past and present fuse ominously, both father and son learn the uncomfortable truth: sometimes, darkness comes from unexpected places.", 
                    new DateTime(2016, 7, 31), 
                    17.00M, 
                    30, 
                    1, 2, 3);
                

                this.CreateBook(context, 
                    "The Girl on the Train", 
                    "Paula Hawkins", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/the-girl-on-the-train.jpg"), "EVERY DAY THE SAME Rachel takes the same commuter train every morning and night.Every day she rattles down the track, flashes past a stretch of cozy suburban homes, and stops at the signal that allows her to daily watch the same couple breakfasting on their deck.She’s even started to feel like she knows them.Jess and Jason, she calls them.Their life—as she sees it—is perfect.Not unlike the life she recently lost. UNTIL TODAY. And then she sees something shocking.It’s only a minute until the train moves on, but it’s enough.Now everything’s changed.Unable to keep it to herself, Rachel goes to the police.But is she really as unreliable as they say ? Soon she is deeply entangled not only in the investigation but in the lives of everyone involved.Has she done more harm than good?", 
                    new DateTime(2015, 1, 6), 
                    7.00M, 
                    15, 
                    2, 3);

                this.CreateBook(context,
                    "Pro ASP.NET MVC 5",
                    "Adam Freeman",
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/ASP-Net-MVC.jpg"),
                    "The ASP.NET MVC 5 Framework is the latest evolution of Microsoft’s ASP.NET web platform. It provides a high-productivity programming model that promotes cleaner code architecture, test-driven development, and powerful extensibility, combined with all the benefits of ASP.NET. ASP.NET MVC 5 contains a number of advances over previous versions, including the ability to define routes using C# attributes and the ability to override filters. The user experience of building MVC applications has also been substantially improved. The new, more tightly integrated, Visual Studio 2013 IDE has been created specifically with MVC application development in mind and provides a full suite of tools to improve development times and assist in reporting, debugging and deploying your code. The popular Bootstrap JavaScript library has also now been included natively within MVC 5 providing you, the developer, with a wider range of multi-platform CSS and HTML5 options than ever before without the penalty of having to load-in third party libraries.", 
                    new DateTime(2013, 12, 19), 
                    50.00M,
                    10,
                    2, 4);

                this.CreateBook(context, 
                    "Въведение в програмирането със C#", 
                    "Светлин Наков", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/Intro-CSharp-Book-English.jpg"), "Книгата “Въведение в програмирането със C#” е отлично съвременно ръководство за навлизане в програмирането и алгоритмичното мислене с платформата .NET и езика C#. В нея се разглеждат серия уроци по програмиране – от основите на програмирането, среда за разработка, променливи, оператори, масиви и цикли до по-сложни концепции като рекурсия, фундаментални структури от данни и класически алгоритми, списъчни структури, дървета и дървовидни структури, графи, хеш-таблици, оценяване сложността на алгоритми, принципи на обектно-ориентираното програмиране (ООП), LINQ заявки, конструиране на качествен програмен код и решаване на изпитни задачи. Книгата е насочена главно към начинаещите програмисти, които правят своите първи стъпки в сферата на информационните технологии и предлага редица примери и практически задачи за упражнения. Книгата използва езика C#, но получените знания и умения с лекота могат да бъдат приложени при програмиране на всеки един съвременен език, тъй като читателите се запознават в детайли с основните принципи и концепции в разработката на софтуер и развиват своето алгоритмично мислене. Книгата е написана на български език, създадена е от широк авторски колектив, професионалисти, разработващи софтуер от много години, под ръководството на д-р Светлин Наков.", 
                    new DateTime(2011, 8, 8), 
                    10.00M, 
                    8, 
                    2, 4);

                this.CreateBook(
                    context, 
                    "Algorithms", 
                    "Robert Sedgewick", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/algorithms.jpg"), "This fourth edition of Robert Sedgewick and Kevin Wayne's Algorithms is the leading textbook on algorithms today and is widely used in colleges and universities worldwide. This book surveys the most important computer algorithms currently in use and provides a full treatment of data structures and algorithms for sorting, searching, graph processing, and string processing -- including fifty algorithms every programmer should know. In this edition, new Java implementations are written in an accessible modular programming style, where all of the code is exposed to the reader and ready to use. The algorithms in this book represent a body of knowledge developed over the last 50 years that has become indispensable, not just for professional programmers and computer science students but for any student with interests in science, mathematics, and engineering, not to mention students who use computation in the liberal arts.", 
                    new DateTime(2011, 3, 19), 
                    60.00M, 
                    2, 
                    4);

                this.CreateBook(context, 
                    "The Casual Vacancy", 
                    "J.K. Rowling",
                    File.ReadAllBytes(
                        "D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/the-casual-vacancy.jpg"),
                    "When Barry Fairbrother dies in his early forties, the town of Pagford is left in shock. Pagford is, seemingly, an English idyll, with a cobbled market square and an ancient abbey, but what lies behind the pretty façade is a town at war. Rich at war with poor, teenagers at war with their parents, wives at war with their husbands, teachers at war with their pupils...Pagford is not what it first seems. And the empty seat left by Barry on the parish council soon becomes the catalyst for the biggest war the town has yet seen.Who will triumph in an election fraught with passion, duplicity and unexpected revelations?",
                    new DateTime(2012, 9, 27), 
                    8.00M, 
                    12, 
                    3);

                this.CreateBook(context, 
                    "Very Good Lives: The Fringe Benefits of Failure and the Importance of Imagination", 
                    "J.K. Rowling", 
                    File.ReadAllBytes("D:/SoftUni/Tech Module/Software Technologies/Teamwork-Forest Bookstore/Forest-Bookstore/ForestBookstore/ForestBookstore/Content/Images/very-good-lives.jpg"), "In 2008, J.K. Rowling delivered a deeply affecting commencement speech at Harvard University. Now published for the first time in book form, Very Good Lives offers J.K. Rowling’s words of wisdom for anyone at a turning point in life, asking the profound and provocative questions: How can we embrace failure? And how can we use our imagination to better both ourselves and others?", 
                    new DateTime(2015, 4, 14), 
                    10.00M, 
                    4, 
                    4);
                this.SaveChanges(context);


                this.AddReviewToBook(context, "Въведение в програмирането със C#", "The book encompasses the topics needed to start practical with programming. The approach with topics and assignments is the best for teaching a person how to program. Great work.", "pesho");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "Pro ASP.NET MVC 5", "I'm often frustrated that I cannot work through the tutorials or use the examples because my company blocks NuGet with no work around and many of the open source packages are never approved anyway. So I wasted my money purchasing this book. A better approach, more useful, would be to use the features that come standard with ASP.NET Visual Studio as opposed to relying so heavily on 3rd party downloads.", "pesho");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "Pro ASP.NET MVC 5", "Excellent book, and excellent place to start. I bought it about two and a half years ago, I was making less than 20k and now I'm making close to six figures. My only criticism is that it complicates learning about dependency injection but kudos for introducing it early because learning how to use design patterns is key to being a good programmer. I also never could figure out how to get the referenced materials from the publisher's website. Neither one of these things affects my 5 stars though. Great book you can't go wrong.", "admin");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "The Girl on the Train", "I read this book due to the fact that everyone else read it. Yes, mom I would jump off the cliff right behind everyone else.", "pesho");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "The Girl on the Train", "Excellent", "admin");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "Harry Potter and the Cursed Child - Parts One and Two", "Let's be honest - this read like badly written fan fiction", "pesho");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "Harry Potter and the Cursed Child - Parts One and Two", "I very much enjoyed this story. I knew it was in scrip form and thought it would be harder to read but it wasn't. In saying that I wish it would have been more like a regular book. I missed the more which comes in a regular story. I was grateful to be back in the Harry Potter world if even for a very short time.", "admin");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "Wild: From Lost to Found on the Pacific Crest Trail", "A self-absorbed, ill-prepared woman, 26 years old, leaves her husband (a decent guy) for no good reason, mucks her life up even further with drugs and reckless sex, then engages in some vacuous navel-gazing on the Pacific Crest Trail. As a woman hiking alone she gets all kinds of special treatment and help from fellow hikers. She loses a few pounds, gets some muscles and some sun-bleached hair and calls her work done.", "admin");
                this.SaveChanges(context);

                this.AddReviewToBook(context, "Wild: From Lost to Found on the Pacific Crest Trail", "So far, a great read. It's Eat, Pray, Love without all the whining.", "pesho");
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

        private void CreateCategory(ApplicationDbContext context, string name)
        {
            var category = new Category();
            category.Name = name;
            context.Categories.Add(category);
        }

        private void CreateBook(ApplicationDbContext context,
            string title, string authorName, byte[] image, string description, DateTime releaseDate, decimal price, int countInStock, params int[] categoryIds)
        {
            var book = new Book();
            book.Name = title;
            book.Author = new Author{ Name = authorName };
            book.Image = image;
            book.Description = description;
            book.ReleaseDate = releaseDate;
            book.Price = price;
            book.CurrentCount = countInStock;

            if (categoryIds.Length > 0)
            {
                foreach (var id in categoryIds)
                {
                    var category = context.Categories.Single(c => c.Id == id);
                    book.Categories.Add(category);
                }
            }
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

        private void AddReviewToBook(ApplicationDbContext context, string bookTitle, string text, string authorUsername)
        {
            Book book = context.Books.Include(b => b.Reviews).FirstOrDefault(b => b.Name == bookTitle);
            if (book != null)
            {
                Review review = new Review();
                review.Text = text;
                ApplicationUser author = context.Users.FirstOrDefault(u => u.UserName == authorUsername);
                if (author != null)
                {
                    review.Author = author;
                }
                
                book.Reviews.Add(review);
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
