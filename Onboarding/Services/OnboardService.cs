using Chilkat;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Onboarding.Contract;
using Onboarding.Models;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Onboarding.Services
{
    public class OnboardService : IOnboardingService
    {
        private readonly OnboardingContext _context;

        public OnboardService(OnboardingContext context)
        {
            _context = context;
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public string SendMail(LoginViewModel value)
        {
            //generate token 
            string token = RandomString(6);

            //instantiate mimemsg
            var message = new MimeMessage();

            //from add
            message.From.Add(new MailboxAddress("TL;DM", "talklessDM@gmail.com"));
            //to add
            message.To.Add(new MailboxAddress("Hi", value.EmailId));
            //subject
            message.Subject = "Verification Mail";

            //body

            if (value.Workspace == null)
            {
                message.Body = new TextPart("plain")
                {
                    Text = "Welcome to TL;DM your temporaray token is  " + token + " Welcome Aboard!"
                };

            }
            else if (value.Password == "Bot" && value.Workspace == "Bot")
            {
                JsonObject claims = new JsonObject();
                claims.AppendString("Email", value.EmailId);
                //claims.AppendString("UserID", user.Id);

                JWTTokenService jwt = new JWTTokenService();

                var JWToken = jwt.GetToken(claims);

               // return claims;
                message.Body = new TextPart("plain")
                {
                    Text =  " Your JWT Token is " + JWToken + " Welcome Aboard!"
                };

            }
            else
            {
                message.Body = new TextPart("plain")
                {
                    Text = "Welcome to TL;DM You have been invited to join " + value.Workspace + " Your Temporary token is " + token + " Welcome Aboard!"
                };
            }

            //Configure and send email

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("talklessDM@gmail.com", "tldm1234");
                client.Send(message);
                client.Disconnect(true);

            }
            return token;
        }

        public async Task<Object> CreateWorkspace(Workspace workspace)
        {
            
            var unique = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceName == workspace.WorkspaceName);
            if (unique == null)
            {
                _context.Workspace.Add(workspace);
                _context.SaveChanges();
                return workspace;
            }
            return null;
        }

        public async Task<Object> OnboardUser(LoginViewModel value)
        {

            var workspace = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceName == value.Workspace);

            if (workspace != null)
            {
                string token = SendMail(value);

                UserState user = new UserState() { EmailId = value.EmailId, Otp = token };
                workspace.UsersState.Add(user);

                var newuser = await _context.UserAccount.Include(i => i.Workspaces).FirstOrDefaultAsync(x => x.EmailId == value.EmailId);

                if (newuser == null)
                {
                    UserAccount details = new UserAccount() { EmailId = value.EmailId };
                    await _context.UserWorkspaces.AddAsync(new UserWorkspace { Workspace = workspace, UserAccount = details });
                    _context.SaveChanges();
                }
                _context.Workspace.Update(workspace);
                _context.SaveChanges();
                return user;
            }
            return null;
        }

        public async Task<JsonObject> VerifyUser(string token)
        {        
            var user = await _context.UserState.FirstOrDefaultAsync(x => x.Otp == token);
            if (user != null)
            {
                JsonObject claims = new JsonObject();
                claims.AppendString("Email", user.EmailId);
                claims.AppendString("UserID", user.Id);

                return claims;
            }

            return null;
        }


        public async Task<JsonObject> VerifyInvitedUser(LoginViewModel token)
        {            
            var space = await _context.Workspace.Include(i => i.UsersState).FirstOrDefaultAsync(x => x.WorkspaceName == token.Workspace);            
            var user = space.UsersState.FirstOrDefault(x => x.Otp == token.Password);
            if (user != null)
            {
                JsonObject claims = new JsonObject();
                claims.AppendString("Email", user.EmailId);
                claims.AppendString("UserID", user.Id);

                return claims;
            }

            return null;
        }

        public async Task<UserAccount> PersonalDetails(UserAccount user)
        {
            var newuser = await _context.UserAccount.Include(i => i.Workspaces).FirstOrDefaultAsync(x => x.EmailId == user.EmailId);

            if (newuser != null)
            {
                newuser.FirstName = user.FirstName;
                newuser.LastName = user.LastName;
                newuser.Password = user.Password;
                newuser.IsVerified = true;

                var workspaceName = user.Workspaces.Select(i => i.Name).ToList();
                var v = newuser.Workspaces.Exists(x => x.Name == workspaceName[0]);
                if (!v)
                {
                    newuser.Workspaces.AddRange(user.Workspaces);
                }

                _context.UserAccount.Update(newuser);
                _context.SaveChanges();
            }
            return newuser;
        }

        public async Task<Workspace> WorkSpaceDetails(Workspace workspace)
        {
            var space = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceName == workspace.WorkspaceName);
            space.Channels.AddRange(workspace.Channels);
            space.Bots.AddRange(workspace.Bots);
            space.PictureUrl = workspace.PictureUrl;
            _context.Workspace.Update(space);
            _context.SaveChanges();
            return space;
        }

        public async Task<Object> OnboardUserFromWorkspace(LoginViewModel value)
        {
            var workspace = await _context.Workspace.Include(i => i.UsersState).FirstOrDefaultAsync(x => x.WorkspaceName == value.Workspace);
            var user = workspace.UsersState.FirstOrDefault(x => x.EmailId == value.EmailId);

            if (user == null || !user.IsJoined)
            {
                var otp = SendMail(value);
                var newuser = await _context.UserAccount.Include(i => i.Workspaces).FirstOrDefaultAsync(x => x.EmailId == value.EmailId);

                if (newuser == null)
                {
                    var details = new UserAccount() { EmailId = value.EmailId };
                    await _context.UserWorkspaces.AddAsync(new UserWorkspace { Workspace = workspace, UserAccount = details });
                    await _context.UserAccount.AddAsync(details);
                    _context.SaveChanges();
                }
                UserState newUser = new UserState() { EmailId = value.EmailId, Otp = otp };
                workspace.UsersState.Add(newUser);
                _context.Workspace.Update(workspace);
                _context.SaveChanges();
                return newUser;

            }
            return null;

        }

        public async Task<IEnumerable> GetAllWorkspace(string value)
        {
            var user = await _context.UserAccount.Include(t => t.Workspaces).FirstOrDefaultAsync(x => x.EmailId == value);
            if (user != null)
            {
                var list = user.Workspaces.Select(v => v.Name);

                return list;
            }

            return null;
            
        }

        public async Task<JsonObject> Login(LoginViewModel login)
        {
            var user = await _context.UserAccount.Where(existUser =>
           existUser.EmailId == login.EmailId
           && existUser.Password == login.Password)
           .FirstOrDefaultAsync();

            if (user != null)
            {
                    JsonObject claims = new JsonObject();
                    claims.AppendString("Email", user.EmailId);
                    claims.AppendString("UserID", user.Id);

                    return claims;
                }

            return null;
        }

        
        public async Task<Workspace> GetWorkspaceByName(string name)
        {
            var space = await _context.Workspace.Include(x => x.UsersState).Include(y => y.Channels)
                .Include(z => z.UserWorkspaces).FirstOrDefaultAsync(i => i.WorkspaceName == name);
            return space;
        }

        public async Task<string> BotVerification(LoginViewModel value)
        {
            var token =  SendMail(value);
            return token;
        }

    }
}
