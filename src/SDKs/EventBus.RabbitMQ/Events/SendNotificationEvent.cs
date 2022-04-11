using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ.Events;

public interface SendNotificationEvent
{
    string Name { get; }

    string EmailAddress { get; }

    string PhoneNumber { get; }
}
