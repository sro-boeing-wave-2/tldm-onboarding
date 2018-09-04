using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onboarding.Contract;
using Onboarding.Controllers;
using Onboarding.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace OnboardingTest
{
    public class OnboardControllerTest
    {
        private readonly IOnboardingService Context;
        private OnboardingController controller;
        private OnboardingContext _Context;

        public OnboardingController GetController()
        {
            var optionBuilder = new DbContextOptionsBuilder<OnboardingContext>();
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _Context = new OnboardingContext(optionBuilder.Options);
            CreateData(optionBuilder.Options);
            return new OnboardingController(Context);
        }

        public void CreateData(DbContextOptions<OnboardingContext> options)
        {
            using (var userContext = new OnboardingContext(options))
            {
                List<UserAccount> users = new List<UserAccount>()
           {
                   new UserAccount()
                   {
                       FirstName="Sudarshan",
                       LastName="K",
                       EmailId="ssk@gmail.com",
                       Password="ssk123",
                       IsVerified=true,
                       Workspaces = new List<WorkspaceName>()
                       {
                           new WorkspaceName()
                           {
                               Name="stackroute"
                           },
                           new WorkspaceName()
                           {
                               Name="sroboeing"
                           }
                       }
                   },

                    new UserAccount()
                   {
                       FirstName="Rahul",
                       LastName="K",
                       EmailId="rsk@gmail.com",
                       Password="rsk123",
                       IsVerified=true,
                    Workspaces = new List<WorkspaceName>()
                       {
                           new WorkspaceName()
                           {
                               Name="stackroute"
                           },
                           new WorkspaceName()
                           {
                               Name="sroboeing"
                           }
                       }

                   }
           };

                _Context.UserAccount.AddRange(users);
                _Context.SaveChanges();
            }
        }

        [Fact]
        public void OnboardUserTest()
        {
            var Model = new LoginViewModel()
            {
                EmailId = "ssk@gmail.com",
                Password = "ssk123",
                Workspace = ""
            };
            var controller = GetController();

            var result = controller.OnboardUser(Model);
            var resultAsOkObjectResult = result as OkObjectResult;
            Assert.Equal(200, resultAsOkObjectResult.StatusCode);
        }
    }
}