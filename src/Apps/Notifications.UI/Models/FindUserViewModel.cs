using System.ComponentModel.DataAnnotations;

namespace Notifications.UI.Models;

public class FindUserViewModel
{
    [Display(Name = "User ID")]
    public int ID { get; set; }
}
