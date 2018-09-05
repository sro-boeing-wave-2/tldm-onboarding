using Onboarding.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onboarding.Contract
{
    public interface IOnboardingService
    {

        Task<Object> OnboardUser(LoginViewModel email);

        Task OnboardUserFromWorkspace(LoginViewModel user);

        Task<Object> VerifyUser(string otp);

        //Task JwtGenerator(LoginViewModel model);

        //Task SendMail(string email);

        //Task Login(LoginViewModel model);

        Task PersonalDetails(UserAccount user);

        Task WorkSpaceDetails(Workspace workspace);

        Task CreateWorkspace(Workspace workspace);

        Task<Object> Login(LoginViewModel login);

        Task<IEnumerable> GetAllWorkspace(string emailId);
    }
}
