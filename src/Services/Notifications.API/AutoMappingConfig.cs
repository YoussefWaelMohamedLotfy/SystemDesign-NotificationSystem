using AutoMapper;
using Notifications.API.Models;
using Notifications.Core.Requests;
using Notifications.Core.Responses;

namespace Notifications.API;

public class AutoMappingConfig : Profile
{
    public AutoMappingConfig()
    {
        // Requests to Domain Models
        CreateMap<CreateUserRequest, User>();
        CreateMap<CreateNotificationSettingRequest, NotificationSetting>().ReverseMap();
        CreateMap<UpdateUserRequest, User>();
        CreateMap<UpdateNotificationSettingRequest, NotificationSetting>().ReverseMap();

        // Domain Models to Responses
        CreateMap<User, CreateUserResponse>();
        CreateMap<NotificationSetting, CreateNotificationSettingResponse>();
        CreateMap<User, UpdateUserResponse>();
        CreateMap<NotificationSetting, UpdateNotificationSettingResponse>();
        CreateMap<User, GetUserResponse>();
        CreateMap<NotificationSetting, GetNotificationSettingResponse>();
    }
}
