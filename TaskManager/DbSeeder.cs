using System.Numerics;
using System;
using infrastructure.Extentions;
using Microsoft.Identity.Client;
using infrastructure.Persistence;
using Domain.Task;
using Domain.Employee;
using Domain.Team;
using Domain.Project;
namespace TaskManager
{
    public static class DbSeeder
    {
        
        public static void Seed(IApplicationBuilder app)
        {

            
            TaskManagerDbContext Context =  app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<TaskManagerDbContext>();
            
            if(!Context.projects.Any())
            {

                Projects project1 = new Projects {
                    Name = "E-commerce Platform",
                    Description = "Building a comprehensive platform for online sales and customer management.",
                    Type = "E-commerce"
                };
                
                
                Projects project2 = new Projects
                {
                    Name = "Customer Relationship Management System",
                    Description = "Developing a system to manage interactions with current and potential customers.",
                    Type = "Software"
                };

                Projects project3 = new Projects
                {
                    Name = "SEO Optimization",
                    Description = "Implementing SEO strategies to improve website visibility and ranking on search engines.",
                    Type = "Marketing"
                };
                project1.Tasks = new List<Tasks>();

                List<Teams> teams = new List<Teams> {

                    new Teams{
                        Name = "Quality Assurance",
                        Description = "Ensures the quality of software products through rigorous testing and evaluation."
                    },

                    new Teams{
                        Name = "Backend Team",
                        Description = "Develops and maintains the server-side logic, databases, and application integration."
                    },

                    new Teams{
                        Name = "Frontend Team",
                        Description = "Focuses on developing user interface and user experience for web and mobile applications."
                    },

                    new Teams{
                        Name = "DevOps Team",
                        Description = "Facilitates collaboration between development and operations teams to improve productivity and project delivery."
                    },

                    new Teams{
                        Name = "Product Management",
                        Description = "Oversees the development and marketing strategy of products from conception to launch."
                    },

                    new Teams{
                        Name = "UX/UI Design Team",
                        Description = "Designs intuitive and engaging user interfaces that enhance user experience."
                    },

                    new Teams{
                        Name = "Marketing Team",
                        Description = "Develops strategies to effectively promote products and engage with the target audience."
                    },

                    new Teams{
                        Name = "Customer Support",
                        Description = "Provides assistance and support to customers, ensuring customer satisfaction and loyalty."
                    }

                }; 

                List<Employees> employess = new List<Employees> {
                    // 0
                    new Employees{
                        Manager = true,
                        FirstName = "Angel",
                        LastName = "Rodriguez",
                        PhoneNumber = "1486738006",
                        Email = "lhorton@yahoo.com",
                        Password = "5yZ83R5yFZ@1".Hash(),
                        Position = "Quality Assurance",
                        BirthDay = DateTime.Parse("2004-05-10"),
                        team = teams[0]
                        ,Projects = [project1, project2]
                    
                        },

                    // 1
                     new Employees{
                        Manager = true,
                        FirstName = "Mark",
                        LastName = "Ruben",
                        PhoneNumber = "13867384",
                        Email = "Mark@yahoo.com",
                        Password = "5yZ83R589yFZ@1".Hash(),
                        Position = "Quality Assurance",
                        BirthDay = DateTime.Parse("2004-05-23"),
                        team = teams[0]
                        ,Projects = [project1 , project2]
                        },
                     
                     // 2  
                     new Employees{
                        Manager = true,
                        FirstName = "Muhammed",
                        LastName = "Ali",
                        PhoneNumber = "15315531584",
                        Email = "Muhammed@yahoo.com",
                        Password = "5yZ8efjknvj589yFZ@1".Hash(),
                        Position = "Quality Assurance Manager",
                        BirthDay = DateTime.Parse("2001-08-20"),
                        team = teams[0]
                        ,Projects = [project1]

                        },
                     //3
                     new Employees{
                        Manager = true,
                        FirstName = "Turki",
                        LastName = "Salem",
                        PhoneNumber = "15614584",
                        Email = "TrukiSallem@yahoo.com",
                        Password = "5yvklmklerev81".Hash(),
                        Position = "Quality Assurance intern",
                        BirthDay = DateTime.Parse("2004-04-10"),
                        team = teams[0]
                        ,Projects = [project1]

                        }
                    ,
                     // 4
                    new Employees{
                        Manager = false,
                        FirstName = "Norma",
                        LastName = "Mcguire",
                        PhoneNumber = "+1-482-866-4887x1209",
                        Email = "bianca43@hotmail.com",
                        Password = "zO3UnbhipB(Q".Hash(),
                        Position = "Project Manager",
                        BirthDay = DateTime.Parse("1995-11-12"),
                        team = teams[4]
                        ,Projects = [project1, project2]
                        },
                    // 5
                    new Employees{
                        Manager = true,
                        FirstName = "Rhonda",
                        LastName = "Huynh",
                        PhoneNumber = "001-622-105-4216x6535",
                        Email = "pricesherri@stevens-williams.com",
                        Password = "cG0MSmuHF(GH".Hash(),
                        Position = "System Analyst",
                        BirthDay = DateTime.Parse("1980-03-24"),
                        team = teams[4]
                        ,Projects = [project1, project2]
                        },
                    // 6
                    new Employees{ 
                        Manager = true,
                        FirstName = "Laura",
                        LastName = "Williams",
                        PhoneNumber = "336-047-3655",
                        Email = "nicolecervantes@yahoo.com",
                        Password = "123456".Hash(),
                        Position = "Software Devoleper",
                        BirthDay = DateTime.Parse("1959-03-10"),
                        team = teams[1]
                        ,Projects = [project1, project2]
                    },

                    // 7
                    new Employees{
                        Manager = true,
                        FirstName = "Sattam",
                        LastName = "Aljofan",
                        PhoneNumber = "96654233975135",
                        Email = "SattamDfo@yahoo.com",
                        Password = "jioejv*U&&^".Hash(),
                        Position = "backend Devoleper",
                        BirthDay = DateTime.Parse("1990-03-10"),
                        team = teams[1]
                        ,Projects = [project1, project2]
                    },
                    
                    // 8
                    new Employees{
                        Manager = true,
                        FirstName = "Fahad",
                        LastName = "Alturki",
                        PhoneNumber = "966154315132",
                        Email = "SattamDfo@yahoo.com",
                        Password = "mitkrmgi*&^T%^".Hash(),
                        Position = "Software Engineer",
                        BirthDay = DateTime.Parse("1995-05-22"),
                        team = teams[1]
                        ,Projects = [project1, project2]
                    },
                    // 9
                    new Employees{
                        Manager = true,
                        FirstName = "Ibrahim",
                        LastName = "Alrayes",
                        PhoneNumber = "9661515641561",
                        Email = "IbrahimAlr@yahoo.com",
                        Password = "kcmek&Y^*&^T%^".Hash(),
                        Position = "Software Engineer",
                        BirthDay = DateTime.Parse("2001-05-22"),
                        team = teams[1]
                        ,Projects = [project1]
                    },

                    // 10
                    new Employees{
                        Manager = true,
                        FirstName = "Muhammed",
                        LastName = "Ali",
                        PhoneNumber = "966155648468",
                        Email = "MuhammedAlih.com",
                        Password = "ktbejknY^*&^T%^".Hash(),
                        Position = "Software Engineer intern",
                        BirthDay = DateTime.Parse("2002-06-12"),
                        team = teams[1],
                        Projects = [project1]
                    },

                    // 11
                    new Employees{
                        Manager = true,
                        FirstName = "Mark",
                        LastName = "Liu",
                        PhoneNumber = "498.421.3270x723",
                        Email = "brightemily@daniel.com",
                        Password = "1k&owaAiPi2O".Hash(),
                        Position = "Product Manager",
                        BirthDay = DateTime.Parse( "1960-09-20"),
                        team = teams[4]
                        ,Projects = [project1, project2]
                    },

                    // 12 
                    new Employees{
                        Manager = false,
                        FirstName = "Amy",
                        LastName = "Collins",
                        PhoneNumber = "877.171.3555x11015",
                        Email = "ericescobar@hotmail.com",
                        Password = "*yzP41Wgg!Zn".Hash(),
                        Position = "Project Manager",
                        BirthDay = DateTime.Parse("1966-03-18"),
                    team = teams[4]
                    ,Projects = [project1, project2]
                    },

                    // 13
                    new Employees{
                        Manager = false,
                        FirstName = "Ruben",
                        LastName = "Nevez",
                        PhoneNumber = "1515615611321",
                        Email = "RumbenNeves@hotmail.com",
                        Password = "*krmbgoirtmbgg!Zn".Hash(),
                        Position = "Product Manager",
                        BirthDay = DateTime.Parse("1966-03-18"),
                    team = teams[4]
                    ,Projects = [project1, project2]
                    },

                    // 14
                    new Employees{
                        Manager = false,
                        FirstName = "Kenneth",
                        LastName = "Mitchell",
                        PhoneNumber = "+1-062-940-6504",
                        Email = "wellssherry@gmail.com",
                        Password = "X^5t8UJvx%ir".Hash(),
                        Position = "UX/UI Designer",
                        BirthDay = DateTime.Parse("1971-10-30"),
                    team = teams[5]
                    ,Projects = [project1, project2]
                    },

                    // 15
                    new Employees{
                        Manager = true,
                        FirstName = "Latasha",
                        LastName = "Hughes",
                        PhoneNumber = "+1-043-631-6540",
                        Email = "alexandercampbell@yahoo.com",
                        Password = "Uf_ZsHBF+6JF".Hash(),
                        Position = "UX/UI Designer",
                        BirthDay = DateTime.Parse("1982-04-06"),
                        team = teams[5]
                        ,Projects = [project1, project2]
                    },

                    // 16
                    new Employees{
                        Manager = true,
                        FirstName = "Thamer",
                        LastName = "Almusa",
                        PhoneNumber = "966536919438",
                        Email = "ThamerAlM@yahoo.com",
                        Password = "cjefn+6JF".Hash(),
                        Position = "UX/UI Designer intern",
                        BirthDay = DateTime.Parse("2005-04-06"),
                        team = teams[5]
                        ,Projects = [project1]
                    },

                    // 17
                    new Employees{
                        Manager = false,
                        FirstName = "Derek",
                        LastName = "Atkinson",
                        PhoneNumber = "(616)596-1471x036",
                        Email = "zacharywhite@yahoo.com",
                        Password = ")jaAk$eSS39o".Hash(),
                        Position = "FrontEnd Devoleper",
                        BirthDay = DateTime.Parse("1978-06-22"),
                        team= teams[2]
                        ,Projects = [project1, project2]
                    },
                    // 18
                    new Employees{
                        Manager = false,
                        FirstName = "Debbie",
                        LastName = "Wong",
                        PhoneNumber = "001-177-751-6593x9719",
                        Email = "qjohnson@velez.com",
                        Password = "f8L$67m4^8ge".Hash(),
                        Position = "FrontEnd Devoleper",
                        BirthDay = DateTime.Parse("1998-06-02"),
                        team= teams[2]
                        ,Projects = [project1, project2]
                    },
                    
                    // 19
                    new Employees{
                        Manager = true,
                        FirstName = "Derek",
                        LastName = "Davis",
                        PhoneNumber = "410-617-6432",
                        Email = "tylerfigueroa@gmail.com",
                        Password = ")A+$Rb8mrIk0".Hash(),
                        Position = "DevOps manager",
                        BirthDay = DateTime.Parse("1993-08-05"),
                        team= teams[3]
                        ,Projects = [project1, project2]


                    },
                    
                    // 20
                    new Employees{
                        Manager = false,
                        FirstName = "Alexander",
                        LastName = "Atkinson",
                        PhoneNumber = "001-809-891-8493x589",
                        Email = "thomas88@yahoo.com",
                        Password = ")63Se+y4Y_$I".Hash(),
                        Position = "DevOps Enginner",
                        BirthDay = DateTime.Parse("1991-01-29"),
                        team= teams[3]
                        ,Projects = [project1, project2]
                    },

                    // 21
                    new Employees{
                        Manager = false,
                        FirstName = "Christian",
                        LastName = "Reed",
                        PhoneNumber = "+1-383-022-5504x6297",
                        Email = "andrew47@rodriguez.biz",
                        Password = "j4EaHwdD&3_)".Hash(),
                        Position = "DevOps Engineer",
                        BirthDay = DateTime.Parse("1989-11-27"),
                        team= teams[3]
                        ,Projects = [project1]
                    },

                    // 22
                    new Employees{
                        Manager = false,
                        FirstName = "Nawaf",
                        LastName = "Alqahtani",
                        PhoneNumber = "+1-383-022-5504x6297",
                        Email = "NawafQa@rodriguez.biz",
                        Password = "kmjvofv*Y&&*BHJ".Hash(),
                        Position = "DevOps intern",
                        BirthDay = DateTime.Parse("1999-10-27"),
                        team= teams[3]
                        ,Projects = [project1]
                    },

                    // 23
                    new Employees{
                        Manager = false,
                        FirstName = "David",
                        LastName = "Vang",
                        PhoneNumber = "224.359.9233x0935",
                        Email = "danielsmichelle@gmail.com",
                        Password = "tpLieBwO^3cr".Hash(),
                        Position = "Marketing Manager",
                        BirthDay = DateTime.Parse("1965-03-10"),
                        team = teams[6]
                        ,Projects = [project1, project2]
                    },

                    // 24
                    new Employees{
                        Manager = true,
                        FirstName = "Christine",
                        LastName = "Li",
                        PhoneNumber = "603.713.0905x5256",
                        Email = "kristen91@gonzalez.com",
                        Password = "2iBY%TYk!f37".Hash(),
                        Position = "Marketer",
                        BirthDay = DateTime.Parse("1980-08-03"),
                        team = teams[6]
                        ,Projects = [project1, project2]
                    },
                    
                    // 25
                    new Employees{
                        Manager = false,
                        FirstName = "Anthony",
                        LastName = "Combs",
                        PhoneNumber = "+1-643-745-7768x4384",
                        Email = "david57@ellis.info",
                        Password = "k^1sUtq!Ij$*".Hash(),
                        Position = "Marketer intern",
                        BirthDay = DateTime.Parse("1994-03-30"),
                        team = teams[6]
                        ,Projects = [project1]
                    },

                    // 26
                    new Employees{
                        Manager = true,
                        FirstName = "Jennifer",
                        LastName = "Dominguez",
                        PhoneNumber = "001-579-206-3677",
                        Email = "bkane@yahoo.com",
                        Password = "a*8(qfjIctfQ".Hash(),
                        Position = "customer support specialist",
                        BirthDay = DateTime.Parse("1962-12-18"),
                        team = teams[7]
                        ,Projects = [project1, project2]
                    },

                    // 27
                    new Employees{
                        Manager = false,
                        FirstName = "Angela",
                        LastName = "Duran",
                        PhoneNumber = "997-179-7617x054",
                        Email = "ugriffin@gmail.com",
                        Password = "#*2FgbQJroTv".Hash(),
                        Position = "customer support employee",
                        BirthDay = DateTime.Parse("1983-07-19"),
                        team = teams[7]
                        ,Projects = [project1, project2]
                    },

                    // 28
                    new Employees{
                        Manager = true,
                        FirstName = "Donald",
                        LastName = "Grant",
                        PhoneNumber = "2509536011",
                        Email = "sarahburch@hotmail.com",
                        Password = "@04BzGV#MzZj".Hash(),
                        Position = "customer support intern",
                        BirthDay = DateTime.Parse("1993-07-28"),
                        team = teams[7]
                        ,Projects = [project1]
                    },

                    // 29
                    new Employees{
                        Manager = false,
                        FirstName = "Tricia",
                        LastName = "Brown",
                        PhoneNumber = "089-304-6934",
                        Email = "dwhite@wolfe.biz",
                        Password = "vPp89DxYs(W4".Hash(),
                        Position = "customer support intern",
                        BirthDay = DateTime.Parse("1966-07-20"),
                        team = teams[7]
                        ,Projects = [project1]
                    }
                    ,
                    // testing account
                    new Employees{
                        Manager = true,
                        FirstName = "Turki",
                        LastName = "Alothman",
                        PhoneNumber = "966536919403",
                        Email = "toto--1-@hotmail.com",
                        Password = "12345678".Hash(),
                        Position = "customer support intern",
                        BirthDay = DateTime.Parse("2001-07-15"),
                        team = teams[1]
                        ,Projects = [project1]
                    }
                };

                
                project1.Tasks = new List<Tasks> {
                    
                    // project1
                    // Quality assurance tasks
                    
                    new Tasks{
                        Title = "Test Plan Creation",
                        StartDate = DateTime.Parse("2024-03-01"),
                        DueDate = DateTime.Parse("2024-03-05"),
                        Priority = "High",
                        Description = "Create detailed test plans for the new project features.",
                        Status = "Not Started",
                        Type="Quality assurance",
                        Asignees = [employess[0]],
                        Reporter =employess[4]

                    },

                    new Tasks{
                        Title = "Automated Test Development",
                        StartDate = DateTime.Parse("2024-03-06"),
                        DueDate = DateTime.Parse("2024-03-12"),
                        Priority = "High",
                        Description = "Develop automated tests for continuous integration.",
                        Status = "In Progress",
                        Type="Quality assurance",
                        Asignees = [employess[0]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Regression Testing",
                        StartDate = DateTime.Parse("2024-03-13"),
                        DueDate = DateTime.Parse("2024-03-19"),
                        Priority = "Medium",
                        Description = "Conduct regression testing for the latest release.",
                        Status = "Not Started",
                        Type="Quality assurance",
                        Asignees = [employess[0]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Performance Testing",
                        StartDate = DateTime.Parse("2024-03-20"),
                        DueDate = DateTime.Parse("2024-03-26"),
                        Priority = "Medium",
                        Description = "Perform performance testing on critical features.",
                        Status = "Not Started",
                        Type="Quality assurance",
                        Asignees = [employess[0]],
                        Reporter =employess[13]
                    },
                    
                    new Tasks{
                        Title = "Security Testing",
                        StartDate = DateTime.Parse("2024-03-27"),
                        DueDate = DateTime.Parse("2024-04-02"),
                        Priority = "High",
                        Description = "Execute comprehensive security testing protocols.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[0]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Test Report Compilation",
                        StartDate = DateTime.Parse("2024-04-03"),
                        DueDate = DateTime.Parse("2024-04-09"),
                        Priority = "Low",
                        Description = "Compile testing reports and document findings.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[1]],
                        Reporter =employess[11]

                    },

                    new Tasks{
                        Title = "User Acceptance Testing Coordination",
                        StartDate = DateTime.Parse("2024-04-10"),
                        DueDate = DateTime.Parse("2024-04-16"),
                        Priority = "Medium",
                        Description = "Coordinate user acceptance testing with stakeholders.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[1]],
                        Reporter =employess[12]

                    },

                    new Tasks{
                        Title = "Bug Triage Meetings",
                        StartDate = DateTime.Parse("2024-04-17"),
                        DueDate = DateTime.Parse("2024-04-23"),
                        Priority = "High",
                        Description = "Lead bug triage meetings to prioritize bug fixes.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[1]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Testing Tools Evaluation",
                        StartDate = DateTime.Parse("2024-04-24"),
                        DueDate = DateTime.Parse("2024-04-30"),
                        Priority = "Low",
                        Description = "Evaluate new testing tools and processes for efficiency.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[1]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Quality Assurance Process Review",
                        StartDate = DateTime.Parse("2024-05-01"),
                        DueDate = DateTime.Parse("2024-05-07"),
                        Priority = "Medium",
                        Description = "Review and update the quality assurance processes.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[1]],
                        Reporter =employess[11]
                    },

                                        new Tasks{
                        Title = "Accessibility Testing",
                        StartDate = DateTime.Parse("2024-05-08"),
                        DueDate = DateTime.Parse("2024-05-14"),
                        Priority = "Medium",
                        Description = "Ensure software compliance with accessibility standards.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[2]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Cross-Browser Testing",
                        StartDate = DateTime.Parse("2024-05-15"),
                        DueDate = DateTime.Parse("2024-05-21"),
                        Priority = "High",
                        Description = "Test application compatibility across multiple browsers.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[2]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "API Testing",
                        StartDate = DateTime.Parse("2024-05-22"),
                        DueDate = DateTime.Parse("2024-05-28"),
                        Priority = "High",
                        Description = "Validate the functionality, reliability, performance, and security of the application's API.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[2]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Load Testing",
                        StartDate = DateTime.Parse("2024-05-29"),
                        DueDate = DateTime.Parse("2024-06-04"),
                        Priority = "Medium",
                        Description = "Simulate peak load conditions to test system performance and stability.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[2]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Usability Testing",
                        StartDate = DateTime.Parse("2024-06-05"),
                        DueDate = DateTime.Parse("2024-06-11"),
                        Priority = "Medium",
                        Description = "Evaluate the application for user friendliness and ease of use.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[2]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Localization Testing",
                        StartDate = DateTime.Parse("2024-06-12"),
                        DueDate = DateTime.Parse("2024-06-18"),
                        Priority = "Low",
                        Description = "Check the software for adaptability in different languages and regions.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[3]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Integration Testing",
                        StartDate = DateTime.Parse("2024-06-19"),
                        DueDate = DateTime.Parse("2024-06-25"),
                        Priority = "High",
                        Description = "Test the integration points between the application modules.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[3]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Security Certification Preparation",
                        StartDate = DateTime.Parse("2024-06-26"),
                        DueDate = DateTime.Parse("2024-07-02"),
                        Priority = "High",
                        Description = "Prepare the application for security certification assessments.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[3]]
                        ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Continuous Testing Pipeline Enhancement",
                        StartDate = DateTime.Parse("2024-07-03"),
                        DueDate = DateTime.Parse("2024-07-09"),
                        Priority = "Medium",
                        Description = "Improve the efficiency and coverage of the continuous testing pipeline.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[3]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Test Case Review and Optimization",
                        StartDate = DateTime.Parse("2024-07-10"),
                        DueDate = DateTime.Parse("2024-07-16"),
                        Priority = "Low",
                        Description = "Review and optimize existing test cases for better efficiency.",
                        Status = "Planned",
                        Type="Quality assurance",
                        Asignees = [employess[3]],
                        Reporter =employess[14]
                    },

                    // backend tasks
                    new Tasks{
                        Title = "Database Schema Design",
                        StartDate = DateTime.Parse("2024-01-01"),
                        DueDate = DateTime.Parse("2024-01-07"),
                        Priority = "High",
                        Description = "Design and review database schema for the new inventory system.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[6]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Authentication Service Update",
                        StartDate = DateTime.Parse("2024-01-08"),
                        DueDate = DateTime.Parse("2024-01-14"),
                        Priority = "High",
                        Description = "Update the authentication service to support OAuth 2.0.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[6]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "API Rate Limiting Implementation",
                        StartDate = DateTime.Parse("2024-01-15"),
                        DueDate = DateTime.Parse("2024-01-21"),
                        Priority = "Medium",
                        Description = "Implement rate limiting on public APIs to enhance security.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[6]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Logging Framework Integration",
                        StartDate = DateTime.Parse("2024-01-22"),
                        DueDate = DateTime.Parse("2024-01-28"),
                        Priority = "Medium",
                        Description = "Integrate a new logging framework for better traceability.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[6]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Payment Gateway Integration",
                        StartDate = DateTime.Parse("2024-01-29"),
                        DueDate = DateTime.Parse("2024-02-04"),
                        Priority = "High",
                        Description = "Integrate Stripe payment gateway for processing transactions.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[6]],
                        Reporter =employess[14]
                    },

                    new Tasks{
                        Title = "Redis Caching Layer Addition",
                        StartDate = DateTime.Parse("2024-02-05"),
                        DueDate = DateTime.Parse("2024-02-11"),
                        Priority = "Medium",
                        Description = "Add Redis caching to improve application performance.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[7]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Microservices Architecture Migration",
                        StartDate = DateTime.Parse("2024-02-12"),
                        DueDate = DateTime.Parse("2024-02-18"),
                        Priority = "High",
                        Description = "Migrate monolithic architecture to microservices for scalability.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[7]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Dockerization of Services",
                        StartDate = DateTime.Parse("2024-02-19"),
                        DueDate = DateTime.Parse("2024-02-25"),
                        Priority = "Medium",
                        Description = "Dockerize existing services for better deployment efficiency.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[7]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Automated Backup System",
                        StartDate = DateTime.Parse("2024-02-26"),
                        DueDate = DateTime.Parse("2024-03-04"),
                        Priority = "Low",
                        Description = "Implement an automated backup system for disaster recovery.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[7]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "GraphQL Endpoint Implementation",
                        StartDate = DateTime.Parse("2024-03-05"),
                        DueDate = DateTime.Parse("2024-03-11"),
                        Priority = "High",
                        Description = "Implement GraphQL endpoints for more flexible data retrieval.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[7]],
                        Reporter =employess[11]

                    },
                    new Tasks{
                        Title = "Continuous Integration Pipeline Setup",
                        StartDate = DateTime.Parse("2024-03-12"),
                        DueDate = DateTime.Parse("2024-03-18"),
                        Priority = "High",
                        Description = "Set up a CI pipeline for automated testing and deployment.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[8]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Service Monitoring Implementation",
                        StartDate = DateTime.Parse("2024-03-19"),
                        DueDate = DateTime.Parse("2024-03-25"),
                        Priority = "Medium",
                        Description = "Implement monitoring for backend services to ensure uptime and performance.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[8]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Database Optimization",
                        StartDate = DateTime.Parse("2024-03-26"),
                        DueDate = DateTime.Parse("2024-04-01"),
                        Priority = "High",
                        Description = "Optimize database queries and indexes for performance improvement.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[8]],
                        Reporter =employess[14]
                    },

                    new Tasks{
                        Title = "Error Handling Framework",
                        StartDate = DateTime.Parse("2024-04-02"),
                        DueDate = DateTime.Parse("2024-04-08"),
                        Priority = "Medium",
                        Description = "Develop a comprehensive error handling framework for the backend.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[8]]
                        ,
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "API Documentation",
                        StartDate = DateTime.Parse("2024-04-09"),
                        DueDate = DateTime.Parse("2024-04-15"),
                        Priority = "Medium",
                        Description = "Create detailed API documentation for developers and API consumers.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[8]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Data Encryption Implementation",
                        StartDate = DateTime.Parse("2024-04-16"),
                        DueDate = DateTime.Parse("2024-04-22"),
                        Priority = "High",
                        Description = "Implement data encryption for sensitive user data storage and transfer.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[9]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Third-Party API Integration",
                        StartDate = DateTime.Parse("2024-04-23"),
                        DueDate = DateTime.Parse("2024-04-29"),
                        Priority = "Medium",
                        Description = "Integrate third-party APIs for enhanced functionality.",
                        Status = "Planned",
                        Type="backend",
                        
                        Asignees = [employess[9]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Backend Refactoring",
                        StartDate = DateTime.Parse("2024-04-30"),
                        DueDate = DateTime.Parse("2024-05-06"),
                        Priority = "Medium",
                        Description = "Refactor existing backend code to improve maintainability and scalability.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[9]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "User Authentication Enhancements",
                        StartDate = DateTime.Parse("2024-05-07"),
                        DueDate = DateTime.Parse("2024-05-13"),
                        Priority = "High",
                        Description = "Enhance user authentication mechanisms to include multi-factor authentication.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[9]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Serverless Architecture Exploration",
                        StartDate = DateTime.Parse("2024-05-14"),
                        DueDate = DateTime.Parse("2024-05-20"),
                        Priority = "Low",
                        Description = "Explore the adoption of serverless architecture for specific microservices.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[9]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Kubernetes Deployment",
                        StartDate = DateTime.Parse("2024-05-21"),
                        DueDate = DateTime.Parse("2024-05-27"),
                        Priority = "Medium",
                        Description = "Deploy backend services on Kubernetes for orchestration.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[10]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Cloud Services Cost Optimization",
                        StartDate = DateTime.Parse("2024-05-28"),
                        DueDate = DateTime.Parse("2024-06-03"),
                        Priority = "Medium",
                        Description = "Optimize cloud services usage to reduce costs while maintaining performance.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[10]]
                        ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Backend Security Audit",
                        StartDate = DateTime.Parse("2024-06-04"),
                        DueDate = DateTime.Parse("2024-06-10"),
                        Priority = "High",
                        Description = "Conduct a security audit to identify and fix potential vulnerabilities.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[10]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "WebSockets Implementation",
                        StartDate = DateTime.Parse("2024-06-11"),
                        DueDate = DateTime.Parse("2024-06-17"),
                        Priority = "Medium",
                        Description = "Implement WebSockets for real-time data exchange.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[10]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Automated Database Migrations",
                        StartDate = DateTime.Parse("2024-06-18"),
                        DueDate = DateTime.Parse("2024-06-24"),
                        Priority = "Low",
                        Description = "Set up automated database migration scripts for seamless deployments.",
                        Status = "Planned",
                        Type="backend",
                        Asignees = [employess[10]],
                        Reporter =employess[12]
                    },

                    // UI-UX designer
                    new Tasks{
                        Title = "User Research for New App",
                        StartDate = DateTime.Parse("2024-01-01"),
                        DueDate = DateTime.Parse("2024-01-07"),
                        Priority = "High",
                        Description = "Conduct user research to gather insights for the new mobile application design.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[14]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Design Sprints for Feature X",
                        StartDate = DateTime.Parse("2024-01-08"),
                        DueDate = DateTime.Parse("2024-01-14"),
                        Priority = "High",
                        Description = "Run design sprints to prototype Feature X for user testing.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[14]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Create Wireframes for Dashboard",
                        StartDate = DateTime.Parse("2024-01-15"),
                        DueDate = DateTime.Parse("2024-01-21"),
                        Priority = "Medium",
                        Description = "Develop wireframes for the new dashboard design.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[14]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Develop UI Kit",
                        StartDate = DateTime.Parse("2024-01-22"),
                        DueDate = DateTime.Parse("2024-01-28"),
                        Priority = "Medium",
                        Description = "Create a UI kit for consistent design across all platforms.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[14]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "User Flow Analysis",
                        StartDate = DateTime.Parse("2024-01-29"),
                        DueDate = DateTime.Parse("2024-02-04"),
                        Priority = "High",
                        Description = "Analyze and optimize user flows for the checkout process.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[14]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Accessibility Review",
                        StartDate = DateTime.Parse("2024-02-05"),
                        DueDate = DateTime.Parse("2024-02-11"),
                        Priority = "Medium",
                        Description = "Review and enhance the accessibility of web applications.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[15]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Interactive Prototyping",
                        StartDate = DateTime.Parse("2024-02-12"),
                        DueDate = DateTime.Parse("2024-02-18"),
                        Priority = "High",
                        Description = "Create interactive prototypes for the new feature set.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[15]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Design System Update",
                        StartDate = DateTime.Parse("2024-02-19"),
                        DueDate = DateTime.Parse("2024-02-25"),
                        Priority = "Medium",
                        Description = "Update the design system with new components and guidelines.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[15]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "User Testing Session Setup",
                        StartDate = DateTime.Parse("2024-02-26"),
                        DueDate = DateTime.Parse("2024-03-04"),
                        Priority = "High",
                        Description = "Set up user testing sessions for the latest prototype.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[15]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Competitive Analysis Report",
                        StartDate = DateTime.Parse("2024-03-05"),
                        DueDate = DateTime.Parse("2024-03-11"),
                        Priority = "Low",
                        Description = "Conduct a competitive analysis to benchmark UX practices.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[15]]
                        ,
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Persona Development",
                        StartDate = DateTime.Parse("2024-03-12"),
                        DueDate = DateTime.Parse("2024-03-18"),
                        Priority = "Medium",
                        Description = "Develop user personas for targeted design strategies.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[16]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "UX Workshop Facilitation",
                        StartDate = DateTime.Parse("2024-03-19"),
                        DueDate = DateTime.Parse("2024-03-25"),
                        Priority = "Medium",
                        Description = "Facilitate a UX workshop for the product team.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[16]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Mobile App Redesign",
                        StartDate = DateTime.Parse("2024-03-26"),
                        DueDate = DateTime.Parse("2024-04-01"),
                        Priority = "High",
                        Description = "Lead the redesign of the mobile app for enhanced user experience.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[16]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Landing Page Design",
                        StartDate = DateTime.Parse("2024-04-02"),
                        DueDate = DateTime.Parse("2024-04-08"),
                        Priority = "High",
                        Description = "Design a new landing page to increase conversions.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[16]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Brand Identity Refresh",
                        StartDate = DateTime.Parse("2024-04-09"),
                        DueDate = DateTime.Parse("2024-04-15"),
                        Priority = "Medium",
                        Description = "Refresh the brand identity to align with the new company vision.",
                        Status = "Planned",
                        Type="UI-UX",
                        Asignees = [employess[16]],
                        Reporter =employess[4]
                    },

                    // frontend devoleper
                    new Tasks{
                        Title = "Landing Page Redesign",
                        StartDate = DateTime.Parse("2024-01-01"),
                        DueDate = DateTime.Parse("2024-01-08"),
                        Priority = "High",
                        Description = "Redesign the landing page to improve user engagement and conversion rates.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[17]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "React Component Library Development",
                        StartDate = DateTime.Parse("2024-01-09"),
                        DueDate = DateTime.Parse("2024-01-16"),
                        Priority = "Medium",
                        Description = "Develop a reusable component library using React for faster development cycles.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[17]]
                        ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Accessibility Audit and Improvements",
                        StartDate = DateTime.Parse("2024-01-17"),
                        DueDate = DateTime.Parse("2024-01-24"),
                        Priority = "High",
                        Description = "Perform an accessibility audit and implement improvements to meet WCAG standards.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[17]]
                        ,
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Implement Responsive Design",
                        StartDate = DateTime.Parse("2024-01-25"),
                        DueDate = DateTime.Parse("2024-02-01"),
                        Priority = "High",
                        Description = "Ensure all new web pages are fully responsive across devices.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[17]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Performance Optimization",
                        StartDate = DateTime.Parse("2024-02-02"),
                        DueDate = DateTime.Parse("2024-02-09"),
                        Priority = "Medium",
                        Description = "Optimize website performance to achieve a sub-2-second load time.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[17]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "CSS Refactoring",
                        StartDate = DateTime.Parse("2024-02-10"),
                        DueDate = DateTime.Parse("2024-02-17"),
                        Priority = "Low",
                        Description = "Refactor CSS to improve maintainability and reduce stylesheet size.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[18]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Integrate New API Endpoints",
                        StartDate = DateTime.Parse("2024-02-18"),
                        DueDate = DateTime.Parse("2024-02-25"),
                        Priority = "Medium",
                        Description = "Integrate frontend with new backend API endpoints for enhanced functionality.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[18]]
                        ,
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Frontend Security Enhancements",
                        StartDate = DateTime.Parse("2024-02-26"),
                        DueDate = DateTime.Parse("2024-03-05"),
                        Priority = "High",
                        Description = "Implement security best practices to protect against XSS and CSRF attacks.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[18]]
                        ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Single Page Application (SPA) Routing",
                        StartDate = DateTime.Parse("2024-03-06"),
                        DueDate = DateTime.Parse("2024-03-13"),
                        Priority = "Medium",
                        Description = "Implement client-side routing for the SPA to improve user experience.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[18]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "UI Testing Automation",
                        StartDate = DateTime.Parse("2024-03-14"),
                        DueDate = DateTime.Parse("2024-03-21"),
                        Priority = "Medium",
                        Description = "Set up automated UI testing to ensure frontend stability and reduce bugs.",
                        Status = "Planned",
                        Type="frontend",
                        Asignees = [employess[18]]
                        ,
                        Reporter =employess[4]
                    },

                    // DevOps Tasks
                    new Tasks{
                        Title = "CI/CD Pipeline Setup",
                        StartDate = DateTime.Parse("2024-01-01"),
                        DueDate = DateTime.Parse("2024-01-15"),
                        Priority = "High",
                        Description = "Establish a Continuous Integration/Continuous Deployment pipeline to automate the software release process.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[19]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Infrastructure as Code Implementation",
                        StartDate = DateTime.Parse("2024-01-16"),
                        DueDate = DateTime.Parse("2024-01-31"),
                        Priority = "High",
                        Description = "Implement Infrastructure as Code (IaC) using Terraform to manage and provision infrastructure through code.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[19]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Containerization with Docker",
                        StartDate = DateTime.Parse("2024-02-01"),
                        DueDate = DateTime.Parse("2024-02-14"),
                        Priority = "Medium",
                        Description = "Containerize applications using Docker to enhance portability and consistency across environments.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[19]]
                    ,
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Kubernetes Cluster Configuration",
                        StartDate = DateTime.Parse("2024-02-15"),
                        DueDate = DateTime.Parse("2024-03-01"),
                        Priority = "High",
                        Description = "Configure and manage a Kubernetes cluster for orchestrating containerized applications.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[19]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Logging and Monitoring Setup",
                        StartDate = DateTime.Parse("2024-03-02"),
                        DueDate = DateTime.Parse("2024-03-16"),
                        Priority = "Medium",
                        Description = "Set up logging and monitoring solutions to track application performance and troubleshoot issues.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[19]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Cloud Resource Optimization",
                        StartDate = DateTime.Parse("2024-03-17"),
                        DueDate = DateTime.Parse("2024-03-31"),
                        Priority = "Medium",
                        Description = "Analyze and optimize cloud resource usage to reduce costs without compromising performance.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[20]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Security Best Practices Implementation",
                        StartDate = DateTime.Parse("2024-04-01"),
                        DueDate = DateTime.Parse("2024-04-15"),
                        Priority = "High",
                        Description = "Implement security best practices and tools to safeguard the infrastructure and applications.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[20]]
                    ,
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Automated Backup Strategies",
                        StartDate = DateTime.Parse("2024-04-16"),
                        DueDate = DateTime.Parse("2024-04-30"),
                        Priority = "Low",
                        Description = "Develop and implement automated backup strategies for disaster recovery and data retention.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[20]]
                    ,
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Serverless Architecture Deployment",
                        StartDate = DateTime.Parse("2024-05-01"),
                        DueDate = DateTime.Parse("2024-05-15"),
                        Priority = "Medium",
                        Description = "Deploy serverless architecture components to improve scalability and reduce operational costs.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[20]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Performance Tuning and Stress Testing",
                        StartDate = DateTime.Parse("2024-05-16"),
                        DueDate = DateTime.Parse("2024-05-30"),
                        Priority = "Medium",
                        Description = "Conduct performance tuning and stress testing to ensure the system's reliability under high load.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[20]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Automate Database Migrations",
                        StartDate = DateTime.Parse("2024-06-01"),
                        DueDate = DateTime.Parse("2024-06-15"),
                        Priority = "Medium",
                        Description = "Automate the database migration process to ensure seamless transitions between versions.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[21]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Implement Zero Downtime Deployments",
                        StartDate = DateTime.Parse("2024-06-16"),
                        DueDate = DateTime.Parse("2024-06-30"),
                        Priority = "High",
                        Description = "Implement strategies for zero downtime deployments to enhance user experience.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[21]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Secure Secrets Management",
                        StartDate = DateTime.Parse("2024-07-01"),
                        DueDate = DateTime.Parse("2024-07-15"),
                        Priority = "High",
                        Description = "Set up a secure secrets management tool to handle sensitive information like passwords and API keys.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[21]]
                    ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Cloud Network Configuration",
                        StartDate = DateTime.Parse("2024-07-16"),
                        DueDate = DateTime.Parse("2024-07-31"),
                        Priority = "Medium",
                        Description = "Configure cloud network settings to optimize performance and security.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[21]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Automated Security Scanning",
                        StartDate = DateTime.Parse("2024-08-01"),
                        DueDate = DateTime.Parse("2024-08-15"),
                        Priority = "Medium",
                        Description = "Implement automated security scanning within the CI/CD pipeline to detect vulnerabilities early.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[21]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Infrastructure Cost Analysis",
                        StartDate = DateTime.Parse("2024-08-16"),
                        DueDate = DateTime.Parse("2024-08-31"),
                        Priority = "Low",
                        Description = "Perform a thorough cost analysis of the current infrastructure and identify areas for cost reduction.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[22]]
                    ,
                        Reporter =employess[14]
                    },

                    new Tasks{
                        Title = "Build Artifacts Repository Setup",
                        StartDate = DateTime.Parse("2024-09-01"),
                        DueDate = DateTime.Parse("2024-09-15"),
                        Priority = "Medium",
                        Description = "Set up a build artifacts repository to store and manage build artifacts efficiently.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[22]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Disaster Recovery Plan",
                        StartDate = DateTime.Parse("2024-09-16"),
                        DueDate = DateTime.Parse("2024-09-30"),
                        Priority = "High",
                        Description = "Develop and document a comprehensive disaster recovery plan for critical services.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[22]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Internal Developer Platform Enhancement",
                        StartDate = DateTime.Parse("2024-10-01"),
                        DueDate = DateTime.Parse("2024-10-15"),
                        Priority = "Medium",
                        Description = "Enhance the internal developer platform to streamline development workflows.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[22]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Compliance Auditing Automation",
                        StartDate = DateTime.Parse("2024-10-16"),
                        DueDate = DateTime.Parse("2024-10-31"),
                        Priority = "Low",
                        Description = "Automate compliance auditing processes to ensure continuous compliance with industry standards.",
                        Status = "Planned",
                        Type="DevOps",
                    Asignees = [employess[22]]
                    ,
                        Reporter =employess[12]
                    },

                    // Marketing Tasks
                    new Tasks{
                        Title = "SEO Strategy Development",
                        StartDate = DateTime.Parse("2024-01-01"),
                        DueDate = DateTime.Parse("2024-01-15"),
                        Priority = "High",
                        Description = "Develop a comprehensive SEO strategy to improve organic search rankings.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[23]]
                    ,
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Content Marketing Plan",
                        StartDate = DateTime.Parse("2024-01-16"),
                        DueDate = DateTime.Parse("2024-01-31"),
                        Priority = "Medium",
                        Description = "Create a content marketing plan to support SEO goals and user engagement.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[23]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Social Media Campaign Launch",
                        StartDate = DateTime.Parse("2024-02-01"),
                        DueDate = DateTime.Parse("2024-02-14"),
                        Priority = "High",
                        Description = "Launch a social media campaign to increase brand awareness and engagement.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[23]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Email Marketing Automation",
                        StartDate = DateTime.Parse("2024-02-15"),
                        DueDate = DateTime.Parse("2024-02-28"),
                        Priority = "Medium",
                        Description = "Implement email marketing automation to nurture leads and convert subscribers.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[23]]
                    ,
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "PPC Campaign Optimization",
                        StartDate = DateTime.Parse("2024-03-01"),
                        DueDate = DateTime.Parse("2024-03-15"),
                        Priority = "High",
                        Description = "Optimize PPC campaigns to improve click-through and conversion rates.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[23]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Market Research for Product Launch",
                        StartDate = DateTime.Parse("2024-03-16"),
                        DueDate = DateTime.Parse("2024-03-31"),
                        Priority = "High",
                        Description = "Conduct market research to inform the strategy for an upcoming product launch.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[24]]
                    ,
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Influencer Partnership Program",
                        StartDate = DateTime.Parse("2024-04-01"),
                        DueDate = DateTime.Parse("2024-04-15"),
                        Priority = "Medium",
                        Description = "Develop an influencer partnership program to extend brand reach.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[24]]
                    ,
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Customer Feedback Survey",
                        StartDate = DateTime.Parse("2024-04-16"),
                        DueDate = DateTime.Parse("2024-04-30"),
                        Priority = "Low",
                        Description = "Deploy a customer feedback survey to gather insights for product improvements.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[24]]
                    ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Brand Messaging Refresh",
                        StartDate = DateTime.Parse("2024-05-01"),
                        DueDate = DateTime.Parse("2024-05-15"),
                        Priority = "Medium",
                        Description = "Refresh the brand messaging to align with current market positioning.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[24]]
                    ,
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Website UX/UI Overhaul",
                        StartDate = DateTime.Parse("2024-05-16"),
                        DueDate = DateTime.Parse("2024-05-31"),
                        Priority = "High",
                        Description = "Overhaul the website's UX/UI to improve user experience and increase engagement.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[24]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Product Video Production",
                        StartDate = DateTime.Parse("2024-06-01"),
                        DueDate = DateTime.Parse("2024-06-15"),
                        Priority = "Medium",
                        Description = "Produce a product video to highlight features and benefits for marketing channels.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[25]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Trade Show Preparation",
                        StartDate = DateTime.Parse("2024-06-16"),
                        DueDate = DateTime.Parse("2024-06-30"),
                        Priority = "High",
                        Description = "Prepare for trade show participation, including booth design and promotional materials.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[25]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Customer Case Study Development",
                        StartDate = DateTime.Parse("2024-07-01"),
                        DueDate = DateTime.Parse("2024-07-15"),
                        Priority = "Medium",
                        Description = "Develop customer case studies to showcase success stories and build trust.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[25]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Analytics and Reporting System Setup",
                        StartDate = DateTime.Parse("2024-07-16"),
                        DueDate = DateTime.Parse("2024-07-31"),
                        Priority = "Medium",
                        Description = "Set up a comprehensive analytics and reporting system for marketing performance tracking.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[25]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Rebranding Strategy Session",
                        StartDate = DateTime.Parse("2024-08-01"),
                        DueDate = DateTime.Parse("2024-08-15"),
                        Priority = "Low",
                        Description = "Conduct a strategy session to explore potential rebranding efforts.",
                        Status = "Planned",
                        Type="Marketing",
                    Asignees = [employess[25]],
                        Reporter =employess[12]
                    },


                    // customer support team
                    new Tasks{
                        Title = "Implement New Ticketing System",
                        StartDate = DateTime.Parse("2024-01-01"),
                        DueDate = DateTime.Parse("2024-01-10"),
                        Priority = "High",
                        Description = "Deploy and configure a new ticketing system to improve issue tracking and response times.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[26]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Customer Satisfaction Survey",
                        StartDate = DateTime.Parse("2024-01-11"),
                        DueDate = DateTime.Parse("2024-01-20"),
                        Priority = "Medium",
                        Description = "Conduct a customer satisfaction survey to gather feedback on support interactions.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[26]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Knowledge Base Overhaul",
                        StartDate = DateTime.Parse("2024-01-21"),
                        DueDate = DateTime.Parse("2024-02-01"),
                        Priority = "High",
                        Description = "Revise and expand the knowledge base to provide more comprehensive self-service resources.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[26]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Support Chatbot Implementation",
                        StartDate = DateTime.Parse("2024-02-02"),
                        DueDate = DateTime.Parse("2024-02-15"),
                        Priority = "Medium",
                        Description = "Implement a chatbot to offer instant support for common issues and questions.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[26]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Agent Training Program",
                        StartDate = DateTime.Parse("2024-02-16"),
                        DueDate = DateTime.Parse("2024-03-01"),
                        Priority = "High",
                        Description = "Develop and execute a training program for support agents to enhance service quality.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[26]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Weekly Support Team Meetings",
                        StartDate = DateTime.Parse("2024-03-02"),
                        DueDate = DateTime.Parse("2024-03-09"),
                        Priority = "Low",
                        Description = "Establish weekly meetings to discuss ongoing issues, feedback, and strategies for improvement.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[27]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Customer Onboarding Process Improvement",
                        StartDate = DateTime.Parse("2024-03-10"),
                        DueDate = DateTime.Parse("2024-03-20"),
                        Priority = "Medium",
                        Description = "Revise the customer onboarding process to ensure a smoother start and reduce early-stage tickets.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[27]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Monthly Performance Review",
                        StartDate = DateTime.Parse("2024-03-21"),
                        DueDate = DateTime.Parse("2024-03-31"),
                        Priority = "Low",
                        Description = "Initiate monthly performance reviews to assess ticket resolution efficiency and customer feedback.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[27]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Support Workflow Automation",
                        StartDate = DateTime.Parse("2024-04-01"),
                        DueDate = DateTime.Parse("2024-04-15"),
                        Priority = "Medium",
                        Description = "Implement workflow automation tools to streamline ticket routing and resolution processes.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[27]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Emergency Response Protocol",
                        StartDate = DateTime.Parse("2024-04-16"),
                        DueDate = DateTime.Parse("2024-04-30"),
                        Priority = "High",
                        Description = "Develop an emergency response protocol for handling critical support issues.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[27]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Refine Auto-Response Templates",
                        StartDate = DateTime.Parse("2024-05-01"),
                        DueDate = DateTime.Parse("2024-05-07"),
                        Priority = "Medium",
                        Description = "Update and refine auto-response email templates to provide more personalized and helpful initial responses.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[28]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Cross-Training Support Agents",
                        StartDate = DateTime.Parse("2024-05-08"),
                        DueDate = DateTime.Parse("2024-05-14"),
                        Priority = "High",
                        Description = "Initiate cross-training sessions for support agents to handle a wider range of customer issues.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[28]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Customer Feedback Analysis",
                        StartDate = DateTime.Parse("2024-05-15"),
                        DueDate = DateTime.Parse("2024-05-21"),
                        Priority = "Medium",
                        Description = "Analyze recent customer feedback to identify common issues and areas for service improvement.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[28]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Develop Customer Loyalty Program",
                        StartDate = DateTime.Parse("2024-05-22"),
                        DueDate = DateTime.Parse("2024-05-28"),
                        Priority = "Medium",
                        Description = "Work with the marketing team to develop a customer loyalty program to reward and retain long-term customers.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[28]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Internal Knowledge Sharing Sessions",
                        StartDate = DateTime.Parse("2024-05-29"),
                        DueDate = DateTime.Parse("2024-06-04"),
                        Priority = "Low",
                        Description = "Organize regular knowledge sharing sessions among support agents to disseminate solutions to common problems.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[28]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Update Support SLAs",
                        StartDate = DateTime.Parse("2024-06-05"),
                        DueDate = DateTime.Parse("2024-06-11"),
                        Priority = "High",
                        Description = "Review and update the support service level agreements (SLAs) to ensure they meet current customer expectations.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[29]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Optimize Support Channels",
                        StartDate = DateTime.Parse("2024-06-12"),
                        DueDate = DateTime.Parse("2024-06-18"),
                        Priority = "Medium",
                        Description = "Evaluate the effectiveness of different support channels and optimize based on customer preferences and resolution efficiency.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[29]],
                        Reporter =employess[3]
                    },

                    new Tasks{
                        Title = "Customer Success Stories Compilation",
                        StartDate = DateTime.Parse("2024-06-19"),
                        DueDate = DateTime.Parse("2024-06-25"),
                        Priority = "Low",
                        Description = "Compile customer success stories to highlight the value of support services in solving critical issues.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[29]]
                    ,
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Support Team Wellness Program",
                        StartDate = DateTime.Parse("2024-06-26"),
                        DueDate = DateTime.Parse("2024-07-02"),
                        Priority = "Medium",
                        Description = "Implement a wellness program for the support team to address burnout and improve overall job satisfaction.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[29]],
                        Reporter =employess[5]
                    },

                    new Tasks{
                        Title = "Annual Support Strategy Review",
                        StartDate = DateTime.Parse("2024-07-03"),
                        DueDate = DateTime.Parse("2024-07-09"),
                        Priority = "High",
                        Description = "Conduct an annual review of the support strategy to align with company goals and changing customer needs.",
                        Status = "Planned",
                        Type="Customer Support",
                    Asignees = [employess[29]],
                        Reporter =employess[12]
                    }


                };





                project2.Tasks = new List<Tasks>
                    {

                    // quality assurance 
                   new Tasks{
                        Title = "Test Plan Development for CRM System",
                        StartDate = DateTime.Parse("2024-03-01"),
                        DueDate = DateTime.Parse("2024-03-07"),
                        Priority = "High",
                        Description = "Develop a comprehensive test plan covering all functional areas of the CRM system.",
                        Status = "Planned",
                        Asignees = [employess[0]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Automated Testing Framework Setup",
                        StartDate = DateTime.Parse("2024-03-08"),
                        DueDate = DateTime.Parse("2024-03-14"),
                        Priority = "High",
                        Description = "Set up an automated testing framework to facilitate continuous testing throughout the development cycle.",
                        Status = "Planned",
                        Asignees = [employess[0]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "User Acceptance Testing Criteria Definition",
                        StartDate = DateTime.Parse("2024-03-15"),
                        DueDate = DateTime.Parse("2024-03-21"),
                        Priority = "Medium",
                        Description = "Define clear user acceptance testing criteria in collaboration with stakeholders to ensure the system meets business requirements.",
                        Status = "Planned",
                        Asignees = [employess[0]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Performance Testing of CRM System",
                        StartDate = DateTime.Parse("2024-03-22"),
                        DueDate = DateTime.Parse("2024-03-28"),
                        Priority = "High",
                        Description = "Conduct performance testing to ensure the CRM system can handle expected load and user count efficiently.",
                        Status = "Planned",
                        Asignees = [employess[1]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Security Vulnerability Assessment",
                        StartDate = DateTime.Parse("2024-03-29"),
                        DueDate = DateTime.Parse("2024-04-04"),
                        Priority = "High",
                        Description = "Perform a security vulnerability assessment to identify and mitigate potential security risks.",
                        Status = "Planned",
                        Asignees = [employess[1]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "CRM System Regression Testing",
                        StartDate = DateTime.Parse("2024-04-05"),
                        DueDate = DateTime.Parse("2024-04-11"),
                        Priority = "Medium",
                        Description = "Execute regression testing prior to major releases to ensure new changes do not adversely affect existing functionalities.",
                        Status = "Planned",
                        Asignees = [employess[1]],
                        Reporter =employess[4]
                    },





                    // software developer 
                    new Tasks{
                        Title = "CRM Database Design",
                        StartDate = DateTime.Parse("2024-02-01"),
                        DueDate = DateTime.Parse("2024-02-10"),
                        Priority = "High",
                        Description = "Design and model the database schema for the CRM system to ensure efficient data storage and retrieval.",
                        Status = "Planned",
                        Asignees = [employess[6]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "API Development for CRM Features",
                        StartDate = DateTime.Parse("2024-02-11"),
                        DueDate = DateTime.Parse("2024-02-20"),
                        Priority = "High",
                        Description = "Develop RESTful APIs for managing customer data, interactions, and insights within the CRM system.",
                        Status = "Planned",
                        Asignees = [employess[6]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Integration with Email Services",
                        StartDate = DateTime.Parse("2024-02-21"),
                        DueDate = DateTime.Parse("2024-03-02"),
                        Priority = "Medium",
                        Description = "Implement integration with email services for marketing campaigns and customer communication.",
                        Status = "Planned",
                        Asignees = [employess[6]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "User Authentication and Authorization",
                        StartDate = DateTime.Parse("2024-03-03"),
                        DueDate = DateTime.Parse("2024-03-12"),
                        Priority = "High",
                        Description = "Develop a secure authentication and authorization module for the CRM system.",
                        Status = "Planned",
                        Asignees = [employess[7]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Frontend Development for CRM Dashboard",
                        StartDate = DateTime.Parse("2024-03-13"),
                        DueDate = DateTime.Parse("2024-03-22"),
                        Priority = "High",
                        Description = "Design and develop the frontend interface for the CRM dashboard, focusing on usability and data visualization.",
                        Status = "Planned",
                        Asignees = [employess[7]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Mobile Application Development",
                        StartDate = DateTime.Parse("2024-03-23"),
                        DueDate = DateTime.Parse("2024-04-01"),
                        Priority = "Medium",
                        Description = "Develop a mobile application version of the CRM system for iOS and Android platforms.",
                        Status = "Planned",
                        Asignees = [employess[7]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Implementing Customer Feedback Features",
                        StartDate = DateTime.Parse("2024-04-02"),
                        DueDate = DateTime.Parse("2024-04-11"),
                        Priority = "Medium",
                        Description = "Implement features within the CRM to collect and analyze customer feedback.",
                        Status = "Planned",
                        Asignees = [employess[8]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Data Migration Tools Development",
                        StartDate = DateTime.Parse("2024-04-12"),
                        DueDate = DateTime.Parse("2024-04-21"),
                        Priority = "Low",
                        Description = "Create tools to facilitate data migration from existing systems to the new CRM system.",
                        Status = "Planned",
                        Asignees = [employess[8]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Real-time Analytics and Reporting",
                        StartDate = DateTime.Parse("2024-04-22"),
                        DueDate = DateTime.Parse("2024-05-01"),
                        Priority = "High",
                        Description = "Develop real-time analytics and reporting capabilities to provide insights into customer interactions and system performance.",
                        Status = "Planned",
                        Asignees = [employess[8]],
                        Reporter =employess[4]
                    },

                    // UI-UX 

                    new Tasks{
                        Title = "CRM User Interface Design",
                        StartDate = DateTime.Parse("2024-02-01"),
                        DueDate = DateTime.Parse("2024-02-08"),
                        Priority = "High",
                        Description = "Design the user interface for the CRM system, focusing on simplicity and ease of navigation for an optimal user experience.",
                        Status = "Planned",
                        Asignees = [employess[14]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "User Experience Research",
                        StartDate = DateTime.Parse("2024-02-09"),
                        DueDate = DateTime.Parse("2024-02-16"),
                        Priority = "High",
                        Description = "Conduct user experience research to identify the needs and pain points of CRM users.",
                        Status = "Planned",
                        Asignees = [employess[14]],
                        Reporter =employess[11]
                    },

                    new Tasks{
                        Title = "Design System for CRM",
                        StartDate = DateTime.Parse("2024-02-17"),
                        DueDate = DateTime.Parse("2024-02-24"),
                        Priority = "Medium",
                        Description = "Develop a design system for the CRM project to ensure consistency across all user interfaces.",
                        Status = "Planned",
                        Asignees = [employess[14]],
                        Reporter =employess[13]
                    },

                    new Tasks{
                        Title = "Interactive Prototyping",
                        StartDate = DateTime.Parse("2024-02-25"),
                        DueDate = DateTime.Parse("2024-03-04"),
                        Priority = "High",
                        Description = "Create interactive prototypes of the CRM system for early testing and feedback.",
                        Status = "Planned",
                        Asignees = [employess[15]],
                        Reporter =employess[4]
                    },

                    new Tasks{
                        Title = "Accessibility and Inclusivity Audit",
                        StartDate = DateTime.Parse("2024-03-05"),
                        DueDate = DateTime.Parse("2024-03-12"),
                        Priority = "Medium",
                        Description = "Perform an audit to ensure the CRM design is accessible and inclusive, adhering to WCAG guidelines.",
                        Status = "Planned",
                        Asignees = [employess[15]],
                        Reporter =employess[12]
                    },

                    new Tasks{
                        Title = "Usability Testing Sessions",
                        StartDate = DateTime.Parse("2024-03-13"),
                        DueDate = DateTime.Parse("2024-03-20"),
                        Priority = "High",
                        Description = "Organize usability testing sessions with potential CRM users to gather feedback and identify areas for improvement.",
                        Status = "Planned",
                        Asignees = [employess[15]],
                        Reporter =employess[12]
                    },

                    //frontend developers


                    new Tasks{
                            Title = "Implement CRM Dashboard UI",
                            StartDate = DateTime.Parse("2024-02-01"),
                            DueDate = DateTime.Parse("2024-02-10"),
                            Priority = "High",
                            Description = "Develop the user interface for the CRM dashboard based on the UX designs, ensuring responsive design across devices.",
                            Status = "Planned",
                            Asignees = [employess[17]],
                            Reporter =employess[4]
                        },

                        new Tasks{
                            Title = "Integration with Backend APIs",
                            StartDate = DateTime.Parse("2024-02-11"),
                            DueDate = DateTime.Parse("2024-02-17"),
                            Priority = "High",
                            Description = "Integrate the frontend components with backend APIs for dynamic data retrieval and management.",
                            Status = "Planned",
                            Asignees = [employess[17]],
                            Reporter =employess[12]
                        },

                        new Tasks{
                            Title = "Frontend Performance Optimization",
                            StartDate = DateTime.Parse("2024-02-18"),
                            DueDate = DateTime.Parse("2024-02-24"),
                            Priority = "Medium",
                            Description = "Optimize frontend performance to ensure quick load times and a smooth user experience.",
                            Status = "Planned",
                            Asignees = [employess[17]],
                            Reporter =employess[11]
                        },

                        new Tasks{
                            Title = "Implement Advanced Data Visualization",
                            StartDate = DateTime.Parse("2024-02-25"),
                            DueDate = DateTime.Parse("2024-03-03"),
                            Priority = "Medium",
                            Description = "Develop advanced data visualization components for the CRM dashboard to help users easily interpret complex datasets.",
                            Status = "Planned",
                            Asignees = [employess[18]],
                            Reporter =employess[11]
                        },

                        new Tasks{
                            Title = "Mobile Responsiveness Testing",
                            StartDate = DateTime.Parse("2024-03-04"),
                            DueDate = DateTime.Parse("2024-03-10"),
                            Priority = "High",
                            Description = "Conduct thorough testing to ensure the CRM system is fully responsive and functional on mobile devices.",
                            Status = "Planned",
                            Asignees = [employess[18]],
                            Reporter =employess[11]
                        },

                        new Tasks{
                            Title = "Frontend Security Enhancements",
                            StartDate = DateTime.Parse("2024-03-11"),
                            DueDate = DateTime.Parse("2024-03-17"),
                            Priority = "High",
                            Description = "Implement security enhancements in the frontend to protect against XSS, CSRF, and other vulnerabilities.",
                            Status = "Planned",
                            Asignees = [employess[18]],
                            Reporter =employess[11]
                        },


                        //DevOps


                        new Tasks{
                            Title = "CI/CD Pipeline for CRM Development",
                            StartDate = DateTime.Parse("2024-02-01"),
                            DueDate = DateTime.Parse("2024-02-07"),
                            Priority = "High",
                            Description = "Set up and configure a CI/CD pipeline to automate the build, test, and deployment processes for the CRM system.",
                            Status = "Planned",
                            Asignees = [employess[19]],
                            Reporter =employess[12]
                        },

                        new Tasks{
                            Title = "Cloud Infrastructure Provisioning",
                            StartDate = DateTime.Parse("2024-02-08"),
                            DueDate = DateTime.Parse("2024-02-14"),
                            Priority = "High",
                            Description = "Provision and configure cloud infrastructure for hosting the CRM system, ensuring scalability and reliability.",
                            Status = "Planned",
                            Asignees = [employess[19]],
                            Reporter =employess[12]
                        },

                        new Tasks{
                            Title = "Containerization with Docker",
                            StartDate = DateTime.Parse("2024-02-15"),
                            DueDate = DateTime.Parse("2024-02-21"),
                            Priority = "Medium",
                            Description = "Containerize the CRM application using Docker to ensure consistency across different environments.",
                            Status = "Planned",
                            Asignees = [employess[19]],
                            Reporter =employess[12]
                        },

                        new Tasks{
                            Title = "Kubernetes Orchestration Setup",
                            StartDate = DateTime.Parse("2024-02-22"),
                            DueDate = DateTime.Parse("2024-02-28"),
                            Priority = "High",
                            Description = "Set up Kubernetes orchestration for the Docker containers to manage the CRM application deployment and scaling.",
                            Status = "Planned",
                            Asignees = [employess[20]],
                            Reporter =employess[12]
                        },

                        new Tasks{
                            Title = "Monitoring and Logging Systems",
                            StartDate = DateTime.Parse("2024-03-01"),
                            DueDate = DateTime.Parse("2024-03-07"),
                            Priority = "Medium",
                            Description = "Implement monitoring and logging systems to track the CRM system's performance and troubleshoot issues.",
                            Status = "Planned",
                            Asignees = [employess[20]],
                            Reporter =employess[12]
                        },

                        new Tasks{
                            Title = "Automated Backup and Recovery Plan",
                            StartDate = DateTime.Parse("2024-03-08"),
                            DueDate = DateTime.Parse("2024-03-14"),
                            Priority = "High",
                            Description = "Develop and implement an automated backup and recovery plan to ensure data integrity and availability.",
                            Status = "Planned",
                            Asignees = [employess[20]],
                            Reporter =employess[12]
                        },

                        // marketing

                        new Tasks{
                            Title = "CRM System Market Analysis",
                            StartDate = DateTime.Parse("2024-02-01"),
                            DueDate = DateTime.Parse("2024-02-08"),
                            Priority = "High",
                            Description = "Conduct a comprehensive market analysis to understand the competitive landscape and identify target markets for the CRM system.",
                            Status = "Planned",
                            Asignees = [employess[23]],
                            Reporter =employess[11]
                        },

                        new Tasks{
                            Title = "Branding and Messaging Strategy",
                            StartDate = DateTime.Parse("2024-02-09"),
                            DueDate = DateTime.Parse("2024-02-15"),
                            Priority = "High",
                            Description = "Develop a branding and messaging strategy that clearly communicates the value proposition of the CRM system.",
                            Status = "Planned",
                            Asignees = [employess[23]],
                            Reporter =employess[11]
                        },

                        new Tasks{
                            Title = "Content Creation Plan",
                            StartDate = DateTime.Parse("2024-02-16"),
                            DueDate = DateTime.Parse("2024-02-22"),
                            Priority = "Medium",
                            Description = "Outline a content creation plan for blog posts, whitepapers, and case studies that highlight the CRM system's features and benefits.",
                            Status = "Planned",
                            Asignees = [employess[23]],
                            Reporter =employess[11]
                        },

                        new Tasks{
                            Title = "Social Media Promotion Strategy",
                            StartDate = DateTime.Parse("2024-02-23"),
                            DueDate = DateTime.Parse("2024-03-01"),
                            Priority = "Medium",
                            Description = "Develop a social media strategy to promote the CRM system and engage with potential customers on various platforms.",
                            Status = "Planned",
                            Asignees = [employess[24]],
                            Reporter =employess[13]
                        },

                        new Tasks{
                            Title = "Launch Event Planning",
                            StartDate = DateTime.Parse("2024-03-02"),
                            DueDate = DateTime.Parse("2024-03-09"),
                            Priority = "High",
                            Description = "Plan and organize a launch event for the CRM system to generate buzz and attract early adopters.",
                            Status = "Planned",
                            Asignees = [employess[24]],
                            Reporter =employess[13]
                        },

                        new Tasks{
                            Title = "Customer Testimonial Campaign",
                            StartDate = DateTime.Parse("2024-03-10"),
                            DueDate = DateTime.Parse("2024-03-17"),
                            Priority = "Low",
                            Description = "Organize a campaign to collect and feature testimonials from early CRM system users to build credibility and trust.",
                            Status = "Planned",
                            Asignees = [employess[24]],
                            Reporter =employess[13]
                        },

                        // customer support

                        new Tasks{
    Title = "CRM Support Training Program",
    StartDate = DateTime.Parse("2024-02-01"),
    DueDate = DateTime.Parse("2024-02-10"),
    Priority = "High",
    Description = "Develop and conduct a training program for customer support agents on the new CRM system to ensure they are equipped to assist users effectively.",
    Status = "Planned",
    Asignees = [employess[26]],
    Reporter =employess[4]
},

new Tasks{
    Title = "FAQs Compilation for CRM System",
    StartDate = DateTime.Parse("2024-02-11"),
    DueDate = DateTime.Parse("2024-02-17"),
    Priority = "Medium",
    Description = "Compile a comprehensive list of FAQs related to the CRM system to aid in quick resolution of common queries.",
    Status = "Planned",
    Asignees = [employess[26]],
    Reporter =employess[4]
},

new Tasks{
    Title = "Create CRM System Support Guides",
    StartDate = DateTime.Parse("2024-02-18"),
    DueDate = DateTime.Parse("2024-02-24"),
    Priority = "Medium",
    Description = "Create detailed support guides and documentation for the CRM system to enhance the self-service resources for users.",
    Status = "Planned",
    Asignees = [employess[26]],
    Reporter =employess[4]
},

new Tasks{
    Title = "Feedback Collection Mechanism",
    StartDate = DateTime.Parse("2024-02-25"),
    DueDate = DateTime.Parse("2024-03-03"),
    Priority = "High",
    Description = "Implement a mechanism for collecting feedback from CRM system users to continuously improve the support process.",
    Status = "Planned",
    Asignees = [employess[27]],
    Reporter =employess[11]
},

new Tasks{
    Title = "Support Channel Optimization",
    StartDate = DateTime.Parse("2024-03-04"),
    DueDate = DateTime.Parse("2024-03-10"),
    Priority = "Medium",
    Description = "Evaluate and optimize the various support channels (email, phone, chat) to ensure efficient and effective user support.",
    Status = "Planned",
    Asignees = [employess[27]],
    Reporter =employess[12]
},

new Tasks{
    Title = "Emergency Response Plan for CRM Issues",
    StartDate = DateTime.Parse("2024-03-11"),
    DueDate = DateTime.Parse("2024-03-17"),
    Priority = "High",
    Description = "Develop an emergency response plan to swiftly address and resolve critical issues reported by CRM system users.",
    Status = "Planned",
    Asignees = [employess[27]],
    Reporter =employess[13]
}






                };




                Context.AddRange(employess);
                Context.projects.Add(project1);
                Context.projects.Add(project2);

                Context.SaveChanges();

            }
            
        }
    }
}
