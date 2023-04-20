using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeder;

public class DataSeeder : IDataSeeder
{
    private readonly BlogDbContext _dbContext;

    public DataSeeder(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Posts.Any())
        {
            return;
        }

        var authors = AddAuthors();
        var categories = AddCategories();
        var tags = AddTags();
        var posts = AddPosts(authors, categories, tags);

    }

    private IList<Author> AddAuthors()
    {
        var authors = new List<Author>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Jason Mouth",
                UrlSlug = "jason-mouth",
                Email = "json@gmail.com",
                JoinedDate = new DateTime(2022, 10, 21)
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Jessica Wonder",
                UrlSlug = "jessica-wonder",
                Email = "jessica665@motip.com",
                JoinedDate = new DateTime(2022, 4, 19)
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Fuji Kaze",
                UrlSlug = "fuji-kaze",
                Email = "fuji@gmail.com",
                JoinedDate = new DateTime(2023, 01, 21)
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Chillies",
                UrlSlug = "chillies",
                Email = "chillies@gmail.com",
                JoinedDate = new DateTime(2022, 12, 19)
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Mizuki Rin",
                UrlSlug = "mizuki-rin",
                Email = "mizukirinka@gmail.com",
                JoinedDate = new DateTime(2022, 6, 19)
            }
        };

        _dbContext.Authors.AddRange(authors);
        _dbContext.SaveChanges();
        return authors;
    }

    private IList<Category> AddCategories()
    {
        var categories = new List<Category>()
        {
            new() {Id = Guid.NewGuid(), Name = ".NET Core", Description = ".NET Core", UrlSlug = "net-core"},
            new() {Id = Guid.NewGuid(), Name = "Architecture", Description = "Architecture", UrlSlug = "architecture"},
            new() {Id = Guid.NewGuid(), Name = "Messaging", Description = "Messaging", UrlSlug = "messaging"},
            new() {Id = Guid.NewGuid(), Name = "OOP", Description = "Object-Oriented Program", UrlSlug = "oop"},
            new() {Id = Guid.NewGuid(), Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "design-patterns"},
            new() {Id = Guid.NewGuid(), Name = "React", Description = "React is JavaScript library", UrlSlug = "react-js"},
            new() {Id = Guid.NewGuid(), Name = "Angular", Description = "Angular is a TypeScript-based", UrlSlug = "Angular-js"},
            new() {Id = Guid.NewGuid(), Name = "Vue.js", Description = "Vue.js is an open-source model–view–viewmodel", UrlSlug = "vue-js"},
            new() {Id = Guid.NewGuid(), Name = "Next.js", Description = "Next.js is an open-source web", UrlSlug = "next-js"},
            new() {Id = Guid.NewGuid(), Name = "Node.js", Description = "Node.js is a cross-platform, open-source server", UrlSlug = "node-js"},
        };

        _dbContext.Categories.AddRange(categories);
        _dbContext.SaveChanges();
        return categories;
    }

    private IList<Tag> AddTags()
    {
        var tags = new List<Tag>()
        {
            new() {Id = Guid.NewGuid(), Name = "Google", Description = "Google Application", UrlSlug = "google"},
            new() {Id = Guid.NewGuid(), Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "asp-net-mvc"},
            new() {Id = Guid.NewGuid(), Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page"},
            new() {Id = Guid.NewGuid(), Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
            new() {Id = Guid.NewGuid(), Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning"},
            new() {Id = Guid.NewGuid(), Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-network"},
            new() {Id = Guid.NewGuid(), Name = "Front-End Applications", Description = "Consisting of all visual and interactive elements, this side of the site can be experienced by all users.", UrlSlug = "font-end-application"},
            new() {Id = Guid.NewGuid(), Name = "Visual Studio", Description = "An integrated development environment from Microsoft.", UrlSlug = "visual-studio"},
            new() {Id = Guid.NewGuid(), Name = "SQL Server", Description = "A relational database management system developed by Microsoft.", UrlSlug = "sql-server"},
            new() {Id = Guid.NewGuid(), Name = "Git", Description = "Git is a distributed version control system that tracks changes in any set of computer files, usually used for coordinating work among programmers", UrlSlug = "git"},
            new() {Id = Guid.NewGuid(), Name = "EF Core", Description = "A lightweight, extensible, open source and cross-platform version of the popular Entity Framework data access technology", UrlSlug = "entity-framework"},
            new() {Id = Guid.NewGuid(), Name = ".NET Framework", Description = "A proprietary software framework developed by Microsoft.", UrlSlug = "net-framework"},
            new() {Id = Guid.NewGuid(), Name = "ASP.NET Core", Description = ".NET is a free and open-source, managed computer software framework for Windows, Linux, and macOS operating systems", UrlSlug = "aspnet-core"},
            new() {Id = Guid.NewGuid(), Name = "Postman", Description = "Postman is an API platform for developers to design, build, test and iterate their APIs.", UrlSlug = "postman"},
            new() {Id = Guid.NewGuid(), Name = "ChatGPT", Description = "A chat-bot developed by OpenAI.", UrlSlug = "chat-gpt"},
            new() {Id = Guid.NewGuid(), Name = "Data cleansing", Description = "Data cleaning is the process of fixing or removing incorrect, corrupted, incorrectly formatted, duplicate, or incomplete data within a data-set.", UrlSlug = "data-cleansing"},
            new() {Id = Guid.NewGuid(), Name = "Fetch api", Description = "he Fetch API provides a JavaScript interface for accessing and manipulating parts of the protocol, such as requests and responses.", UrlSlug = "fetch-api"},
            new() {Id = Guid.NewGuid(), Name = "Microsoft", Description = "Microsoft", UrlSlug = "microsoft"},
            new() {Id = Guid.NewGuid(), Name = "Microservices", Description = "An approach to building an application that breaks its functionality into modular components.", UrlSlug = "microservices"},
            new() {Id = Guid.NewGuid(), Name = "Web API security", Description = "Web API security entails authenticating programs", UrlSlug = "web-api-security"}
            };

        _dbContext.Tags.AddRange(tags);
        _dbContext.SaveChanges();
        return tags;
    }

    private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags)
    {
        var posts = new List<Post>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friend has great repository filled",
                Description = "Here's a few great DON'T and DO example",
                Meta = "David and friend has great repository filled",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[3],
                Category = categories[4],
                Tags = new List<Tag>()
                {
                    tags[17],
                    tags[18],
                    tags[19],
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "10 Web Development Trends in 2023",
                ShortDescription = "Robin Wieruch",
                Description = "The most popular meta framework called Next.js comes on top of React.js. " +
                              "SSR is all over the place when working with JavaScript frameworks these days.",
                Meta = "Robin Wieruch",
                UrlSlug = "10-web-development-trends-in-2023",
                Published = true,
                PostedDate = new DateTime(2022, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[1],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[15],
                    tags[16],
                    tags[17],
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Creating a resume builder with React, NodeJS and AI",
                ShortDescription = "Resume builder with AI",
                Description = "OpenAI GPT-3 is a type of artificial intelligence program developed by OpenAI " +
                              "that is really good at understanding and processing human language.",
                Meta = "Resume builder with AI",
                UrlSlug = "creating-a-resume-builder-with-react-node-js-and-ai",
                Published = true,
                PostedDate = new DateTime(2022, 9, 20, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[2],
                Category = categories[1],
                Tags = new List<Tag>()
                {
                    tags[7],
                    tags[9],
                    tags[3],
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "LazyWeb",
                ShortDescription = "The ultimate resource for developers",
                Description = "\"Your feedback is incredibly valuable to me as I'm constantly looking for ways to make" +
                              " Lazyweb the best resource solution for developers. So please don't hesitate to share your thoughts " +
                              "and help shape the future of this platform,\" says developer.",
                Meta = "The ultimate resource for developers",
                UrlSlug = "lazy-web",
                Published = true,
                PostedDate = new DateTime(2022, 12, 23, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[4],
                Category = categories[2],
                Tags = new List<Tag>()
                {
                    tags[7],
                    tags[9],
                    tags[3],
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Custom ScrollBar",
                ShortDescription = "Custom ScrollBar with Pure CSS",
                Description = "We must first understand the various components of a scrollbar because each component must be styled separately.",
                Meta = "Custom ScrollBar with Pure CSS",
                UrlSlug = "custom-scrollBar",
                Published = true,
                PostedDate = new DateTime(2022, 3, 3, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[3],
                Category = categories[4],
                Tags = new List<Tag>()
                {
                    tags[7],
                    tags[9],
                    tags[3],
                }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Understanding Database Types",
	            ShortDescription = "Understanding Database Types",
	            Description = "A complex application usually uses several different databases, each catering to a specific aspect of the application’s needs." +
	                          " In this comprehensive three-part series, we’ll explore the art of database selection. " +
	                          "To make the best decision for our projects, it is essential to understand the various types of databases available in the market",
	            Meta = "Understanding Database Types",
	            UrlSlug = "understanding-database-types",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 3, 10, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 10,
	            Author = authors[2],
	            Category = categories[4],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[9],
		            tags[3],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "A Detailed Recreation of Browser",
	            ShortDescription = "Understanding Database Types",
	            Description = "A Detailed Recreation of Browser Inspired by Super Mario Bros. Movie was created by Ümral Ismayilov," +
	                          " a 3D Character Artist and Motion Designer who specializes in " +
	                          "reimagining cartoon characters in 3D. The character was modeled in ZBrush, retopologized " +
	                          "in Maya, and textured with Substance 3D Painter",
	            Meta = "A Detailed Recreation of Bowser",
	            UrlSlug = "a-detailed-recreation-of-browser",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 3, 11, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 10,
	            Author = authors[1],
	            Category = categories[4],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[6],
		            tags[3],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Selling sanctioned storage",
	            ShortDescription = "Selling sanctioned storage to Huawei costs Seagate $300m",
	            Description = "Selling sanctioned storage to Huawei costs Seagate $300 million." +
	                          " The company was on the right side of the law when shipping 7.4 million hard drives to China," +
	                          " in 429 transactions worth a combined $1.1 billion." +
	                          " The U.S. Department of Commerce's s Bureau of Industry and Security sent it a letter alleging violations of the U.",
	            Meta = "A Detailed Recreation of Bowser",
	            UrlSlug = "selling-sanctioned-storage",
	            Published = true,
	            PostedDate = new DateTime(2023, 1, 23, 11, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 10,
	            Author = authors[0],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[0],
		            tags[4],
		            tags[2],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Primo is the IT tool",
	            ShortDescription = "Selling sanctioned storage to Huawei costs Seagate $300m",
	            Description = "Primo is a French startup that recently raised " +
	                          "a $3.4 million funding round to build a software-as-a-service product that handles the IT needs of small and medium companies. " +
	                          "Primo keeps track of your fleet of devices and can then help you manage it without a dedicated IT manager.",
	            Meta = "Primo is the IT tool",
	            UrlSlug = "primo-is-the-it-tool",
	            Published = true,
	            PostedDate = new DateTime(2023, 2, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 10,
	            Author = authors[3],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[0],
		            tags[3],
		            tags[1],
		            tags[4],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "7 Modern and Powerful JavaScript",
	            ShortDescription = "7 Modern and Powerful JavaScript Features You Didn’t Know About\r\n",
	            Description = "Optional Chaining Optional chaining is a new feature in JavaScript that allows " +
	                          "developers to write code that is more concise and easier to read. " +
	                          "The Promise.allSettled() and BigInt The BigInt data type are just a few " +
	                          "of the many cool and modern JavaScript features that are available to developers.",
	            Meta = "7 Modern and Powerful JavaScript",
	            UrlSlug = "7-modern-and-powerful-javascript",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[4],
	            Category = categories[5],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
		            tags[2],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "A cure for React useState hell?",
	            ShortDescription = "A cure for React useState hell?",
	            Description = "There's nothing preventing you from choosing an end date that’s before the start date," +
	                          " which makes no sense. There's no guard for a title or description that is too long. " +
	                          "Using useReducer, we could transform the above code, " +
	                          "to just this: i The hook helps you control transformations from state A to state B",
	            Meta = "A cure for React useState hell?",
	            UrlSlug = "a-cure-for-react-usestate-hell",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
		            tags[2],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "10 Best React Libraries",
	            ShortDescription = "10 Best React Libraries for Building High-Performance Web Applications 2023",
	            Description = "React.js is one of the most popular front-end frameworks today, known for its reusability, " +
	                          "scalability, and efficiency in developing complex web applications. With a vast library of React components, " +
	                          "developers can create intuitive and interactive user interfaces that are fast and reliable.",
	            Meta = "10 Best React Libraries",
	            UrlSlug = "10-best-react-libraries",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Flipboard brings editorial curation",
	            ShortDescription = "Flipboard brings editorial curation to Mastodon with ‘desks’ for news and discovery",
	            Description = "Flipboard brings editorial curation to Mastodon with 'desks' for news and discovery." +
	                          " Initially, the company will launch four desks — News, Tech, Culture, and Science — " +
	                          "which it says won’t be automated by bots, but by professional curators who have expertise" +
	                          " in discovering and elevating interesting content",
	            Meta = "Flipboard brings editorial curation",
	            UrlSlug = "flipboard-brings-editorial-curation",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Top 7 ChatGPT Developer Hacks",
	            ShortDescription = "Top 7 ChatGPT Developer Hacks",
	            Description = "A good engineer needs to recognize how data structures are used in our daily lives." +
	                          " Engineers should be aware of these data structures and their use cases to create effective and efficient solutions. " +
	                          "Message brokers play a crucial role when building distributed systems or " +
	                          "microservices to improve their performance, scalability and maintainability.",
	            Meta = "Top 7 ChatGPT Developer Hacks",
	            UrlSlug = "top-7-chatgpt-developer-hacks",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "How Large Language Models Changed",
	            ShortDescription = "How Large Language Models Changed My Entire OSINT Workflow",
	            Description = "The LLM OSINT Analyst Explorer series delves into the potential " +
	                          "of Large Language Models (LLMs) for uncovering expert-driven insights on specialized topics such as Defense," +
	                          " National Security, Counter-Terrorism, and the battle against" +
	                          " Organized Crime. We will demonstrate the development of custom",
	            Meta = "How Large Language Models Changed",
	            UrlSlug = "how-large-language-models-changed",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[3],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "The Complete Modern React Developer 2022",
	            ShortDescription = "The Complete Modern React Developer 2022",
	            Description = "The Complete Modern React Developer 2022 - DEV Community Introduction" +
	                          " will give you the skills and knowledge to become a Software Developer across the full stack. " +
	                          "The only three topics which are not covered in this course are Redux," +
	                          " GraphQL and React Native which could be covered in a future course.",
	            Meta = "The Complete Modern React Developer 2022",
	            UrlSlug = "the-complete-modern-react-developer-2022",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[0],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Clean code practice",
	            ShortDescription = "Clean code practice: Must for every coder and seniors",
	            Description = "Must for every coder and seniors to know about Clean Code: " +
	                          "Good Practices P1 must for juniors and seniors. " +
	                          "Clean Code is just concerned with writing legible code which ultimately helps in maintaining the code. " +
	                          "Code should follow the naming conventions and common best practices and patterns of the language used",
	            Meta = "Clean code practice",
	            UrlSlug = "clean-code-practice",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[0],
	            Category = categories[1],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[3],
		            tags[2],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Twitter advertisers told",
	            ShortDescription = "Twitter advertisers told: Come back, but don't make demands",
	            Description = "Twitter advertisers told: Come back, but don't make demands. " +
	                          "Elon Musk told NBCUniversal:" +
	                          " \"It is not cool to say what Twitter will do\" " +
	                          "More than half of Twitter's top 1,000 advertisers " +
	                          "have yet to return to the social media site",
	            Meta = "Twitter advertisers told",
	            UrlSlug = "twitter-advertisers-told",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[3],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "REST vs. gRPC",
	            ShortDescription = "REST vs. gRPC - What’s the Difference?",
	            Description = "REST has been around for a long time and is an industry standard for developing and designing APIs. Google Remote Procedure Call" +
	                          ", or gRPC, was created on top of the RPC protocol. " +
	                          "The Amplication repository is about to hit 10,000-stars on GitHub.",
	            Meta = "REST vs. gRPC",
	            UrlSlug = "rest-vs-grpc",
	            Published = true,
	            PostedDate = new DateTime(2023, 3, 20, 21, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[3],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "JavaScript Optimization",
	            ShortDescription = "JavaScript Optimization Techniques for Faster Website Load Times: An In-Depth Guide",
	            Description = "JavaScript Optimization Techniques for Faster Load Times: " +
	                          "An In- Depth Guide Master JavaScript optimization to enhance website performance. " +
	                          "In this article, I’ll guide you through various methods to optimize your JavaScript code, " +
	                          "including minimizing file sizes, reducing network requests.",
	            Meta = "JavaScript Optimization",
	            UrlSlug = "javascript-optimization",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 20, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[3],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[4],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "System Design Master Template",
	            ShortDescription = "System Design Master Template: How to Answer Any System Design Interview Question.",
	            Description = "System Design Master Template: How to Answer Any System Design Interview Question. The two biggest challenges of answering a system design interview question are: " +
	                          "To know where to start. Arslan Ahmad: " +
	                          "Have a look at the top image to understand " +
	                          "the major components that could be part of any system design.",
	            Meta = "System Design Master Template",
	            UrlSlug = "system-design-master-template",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "The most failed JavaScript interview questions",
	            ShortDescription = "The most failed JavaScript interview questions",
	            Description = "The most failed JavaScript interview questions are broken down by topic and the percentage of correct answers from our telegram channel. " +
	                          "For example, we have chosen a quiz that seems to cover all aspects of this topic. " +
	                          "Try yourself and read the explanation.",
	            Meta = "The most failed JavaScript interview questions",
	            UrlSlug = "the-most-failed-javascript-interview-questions",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[2],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Real-time notifications",
	            ShortDescription = "Real-time notifications",
	            Description = "New notification center will allow you to stay up-to-date with all the latest updates in real-time. " +
	                          "You can receive notifications when you get an upvote, comment, mention, or any other major activity on our platform. " +
	                          "Simply click on the notification center icon to view your notifications and stay on top of everything that is happening.",
	            Meta = "Real-time notifications",
	            UrlSlug = "real-time-notifications",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[0],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Ready at Dawn and Downpour Interactive",
	            ShortDescription = "Ready at Dawn and Downpour Interactive impacted by Meta layoffs",
	            Description = "If you see anyone from M*ta (and Ready at Dawn in particular) looking for positions, please signal boost them",
	            Meta = "Ready at Dawn and Downpour Interactive",
	            UrlSlug = "ready-at-dawn-and-downpour-interactive",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[0],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Snap is expanding its AR features",
	            ShortDescription = "Snap is expanding its AR features to 16 additional music festivals",
	            Description = "Snap is expanding its AR features to 16 additional music festivals. " +
	                          "The company first inked a multi-year deal with Live Nation last April to ‘elevate performances " +
	                          "beyond stages and screens’ Snap is also teaming up with event visualization " +
	                          "company Disguise to enable interactive on-stage AR visuals.",
	            Meta = "Snap is expanding its AR features",
	            UrlSlug = "snap-is-expanding-its-ar-features",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[0],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Basics of CI/CD Pipeline",
	            ShortDescription = "Basics of CI/CD Pipeline",
	            Description = "CI/CD Pipeline is a modern software development practice in which incremental code changes are made frequently and reliably." +
	                          " The pipeline is a logical demonstration of how software will move along the various phases or stages " +
	                          "in this lifecycle before it is delivered to the customer or before it's live on production.",
	            Meta = "Basics of CI/CD Pipeline",
	            UrlSlug = "basics-of-ci-cd-pipeline",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[0],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "UI/UX Design Trends 2023",
	            ShortDescription = "UI/UX Design Trends 2023",
	            Description = "UI/UX Design Trends 2023. Yet another year is coming to a close." +
	                          " We are taking a more careful look at both UI and UX trends that continue to evolve." +
	                          " We highlight some of the trends we think will persist and perhaps gain even more traction in the next year.",
	            Meta = "UI/UX Design Trends 2023",
	            UrlSlug = "ui-ux-design-trends-2023",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Court reverses $20m sandbox patent judgment against Google",
	            ShortDescription = "Court reverses $20m sandbox patent judgment against Google",
	            Description = "Court reverses $20m sandbox patent judgment against Google. Google has convinced an appeals court to reverse " +
	                          "a $20 million judgment against the web giant after Chrome infringed some patents. " +
	                          "The court said that three of the reissued patents, used to support the claim against Google",
	            Meta = "Court reverses $20m sandbox patent judgment against Google",
	            UrlSlug = "court-reverses-usd20m-sandbox-patent-judgment-against-google",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[4],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Advanced JavaScript",
	            ShortDescription = "Advanced JavaScript Console Logging for Developers",
	            Description = "Advanced JavaScript Console Logging for Developers Developers need to debug their applications to find errors and issues." +
	                          " Knowing to use advanced console logging is a handy tool for developers. " +
	                          "These tools show us how our app looks out in the wild and enable us to fix it on the fly without relying on source code.",
	            Meta = "Advanced JavaScript",
	            UrlSlug = "advanced-javascript",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[2],
	            Category = categories[3],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[2],
		            tags[0],
	            }
            },
            new()
            {
	            Id = Guid.NewGuid(),
	            Title = "Recording Badge Scans in Apache Pinot",
	            ShortDescription = "Recording Badge Scans in Apache Pinot",
	            Description = "Use a real-time analytics database to record NFC badge scans in Apache Pinot - " +
	                          "DZone Recording Badge Scans in Pinot. " +
	                          "Join the DZone community and get the full member experience. " +
	                          "Join for free about Pinot ApachePinot.",
	            Meta = "Recording Badge Scans in Apache Pinot",
	            UrlSlug = "recording-badge-scans-in-apache-pinot",
	            Published = true,
	            PostedDate = new DateTime(2023, 4, 19, 2, 20, 0),
	            ModifiedDate = null,
	            ViewCount = 0,
	            Author = authors[1],
	            Category = categories[2],
	            Tags = new List<Tag>()
	            {
		            tags[1],
		            tags[3],
		            tags[0],
	            }
            },
		};
        _dbContext.Posts.AddRange(posts);
        _dbContext.SaveChanges();
        return posts;
    }

}