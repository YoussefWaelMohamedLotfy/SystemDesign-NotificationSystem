using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Notifications.API.Data.Repository;
using Notifications.API.Endpoints.V1.Users;
using Notifications.API.Models;
using Notifications.Core.Requests;
using Notifications.Core.Responses;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Notifications.API.Tests.EndpointTests.V1.Users;

[TestFixture]
public class UpdateTests
{
    private Update PUTendpoint = default!;
    private Mock<IUserRepository> _userRepositoryMock = default!;
    private Mock<IMapper> _mapperMock = default!;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new();
        _mapperMock = new();
        PUTendpoint = new(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task ExecutePut_PassUpdateUserRequest_ReturnsOkResponse()
    {
        UpdateUserRequest testRequest = new() { ID = 30, Name = "Jeremy", Settings = new() { Channel = "Email", IsOptIn = true } };
        User testUser = new() { ID = 30, Name = "Hany", Settings = new() { Channel = "SMS", IsOptIn = false } };
        User updatedTestUser = new() { ID = 30, Name = "Jeremy", Settings = new() { Channel = "Email", IsOptIn = true } };
        UpdateUserResponse testResponse = new() { ID = 30, Name = "Jeremy", Settings = new() { Channel = "Email", IsOptIn = true } };

        _mapperMock.Setup(x => x.Map(testRequest, testUser)).Returns(updatedTestUser);
        _mapperMock.Setup(x => x.Map<UpdateUserResponse>(testUser)).Returns(testResponse);
        _userRepositoryMock.Setup(x => x.GetUserById(30, default)).ReturnsAsync(testUser);

        var response = await PUTendpoint.HandleAsync(testRequest);
        var result = response.Result as OkObjectResult;

        Assert.Multiple(() =>
        {
            _mapperMock.Verify(x => x.Map(testRequest, testUser), Times.Once);
            _mapperMock.Verify(x => x.Map<UpdateUserResponse>(testUser), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserById(30, default), Times.Once);

            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Once);

            Assert.That(response, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(result!.Value, Is.Not.Null);
            Assert.That(result!.Value, Is.InstanceOf<UpdateUserResponse>());
        });
    }

    [Test]
    public async Task ExecutePut_EnterUserRequestWithWrongInput_ReturnsBadRequestResponse()
    {
        PUTendpoint.ModelState.AddModelError("TestError", "TestErrorMessage");
        UpdateUserRequest testUserRequest = new() { Name = "Henry" };
        User testUser = new() { ID = 25, Name = "Henry" };

        var response = await PUTendpoint.HandleAsync(testUserRequest);
        var result = response.Result;

        Assert.Multiple(() =>
        {
            _mapperMock.Verify(x => x.Map(testUserRequest, testUser), Times.Never);
            _mapperMock.Verify(x => x.Map<UpdateUserResponse>(testUser), Times.Never);
            _userRepositoryMock.Verify(x => x.GetUserById(30, default), Times.Never);
            _userRepositoryMock.Verify(x => x.Update(testUser), Times.Never);
            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Never);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ActionResult<UpdateUserResponse>>());
        });
    }

    [Test]
    public async Task ExecutePut_EnterUserRequestWithNOnExistingID_UserIsNullAndReturnsNotFound()
    {
        UpdateUserRequest testUserRequest = new() { ID = 50 };
        User testUser = new() { ID = 50 };
        _userRepositoryMock.Setup(x => x.GetUserById(50, default))
            .ReturnsAsync(() => null);

        var response = await PUTendpoint.HandleAsync(testUserRequest);
        var result = response.Result;

        Assert.Multiple(() =>
        {
            _userRepositoryMock.Verify(x => x.GetUserById(50, default), Times.Once());
            _userRepositoryMock.Verify(x => x.Update(testUser), Times.Never());
            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Never());

            Assert.That(response, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        });
    }


    [Test]
    public void ExecutePut_UserRepoIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PUTendpoint = new(null!, _mapperMock.Object));
    }

    [Test]
    public void ExecutePut_IMapperIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PUTendpoint = new(_userRepositoryMock.Object, null!));
    }
}
