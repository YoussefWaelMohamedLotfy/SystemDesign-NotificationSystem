namespace Notifications.API.Tests.EndpointTests.V1.Users;

[TestFixture]
public class DeleteTests
{
    private Delete DELETEendpoint = default!;
    private Mock<IUserRepository> _userRepositoryMock = default!;


    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new();
        DELETEendpoint = new(_userRepositoryMock.Object);
    }

    [Test]
    public async Task ExecuteDelete_EnterUserID_UserDeletedAndReturnsNoContent()
    {
        User testUser = new() { ID = 5 };
        _userRepositoryMock.Setup(x => x.GetUserById(5, default))
            .ReturnsAsync(testUser);

        var response = await DELETEendpoint.HandleAsync(5) as NoContentResult;

        Assert.Multiple(() =>
        {
            _userRepositoryMock.Verify(x => x.GetUserById(5, default), Times.Once());
            _userRepositoryMock.Verify(x => x.Delete(testUser), Times.Once());
            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Once());

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<NoContentResult>());
        });
    }

    [Test]
    public async Task ExecuteDelete_EnterUserID_UserIsNullAndReturnsNotFound()
    {
        User testUser = new() { ID = 50 };
        _userRepositoryMock.Setup(x => x.GetUserById(50, default))
            .ReturnsAsync(() => null);

        var response = await DELETEendpoint.HandleAsync(50);

        Assert.Multiple(() =>
        {
            _userRepositoryMock.Verify(x => x.GetUserById(50, default), Times.Once());
            _userRepositoryMock.Verify(x => x.Delete(testUser), Times.Never());
            _userRepositoryMock.Verify(x => x.CompleteChangesAsync(default), Times.Never());

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public void ExecuteDelete_UserRepoIsNull_ReturnArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DELETEendpoint = new(null!));
    }
}
