using Microsoft.AspNetCore.Mvc;
using Moq;
using Notifications.API.Data.Repository;
using Notifications.API.Endpoints.V1.Users;
using Notifications.API.Models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Notifications.API.Tests.EndpointTests.V1.Users;

[TestFixture]
public class GetTests
{
    private Get usersGETendpoint = default!;
    private Mock<IUserRepository> _userRepo = default!;

    [SetUp]
    public void Setup()
    {
        _userRepo = new();
        usersGETendpoint = new(_userRepo.Object);
    }
    
    [Test]
    public async Task ExecuteGet_EnterUserID_ReturnUserWithEnteredID()
    {
        User testUser = new() { ID = 1, Name = "Henry" };

        _userRepo.Setup(x => x.GetUserById(3, default))
            .ReturnsAsync(testUser);

        var result = await usersGETendpoint.HandleAsync(3) as ObjectResult;
        var res = result!.Value;

        Assert.Multiple(() =>
        {
            _userRepo.Verify(x => x.GetUserById(3, default), Times.Once);

            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.InstanceOf<User>());
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        });
    }

    [Test]
    public async Task ExecuteGet_EnterUserID_ReturnNull()
    {
        _userRepo.Setup(x => x.GetUserById(3, default))
            .ReturnsAsync(() => null);

        var result = await usersGETendpoint.HandleAsync(3) as NotFoundResult;

        Assert.Multiple(() =>
        {
            _userRepo.Verify(x => x.GetUserById(3, default), Times.Once);

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
