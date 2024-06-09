using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.User;

public class UserProfileEntity
{
    public Guid Id { get; set; }
    public string? CoverPhoto { get; set; }
    public string? Biography { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsProfilePublic { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public UserEntity UserEntity { get; set; }
    [ForeignKey("UserEntity")]
    public Guid UserId { get; set; }
}
