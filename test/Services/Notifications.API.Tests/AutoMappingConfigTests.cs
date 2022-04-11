using AutoMapper;
using Notifications.Core.Requests;
using Notifications.Core.Responses;

namespace Notifications.API.Tests;

[TestFixture]
public class AutoMappingConfigTests
{
    MapperConfiguration _config = default!;


    [SetUp]
    public void Setup()
    {
        _config = new(cfg =>
        {
            // Requests to Domain Models
            cfg.CreateMap<CreateUserRequest, User>(MemberList.Source);
            cfg.CreateMap<CreateNotificationSettingRequest, NotificationSetting>(MemberList.Source).ReverseMap();
            cfg.CreateMap<UpdateUserRequest, User>(MemberList.Source);
            cfg.CreateMap<UpdateNotificationSettingRequest, NotificationSetting>(MemberList.Source).ReverseMap();

            // Domain Models to Responses
            cfg.CreateMap<User, CreateUserResponse>(MemberList.Source);
            cfg.CreateMap<NotificationSetting, CreateNotificationSettingResponse>(MemberList.Source)
                .ForSourceMember(x => x.UserID, opt => opt.DoNotValidate())
                .ForSourceMember(x => x.User, opt => opt.DoNotValidate());
            cfg.CreateMap<User, UpdateUserResponse>(MemberList.Source)
                .ForSourceMember(x => x.CreatedAt, opt => opt.DoNotValidate());
            cfg.CreateMap<NotificationSetting, UpdateNotificationSettingResponse>(MemberList.Source)
                .ForSourceMember(x => x.UserID, opt => opt.DoNotValidate())
                .ForSourceMember(x => x.User, opt => opt.DoNotValidate());
        });

    }

    [Test]
    public void InitializeAppProfile_ShouldCreateMaps()
    {
        //var conf = new AutoMappingConfig();
        _config.AssertConfigurationIsValid();
    }
}
