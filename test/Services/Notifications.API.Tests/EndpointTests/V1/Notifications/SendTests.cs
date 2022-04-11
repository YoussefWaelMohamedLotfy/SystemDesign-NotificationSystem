using EventBus.RabbitMQ.Events;
using MassTransit;
using Notifications.API.Endpoints.V1.Notifications;
using System.Threading;

namespace Notifications.API.Tests.EndpointTests.V1.Notifications;

[TestFixture]
public class SendTests
{
    private Send _sendEndpoint = default!;
    private Mock<IUserRepository> _userRepositoryMock = default!;
    private Mock<IPublishEndpoint> _publishEndpointMock = default!;


    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new();
        _publishEndpointMock = new();
        _sendEndpoint = new(_publishEndpointMock.Object, _userRepositoryMock.Object);
    }

    [Test]
    public async Task InvokeSendNotification_UserIdExists_ShouldReturnAccepted()
    {
        var testEvent = new { Name = "Ahmed", EmailAddress = "ahmed@yahoo.com", PhoneNumber = "+2071276912" };
        User testUser = new() { ID = 6, Name = "Ahmed", EmailAddress = "ahmed@yahoo.com", PhoneNumber = "+2071276912" };
        SendNotificationEvent? message = default!;

        _userRepositoryMock.Setup(x => x.GetUserById(6, default)).ReturnsAsync(testUser);
        _publishEndpointMock.Setup(x => x.Publish<SendNotificationEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((a, b) => { message = a as SendNotificationEvent; }); ;

        var response = await _sendEndpoint.HandleAsync(6);

        Assert.Multiple(() =>
        {
            _userRepositoryMock.Verify(x => x.GetUserById(6, default), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish<SendNotificationEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<AcceptedResult>());
        });
    }

    [Test]
    public async Task InvokeSendNotification_UserIdNotExists_ShouldReturnNotFound()
    {
        User testUser = new() { ID = 6, Name = "Ahmed", EmailAddress = "ahmed@yahoo.com", PhoneNumber = "+2071276912" };

        _userRepositoryMock.Setup(x => x.GetUserById(8, default))
            .ReturnsAsync(() => null);

        var response = await _sendEndpoint.HandleAsync(8);

        Assert.Multiple(() =>
        {
            _userRepositoryMock.Verify(x => x.GetUserById(8, default), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish<SendNotificationEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public void ExecutePost_PublishEnpointIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sendEndpoint = new(null!, _userRepositoryMock.Object));
    }

    [Test]
    public void ExecutePost_IUserRepositoryIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sendEndpoint = new(_publishEndpointMock.Object, null!));
    }
}
