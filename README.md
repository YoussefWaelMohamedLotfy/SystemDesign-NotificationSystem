# Notification System - System Design Interview

> This repository is my practical solution for the problem in [Chapter 10 in System Design Interview (Second Edition) by Dr. Alex Xu](https://printige.net/product/system-design-interview-an-insider-guide/)

## System Components
  + `Notification UI` : An MVC App for sending requests to the API, using [Refit Library](https://github.com/reactiveui/refit) and trigger notifications.
  + `Notification API` : An API that contains Users' Data and notification settings.
  + `RabbitMQ Messaging Broker` : Docker Image for publishing events.
  + `SMS Worker Service` : Responsible for consuming events from RabbitMQ and sending SMS using [Twilio API](https://www.twilio.com/)
  + `Email Worker Service` : Responsible for consuming events from RabbitMQ and sending Emails using [SendGrid API](https://sendgrid.com/)

## Configuring the Database
Navigate to the `Notification.API` folder, and Run the command `Update-Database` in VS Package Manager Console, or `dotnet ef database update` with the *dotnet CLI*.
> The Database used is a MS SQL Server LocalDb, which comes by default when installing Visual Studio. You can change the connection string in the *appsettings.json* file.

## Steps to get the System up and running
1. Install Tye on your machine from its [Documentation page on Github](https://github.com/dotnet/tye/blob/main/docs/getting_started.md)
2. Clone the repo using `git clone https://github.com/YoussefWaelMohamedLotfy/SystemDesign-NotificationSystem.git`
3. Change Current Directory to Solution's root folder with `cd ./`
4. Run `tye run` to run the system locally. This will start all the components without any hassle. *Even the RabbitMQ image pulling is handled for you.*
    > You can view the Tye Dashboard at [https://localhost:8000](https://localhost:8000) 
5. Navigate to [http://localhost:7003](http://localhost:7003) to view Notification.UI