# TaskManager

TaskManager is a user-friendly web application developed using ASP.NET Core 8. It serves as a comprehensive tool for organizations to effectively tasks across multiple projects and teams. Within this platform, employees have the flexibility to seamlessly create, delete, and update tasks as needed. Furthermore, they can enhance their task reports by incorporating comments, attachments, and additional details. For managers, TaskManager offers advanced functionalities such as project creation, employees invitations, and access to detailed statistics. These features empower managers to optimize project workflows and productivity within the organization.

TaskManager is designed following the principles of Domain-Driven Design (DDD) and employing a clean architecture approach. It is structured into three main components: infrastructure, Domain, and Application. This setup ensures efficiency and maintainability.



## project's dependencies and packages


## Architecture

- **Task Management:** Employees can seamlessly create, delete, and update tasks as needed.
- **Enhanced Task Reports:** Users can incorporate comments, attachments, and additional details to enrich task reports.
- **Project Creation:** Managers have the ability to create new projects within the platform.
- **Advanced Functionality:** Access to detailed statistics empowers managers to optimize project workflows and enhance productivity within the organization.


### 1. Infrastructure

This component deals with the technical concerns such as databases, file systems, external services, and frameworks. It ensures that the application can interact with external resources efficiently.

### 2. Domain

The domain component encapsulates the core logic and rules of the application domain. It represents the business concepts, rules, and logic. This component is independent of the infrastructure and application layers, ensuring that the business logic remains decoupled and testable.

### 3. Application

The application component acts as a mediator between the infrastructure and domain layers. It orchestrates the execution of application use cases, handling input and output operations, and coordinating the interactions between different parts of the system. This component contains the application-specific logic necessary for fulfilling user requests and achieving system goals.

## Getting Started

To set up TaskManager locally, follow these steps:

1. Clone the repository.
   ```bash
   git clone https://github.com/yourusername/TaskManager.git

