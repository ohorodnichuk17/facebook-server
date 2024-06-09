using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.UserProfile.DeleteUser;

public class DeleteUserRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required string UserId { get; init; }
}