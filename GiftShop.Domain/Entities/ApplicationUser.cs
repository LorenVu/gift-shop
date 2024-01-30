﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace GiftShop.Domain.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public bool Blocked { get; set; }

    [StringLength(50)]
    public string NickName { get; set; } = string.Empty;

    [StringLength(255)]
    public string Avatar { get; set; } = string.Empty;

    public int RenameCount { get; set; }

    public long UniqueDisplayId { get; set; }

    [StringLength(255)]
    public string DeviceID { get; set; } = string.Empty;

    public DateTime BirthDay { get; set; }

    [StringLength(255)]
    public string Address { get; set; } = string.Empty;
}
