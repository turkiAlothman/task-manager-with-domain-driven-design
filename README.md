# TaskManager

TaskManager is a user-friendly web application developed using ASP.NET Core 8. It serves as a comprehensive tool for organizations to effectively tasks across multiple projects and teams. Within this platform, employees have the flexibility to seamlessly create, delete, and update tasks as needed. Furthermore, they can enhance their task reports by incorporating comments, attachments, and additional details. For managers, TaskManager offers advanced functionalities such as project creation, employees invitations, and access to detailed statistics. These features empower managers to optimize project workflows and productivity within the organization.

TaskManager is designed following the principles of Domain-Driven Design (DDD) and employing a clean architecture approach. It is structured into three main components: infrastructure, Domain, and Application. This setup ensures efficiency and maintainability.



## project's dependencies and packages
- **Humanizer:** provide utilizations to convert DateTime object into human readble format date.
- **MimeKit:** Mail client tool used to sends emails within the application.
- **JsonPatch:** provide utilizations to apply a smooth patch request.
- **NewtonsoftJson:** object to json converter.
- **Pomelo.EntityFrameworkCore.MySql** Mysql DBMS driver.
- **Microsoft.EntityFrameworkCore** most pupoler ORM system in asp.net.
- **RandomString4Net** - random text generator.

## features

#### Viewing Tasks

- Employees can view all tasks assigned within the organization.
- Task filtering options are available, allowing employees to filter tasks based on criteria such as status, priority, team, and project.
- Employees can easily access tasks assigned specifically to them.

#### Task Creation

- Employees can create new tasks with various attributes including title, description, priority, status, type, start date, and due date.
- Tasks can be assigned to multiple employees, facilitating collaboration.
- Attachments can be added to tasks, providing additional context or resources.
- Employees have the ability to comment on tasks, promoting communication and collaboration within the team.

#### Editing and Deleting

- Task attributes can be edited by double-clicking on the task.
- Employees can delete their own comments to maintain task clarity and organization.
- Tasks can be deleted if they are no longer relevant or necessary.


## Getting Started

To set up TaskManager locally, follow these steps:

1. Clone the repository.
   ```bash
   git clone https://github.com/yourusername/TaskManager.git

