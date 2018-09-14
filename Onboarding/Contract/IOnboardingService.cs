using Onboarding.Models;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Onboarding.Contract
{
    public interface IOnboardingService
    {

        Task<Object> OnboardUser(LoginViewModel email);

        Task<Object> OnboardUserFromWorkspace(LoginViewModel user);

        Task<Object> VerifyUser(String otp);

        Task<UserAccount> PersonalDetails(UserAccount user);

        Task<Workspace> WorkSpaceDetails(Workspace workspace);

        Task CreateWorkspace(Workspace workspace);

        Task<Object> Login(LoginViewModel login);

        Task<IEnumerable> GetAllWorkspace(string emailId);

        Task<Workspace> GetWorkspaceByName(string name);
        Task<Object> VerifyInvitedUser(LoginViewModel otp);
    }
}
