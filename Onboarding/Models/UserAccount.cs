using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onboarding.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        public string  FirstName { get; set; }

        public string  LastName { get; set; }

        public string EmailId { get; set; }

        public string  Password { get; set; }

        public bool IsVerified { get; set; }

        public List<WorkspaceName> Workspaces { get; set; }

        public List<UserWorkspace> UserWorkspaces { get; set; }
    }
}
