using AutoMapper;
using Notifications.Core.Responses;

namespace Notifications.API.Tests.EndpointTests.V1.Users;

[TestFixture]
public class GetTests
{
    private Get usersGETendpoint = default!;
    private Mock<IUserRepository> _userRepoMock = default!;
    private Mock<IMapper> _mapperMock = default!;

    [SetUp]
    public void Setup()
    {
        _userRepoMock = new();
        _mapperMock = new();
        usersGETendpoint = new(_userRepoMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task ExecuteGet_EnterUserID_ReturnUserWithEnteredID()
    {
        User testUser = new() { ID = 3, Name = "Henry" };

        _userRepoMock.Setup(x => x.GetUserById(3, default))
            .ReturnsAsync(testUser);
        _mapperMock.Setup(x => x.Map<GetUserResponse>(testUser))
            .Returns(new GetUserResponse { ID = 3, Name = "Henry" });

        var response = await usersGETendpoint.HandleAsync(3);
        var res = response!.Result;

        Assert.Multiple(() =>
        {
            _userRepoMock.Verify(x => x.GetUserById(3, default), Times.Once);
            _mapperMock.Verify(x => x.Map<GetUserResponse>(testUser), Times.Once);

            Assert.That(response, Is.InstanceOf<ActionResult<GetUserResponse>>());
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.InstanceOf<OkObjectResult>());
        });
    }

    [Test]
    public async Task ExecuteGet_EnterUserID_ReturnNull()
    {
        _userRepoMock.Setup(x => x.GetUserById(3, default))
            .ReturnsAsync(() => null);
        _mapperMock.Setup(x => x.Map<GetUserResponse>(It.IsAny<User>())).Returns(It.IsAny<GetUserResponse>());

        var response = await usersGETendpoint.HandleAsync(3);
        var result = response.Result;

        Assert.Multiple(() =>
        {
            _userRepoMock.Verify(x => x.GetUserById(3, default), Times.Once);

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ActionResult<GetUserResponse>>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public void ExecuteGet_UserRepoIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => usersGETendpoint = new(null!, _mapperMock.Object));
    }

    [Test]
    public void ExecuteGet_IMapperIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => usersGETendpoint = new(_userRepoMock.Object, null!));
    }
}
