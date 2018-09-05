//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Onboarding.Contract;
//using Onboarding.Controllers;
//using Onboarding.Models;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;
//using Onboarding;
//using Onboarding.Services;

//namespace OnboardingTest
//{
//    public class OnboardControllerTest
//    {
//        private IOnboardingService Context;
//        private OnboardingContext _Context;

//        //public OnboardControllerTest(IOnboardingService Context)
//        //{
//        //    this.Context = Context;
//        //}

//        private OnboardingController GetController()
//        {
//            var optionBuilder = new DbContextOptionsBuilder<OnboardingContext>();
//            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
//            _Context = new OnboardingContext(optionBuilder.Options);
//            CreateData(optionBuilder.Options);
//            Context = new OnboardService(_Context);
//            return new OnboardingController(Context);
//        }

//        private void CreateData(DbContextOptions<OnboardingContext> options)
//        {
//            using (var userContext = new OnboardingContext(options))
//            {
//                List<UserAccount> users = new List<UserAccount>()
//            {
//                    new UserAccount()
//                    {
//                        FirstName="Sudarshan",
//                        LastName="K",
//                        EmailId="ssk@gmail.com",
//                        Password="ssk123",
//                        IsVerified=true,
//                        Workspaces = new List<WorkspaceName>()
//                        {
//                            new WorkspaceName()
//                            {
//                                Name="stackroute"
//                            },
//                            new WorkspaceName()
//                            {
//                                Name="sroboeing"
//                            }
//                        }
//                    },

//                     new UserAccount()
//                    {
//                        FirstName="Rahul",
//                        LastName="K",
//                        EmailId="rsk@gmail.com",
//                        Password="rsk123",
//                        IsVerified=true,
//                     Workspaces = new List<WorkspaceName>()
//                        {
//                            new WorkspaceName()
//                            {
//                                Name="stackroute"
//                            },
//                            new WorkspaceName()
//                            {
//                                Name="sroboeing"
//                            }
//                        }

//                    },

//                  new UserAccount()
//                    {
//                        FirstName="Kiran",
//                        LastName="Kumar",
//                        EmailId="ksk@gmail.com",
//                        Password="ksk123",
//                        IsVerified=true,
//                     Workspaces = new List<WorkspaceName>()
//                        {
//                            new WorkspaceName()
//                            {
//                                Name="stackroute"
//                            },
//                            new WorkspaceName()
//                            {
//                                Name="sroboeing"
//                            }
//                        }

//                    },


//            };

//                List<Workspace> workspaces = new List<Workspace>()
//                {
//                    new Workspace()
//                    {
//                        WorkspaceName = "sroboeing",
//                        PictureUrl = "sro.tldm",
//                    },
//                    new Workspace()
//                    {
//                        WorkspaceName= "stackroute",
//                        PictureUrl="stackroute.tldm"
//                    }
//                };

//                List<UserState> userStates = new List<UserState>()
//                {
//                    new UserState()
//                    {
//                        EmailId="ssk@gmail.com",
//                        Otp = "xyz123"
//                    },
//                    new UserState()
//                    {
//                        EmailId = "rsk@gmail.com",
//                        Otp = "abc123"
//                    },
//                    new UserState()
//                    {
//                        EmailId = "ksk@gmail.com",
//                        Otp = "pqr123"
//                    }
//                };

//                _Context.UserAccount.AddRange(users);
//                // _Context.SaveChanges();

//                _Context.Workspace.AddRange(workspaces);
//                // _Context.SaveChanges();

//                _Context.UserState.AddRange(userStates);
//                _Context.SaveChanges();
//            }
//        }

//        [Fact]
//        public async Task OnboardUserTest()
//        {
//            var Model = new LoginViewModel()
//            {
//                EmailId = "ssk@gmail.com",
//                Password = "ssk123",
//                Workspace = "sroboeing"
//            };
//            var controller = GetController();

//            var result = await controller.OnboardUser(Model);
//            Console.WriteLine(result);
//            var resultAsOkObjectResult = result as OkObjectResult;
//            Assert.Equal(200, resultAsOkObjectResult.StatusCode);

//        }

//        [Fact]
//        public async Task GetALlWorkspacesTest()
//        {
//            string email = "rsk@gmail.com";
//            var controller = GetController();

//            var result = await controller.GetAllWorkspace(email);
//            Console.WriteLine(result);
//            var resultAsOkObjectResult = result as OkObjectResult;
//            var userReturn = resultAsOkObjectResult.Value as List<string>;

//            Assert.Equal(200, resultAsOkObjectResult.StatusCode);

//        }

//        [Fact]
//        public async Task LoginTest()
//        {
//            var Model = new LoginViewModel()
//            {
//                EmailId = "ssk@gmail.com",
//                Password = "ssk123",
//                Workspace = "sroboeing"
//            };

//            var controller = GetController();

//            var result = await controller.Login(Model);
//            var resultAsOkObjectResult = result as OkObjectResult;
//            var response = (resultAsOkObjectResult.Value as object);
//            var jwtstring = JObject.Parse(JsonConvert.SerializeObject(response));
//            var token = new JwtSecurityToken(jwtEncodedString: jwtstring["token"].ToString());
//            // Console.WriteLine("email => " + token.Claims.First(c => c.Type == "email").Value);
//            //Console.WriteLine(jwtstring["token"]);

//            Model.EmailId.Equals(token.Claims.First(c => c.Type == "email").Value);
//        }


//        [Fact]

//        public async Task PersonalDetailsTest()
//        {
//            var user = new UserAccount()
//            {
//                FirstName = "Kiran",
//                LastName = "Kumar",
//                EmailId = "ksk@gmail.com",
//                Password = "ksk123",
//                IsVerified = true,
//                Workspaces = new List<WorkspaceName>()
//                        {
//                            new WorkspaceName()
//                            {
//                                Name="stackroute"
//                            },
//                            new WorkspaceName()
//                            {
//                                Name="sroboeing"
//                            }
//                        }

//            };

//            var controller = GetController();

//            var result = await controller.PersonalDetails(user);
//            var resultAsOkObjectResult = result as OkObjectResult;

//            Assert.Equal(200, resultAsOkObjectResult.StatusCode);
//        }


//        [Fact]
//        public async Task OnboardUserFromWorkspaceTest()
//        {
//            var Model = new LoginViewModel()
//            {
//                EmailId = "ssk@gmail.com",
//                Password = "ssk123",
//                Workspace = "sroboeing"
//            };

//            var controller = GetController();
//            var result = await controller.OnboardUserFromWorkspace(Model);
//            var resultAsOkObjectResult = result as OkResult;

//            Assert.Equal(200, resultAsOkObjectResult.StatusCode);
//        }

//        [Fact]
//        public async Task CreateWorkspaceTest()
//        {
//            var Workspace = new Workspace()
//            {
//                WorkspaceName = "sroboeing",
//                PictureUrl = "boeing.url",
//            };

//            var controller = GetController();
//            var result = await controller.CreateWorkspace(Workspace);
//            var resultAsOkObjectResult = result as OkResult;

//            Assert.Equal(200, resultAsOkObjectResult.StatusCode);

//        }

//        [Fact]
//        public async Task VerifyTest()
//        {
//            string Otp = "xyz1234";

//            var controller = GetController();
//            var result = await controller.Verify(Otp);
//            var resultAsOkObjectResult = result as UnauthorizedResult;

//            Assert.Equal(401, resultAsOkObjectResult.StatusCode);
//        }

//    }
//}