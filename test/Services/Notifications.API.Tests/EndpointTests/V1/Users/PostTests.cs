using AutoMapper;
using Notifications.Core.Requests;
using Notifications.Core.Responses;

namespace Notifications.API.Tests.EndpointTests.V1.Users;

[TestFixture]
public class PostTests
{
    private Post POSTendpoint = default!;
    private Mock<IUserRepository> _userRepositoryMock = default!;
    private Mock<IMapper> _mapperMock = default!;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new();
        _mapperMock = new();
        POSTendpoint = new(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task ExecutePost_EnterNewUser_ReturnsCreatedResponse()
    {
        DateTimeOffset dateTimeOffsetTest = DateTimeOffset.UtcNow;
        CreateUserRequest testUserRequest = new() { Name = "Henry" };
        User testUser = new() { ID = 25, Name = "Henry", CreatedAt = dateTimeOffsetTest };

        _mapperMock.Setup(x => x.Map<User>(testUserRequest)).Returns(testUser);
        _mapperMock.Setup(x => x.Map<CreateUserResponse>(testUser)).Returns(new CreateUserResponse { ID = 25, Name = "Henry" });
        _userRepositoryMock.Setup(x => x.AddAsync(testUser, default));

        var response = await POSTendpoint.HandleAsync(testUserRequest);
        var result = response.Result as CreatedAtRouteResult;
        var data = result!.Value;

        Assert.Multiple(() =>
        {
            _mapperMock.Verify(x => x.Map<User>(testUserRequest), Times.Once);
            _mapperMock.Verify(x => x.Map<CreateUserResponse>(testUser), Times.Once);

            _userRepositoryMock.Verify(x => x.AddAsync(testUser, default), Times.Once);
            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Once);

            Assert.That(data, Is.InstanceOf<CreateUserResponse>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CreatedAtRouteResult>());
            Assert.That(result.RouteName, Is.EqualTo("Users_Get"));

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ActionResult<CreateUserResponse>>());

        });
    }

    [Test]
    public async Task ExecutePost_EnterUserWithWrongInput_ReturnsBadRequestResponse()
    {
        POSTendpoint.ModelState.AddModelError("TestError", "TestErrorMessage");
        CreateUserRequest testUserRequest = new() { Name = "Henry" };
        User testUser = new() { ID = 25, Name = "Henry" };

        var response = await POSTendpoint.HandleAsync(testUserRequest);
        var result = response.Result;

        Assert.Multiple(() =>
        {
            _mapperMock.Verify(x => x.Map<User>(testUserRequest), Times.Never);
            _mapperMock.Verify(x => x.Map<CreateUserResponse>(testUser), Times.Never);

            _userRepositoryMock.Verify(x => x.AddAsync(testUser, default), Times.Never);
            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Never);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ActionResult<CreateUserResponse>>());
        });
    }

    [Test]
    public void ExecutePost_UserRepoIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => POSTendpoint = new(null!, _mapperMock.Object));
    }

    [Test]
    public void ExecutePost_IMapperIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => POSTendpoint = new(_userRepositoryMock.Object, null!));
    }
}
