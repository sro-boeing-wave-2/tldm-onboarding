using Chilkat;
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

        Task<JsonObject> VerifyUser(String otp);

        Task<UserAccount> PersonalDetails(UserAccount user);

        Task<Workspace> WorkSpaceDetails(Workspace workspace);

        Task<Object> CreateWorkspace(Workspace workspace);

        Task<JsonObject> Login(LoginViewModel login);

        Task<IEnumerable> GetAllWorkspace(string emailId);

        Task<Workspace> GetWorkspaceByName(string name);
        Task<JsonObject> VerifyInvitedUser(LoginViewModel otp);
    }
}
