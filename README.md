# Notification System - System Design Interview

> This repository is my practical solution for the problem in [Chapter 10 in System Design Interview (Second Edition) by Dr. Alex Xu](https://printige.net/product/system-design-interview-an-insider-guide/)

## System Components
  + `Notification UI` : An MVC App for the sending requests to the API, using [Refit Library](https://github.com/reactiveui/refit)
  + `Notification API` : An API that contains Users' Data and notification settings.
  + `RabbitMQ Messaging Broker` : Docker Image for publishing events.
  + `SMS Worker Service` : Responsible for consuming events from RabbitMQ and sending SMS using [Twilio API](https://www.twilio.com/)
  + `Email Worker Service` : Responsible for consuming events from RabbitMQ and sending Emails using [SendGrid API](https://sendgrid.com/)

## Steps to get the System up and running

1. Clone the repo using `git clone https://github.com/YoussefWaelMohamedLotfy/SystemDesign-NotificationSystem.git`
2. Change Current Directory to Solution's root folder with `cd ./`
3. Run `tye run` to run the system locally. This will start all the components without any hassle. *Even the RabbitMQ image pulling is handled for you.*
4. Navigate to `localhost:7000/swagger` to view Notification.API