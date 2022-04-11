﻿namespace Notifications.API.Tests.EndpointTests.V1.Users;

[TestFixture]
public class GetTests
{
    private Get usersGETendpoint = default!;
    private Mock<IUserRepository> _userRepoMock = default!;

    [SetUp]
    public void Setup()
    {
        _userRepoMock = new();
        usersGETendpoint = new(_userRepoMock.Object);
    }

    [Test]
    public async Task ExecuteGet_EnterUserID_ReturnUserWithEnteredID()
    {
        User testUser = new() { ID = 1, Name = "Henry" };

        _userRepoMock.Setup(x => x.GetUserById(3, default))
            .ReturnsAsync(testUser);

        var result = await usersGETendpoint.HandleAsync(3) as ObjectResult;
        var res = result!.Value;

        Assert.Multiple(() =>
        {
            _userRepoMock.Verify(x => x.GetUserById(3, default), Times.Once);

            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.InstanceOf<User>());
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        });
    }

    [Test]
    public async Task ExecuteGet_EnterUserID_ReturnNull()
    {
        _userRepoMock.Setup(x => x.GetUserById(3, default))
            .ReturnsAsync(() => null);

        var result = await usersGETendpoint.HandleAsync(3) as NotFoundResult;

        Assert.Multiple(() =>
        {
            _userRepoMock.Verify(x => x.GetUserById(3, default), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public void ExecuteGet_UserRepoIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => usersGETendpoint = new(null!));
    }
}
