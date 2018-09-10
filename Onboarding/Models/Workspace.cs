using System.Collections.Generic;

namespace Onboarding.Models
{
    public class Workspace
    {
        public Workspace()
        {
            this.UsersState = new List<UserState>();
            this.Channels = new List<Channel>();
        }
        public string Id { get; set; }

        public string WorkspaceName { get; set; }

        public string  PictureUrl { get; set; }


        public List<Channel> Channels { get; set; }

        public List<Bot> Bots { get; set; }

        public List<UserState> UsersState { get; set; }

        public List<UserWorkspace> UserWorkspaces { get; set; }
    }
}
