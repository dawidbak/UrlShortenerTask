## General info 
I followed the MVC path as it was originally in the task repository and arranged the project according to the vertical slice architecture.
### Setup
- Databases are in-memory so there is no need for migration.
- Before enabling the app, please check appsettings.json and configure the "Shortening" sections or leave the default. 
- Simply clone the project and run the application.
### Shortening section
- Url - this should be the address of our application, it will be used to create a short link for the user.
- Numbers - if we want to include the use of digits in the generation of short links.
- Specials - if we want to include the use of special chars in the generation of short links.
- Length - length of our unique id for link shortening.

### Functionalities
- On the home page there is a UI for generating links and a button to list these links.
>http://localhost:5023/

>http://localhost:5023/shortened-urls

- The logs are configured using <strong>Serilog</strong> and we can read them from the console or directly from the folder located in:
>src/logs - a text file will be created when the application is launched for the first time

- Redirect to the original link - when you enter a generated link, e.g. http://localhost:5023/abc123, you will be redirected to a specific saved original link.

Basic unit tests have also been written to test the operation of the handlers.

As for the UI site, it has been designed as simply as possible, so there are, for example, no notifications if an exception occurs.
