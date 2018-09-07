using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onboarding.Models
{
    public class UserWorkspace
    {
        public string UserId { get; set; }
        public UserAccount UserAccount { get; set; }
        public string WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
    }
}
