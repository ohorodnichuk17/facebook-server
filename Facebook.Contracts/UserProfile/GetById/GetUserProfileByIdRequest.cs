using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.UserProfile.GetUserProfileById
{
    public class GetUserProfileByIdRequest
    {
        [Required(ErrorMessage = "{PropertyName} is required.")]
        public required Guid UserId { get; init; }
    }
}
