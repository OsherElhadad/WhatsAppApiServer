# Getting Started

* Make sure MariaDB is installed. If not, please download it from here: https://mariadb.org/
* Clone this repository.
* Open the project with IDE like- Visual Studio 2022 and make sure WhatsAppApiServer.sln is set as the project solution.
* To work with MariaDB using an Entity Framework, we will use the Pomelo Entity Framework. It is available as a NuGet package here: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql <br>
You can simply open the package manager (PM) console <br>
(In Visual Studio 2022 -> View -> Other Windows -> Package Manager Console).
* Next, on the Package Manager Console, please enter the following command: <br>
  $ Install-Package Pomelo.EntityFrameworkCore.MySql -Version 6.0.1
* Then, install the Microsoft Entity Framework Tools package by entering the following command: <br>
  $ Install-Package Microsoft.EntityFrameworkcore.Tools -version 6.0.1

* Finally create the database:
  1) Navigate on the cloned directory to: WhatsAppApiServer\Data\WhatsAppApiContext.cs and change your mariaDB password in connectionString- "...password={your              password}..."
  2) Apply the migration by entering on the Package Manager Console: $ update-database

Note: We are working with .NET version 6.0.1. Please make sure it is installed. 

You are ready to start!
In oreder to run the api server, open the project with IDE (like visual studio 2022) and press on run.
<br>
All you need to do now is to wait for the swagger to open on you browser, which indicates that the server is ready and running.

## Users Controller

* Has Get http://localhost:5146/api/Users - returns json of the all users (200) or not found (404) if empty.
* Has Get http://localhost:5146/api/Users/{Id} - returns json of this user (200) or not found (404) if this id doesn't exist.
* Has Post http://localhost:5146/api/Users - create new user by getting json of UserId and Password in the body of the request and returns (200) the token of the JWT if created successfully or failed (400) else.
* Has Put http://localhost:5146/api/Users/{Id} - update user by getting json of Password in the body of the request and returns (204) if updated successfully or failed (400) else.
* Has Delete http://localhost:5146/api/Users/{Id} - delete user and returns (204) if deleted successfully or failed (400) else.

## Contacts Controller

* Has Get http://localhost:5146/api/Contacts - returns json of the all contacts (200) of the loged in user or not found (404) if empty.
* Has Get http://localhost:5146/api/Contacts/{Id} - returns json of this contact (200) of the loged in user or not found (404) if this id doesn't exist.
* Has Post http://localhost:5146/api/Contacts - create new contact to the loged in user by getting json of Id, Name and Server in the body of the request and returns (201) if created successfully or failed (400) else.
* Has Put http://localhost:5146/api/Contacts/{Id} - update contact of the loged in user by getting json of Name and Server in the body of the request and returns (204) if updated successfully or failed (400) else.
* Has Delete http://localhost:5146/api/Contacts/{Id} - delete contact of the loged in user and returns (204) if deleted successfully or failed (400) else.

## Messages Controller

* Has Get http://localhost:5146/api/Contacts/{Id}/Messages - returns json of the all messages (200) of the loged in user and the specific contact or not found (404) if empty.
* Has Get http://localhost:5146/api/Contacts/{Id}/Messages/{Id2} - returns json of this message (200) of the loged in user and the specific contact or not found (404) if this id doesn't exist.
* Has Post http://localhost:5146/api/Contacts/{Id}/Messages - create new message to the loged in user and the specific contact by getting json of Content in the body of the request and returns (201) if created successfully or failed (400) else.
* Has Put http://localhost:5146/api/Contacts/{Id}/Messages/{Id2} - update message of the loged in user and the specific contact by getting json of Content in the body of the request and returns (204) if updated successfully or failed (400) else.
* Has Delete http://localhost:5146/api/Contacts/{Id}/Messages/{Id2} - delete message of the loged in user and the specific contact and returns (204) if deleted successfully or failed (400) else.

## Invitations Controller

* Has Post http://localhost:5146/api/Invitations - create new contact by getting json of From (the contact we add), To (the user we add the contact to) and Server (the server of the contact) in the body of the request and returns (201) if created successfully or failed (400) else.

## Transfer Controller

* Has Post http://localhost:5146/api/Transfer - create new message by getting json of From (the contact send the message), To (the user who got the message) and Content (the content of the message) in the body of the request and returns (201) if created successfully or failed (400) else.

## LogIn Controller

* Has Post http://localhost:5146/api/LogIn - log in (user) by getting json of UserId and Password in the body of the request and returns (200) the token of the JWT if created successfully or failed (400) else.

## Services

* Has IUsersService, IContactsService, IMessagesService interfaces of UsersService, ContactsService, MessagesService that has field of WhatsAppApiContext that contains the 3 DB tables (entity framework) that does the controller methods that uses the DB context.
* Has HubService that has static dictionary of connectionId's and users.

## MariaDB

* There is a database named: WhatsAppApiDB.
* There is a table named Users with these fields:
  1) Id (key)- a string with 2-100 characters.
  2) Password- a string with more then 1 characters.
* There is a table named Contacts with these fields:
  1) Id (key)- a string with 2-100 characters.
  2) UserId (key, and foreign key of Users)- a string with 2-100 characters.
  3) Last- a string with 0 or more characters (last message).
  4) LastDate- a date (nullable) (last date of the last message).
  5) Name- a string with 2-100 characters (the contact's nickname).
  6) Server- a string with 1 or more characters.
* There is a table named Messages with these fields:
  1) Id (key)- a number (auto increase).
  2) UserId (foreign key of Contacts and Users)- a string with 2-100 characters.
  3) ContactId (foreign key of Contacts)- a string with 2-100 characters.
  4) Content- a string with 1-2000 characters.
  5) Created- a date (when created).
  6) Sent- a bool (0 or 1) if sent by the user then it is true and false if sent by the contact.
