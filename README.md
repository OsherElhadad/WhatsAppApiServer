# Getting Started

* Clone this repository.
* Open the project with IDE like- visual studio.
* To run this app on your computer please make sure MariaDB is installed. 
If not, please download it from here: https://mariadb.org/

To work with MariaDB using an Entity Framework, we will 
use the Pomelo Entity Framework. 
It is available as a NuGet package here: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql
You can simply open the package manager (PM) console
(In Visual Studio 2022 -> View -> Other Windows -> Package Manager Console).

And enter the following command:
"Install-Package Pomelo.EntityFrameworkCore.MySql -Version 6.0.1"
(*without the quotation marks*)

Then, install the Microsoft Entity Framework Tools package
by entering the following command:
"Install-Package Microsoft.EntityFrameworkcore.Tools -version 6.0.1"

Finally create the database:
  1) Change to your mariaDB password at WhatsAppApiContext.cs in connectionString- "...password={your password}..."
	2) Then apply the migration using: "update-database"

Note: We are working with .NET version 6.0.1.
	  Please make sure it is installed. 


Now you are ready to start!
In oreder to open the website, go to the cloned folder on the CLI and enter the command :
$ npm start
<br>
All you need to do now is to wait for the website to open on you browser.

## Log-In Page

* This is the home page which allows you to log-in to the system and start chatting with your friends!
* In case you are not already signed, you can click the link below the log-in button and sign-up.
* In case you want to check the website ratings, just click on the relevant link!
* When successfully logged-in, you will be navigated to the chats screen.

## Ratings Page

You can find more information about this page on here: https://github.com/OsherElhadad/WhatsAppRating

## Sign-Up Page

* This page allows you to create your account.
* The username, password and verfiy password fields are requierd in order to sign up. In case one of the mentioned is empty or invalid(will be explanied in a bit), the system won't allow you to sign-up and the submit button will be disabled.
* When all required fields are filled and valid, the submit button will be un-disabled and you will be able to submit your request to sign-up.
* While typing in the input fields you will notice a dynamic message below the relavent field. In case the current input is valid, a green border will apear with a check sign and a "Looking good" message. On the other hand, if the current input is invalid, a red border will aprear with a X sign and a message to indicate the specific problem with the input.

Fields requirement in-depth:
- Username must be one word with length greater or equals to 2, without '-' or '_' characters.
- Password must be one word with length greater or equals to 2 and contatin at least one number and one letter characters.
- Verified Password must match the password.

## Chats Page

* Once you logged-in, your contacts list will appear on the left side.
* Above the contacts list, you can see 3 buttons near your default profile picture. From left to right -
* Add new contact - Upon clicking on it, a form modal will show up. You can enter the new contact's username, nickname and server. 
  When The "Add" button is clicked, the form will be submited.
  In case the contact's server succesfully add the new chat, the chat will be created to the user. Otherwise, an error modal will be shown.
* The search button - Once you click it, you can enter text in order to dynamically filter by it. Only contacts on your contacts list that the current input is a substring to thier nickname will appear on your contacts list(not a case sensitive filter).
* Sign-Off - Once clicked on, a modal will appear to ask if you are sure you want to sign-out. You can choose your action by the relavent button.

As for the chat itself - In order to open a chat just click on the desired contact!
* Upon sending a new message or hoevring on the chat window, it will auto scroll to display the lastest messages in the current chat. 
* You can send text messages by clicking on the envelope button or pressing the "Enter" key. You can't send an empty message.
* The microphone, video and picture buttons are disabled as it is not required in this assignment.
