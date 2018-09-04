using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Onboarding.Contract;
using Onboarding.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            else
            {
                message.Body = new TextPart("plain")
                {
                    Text = "Welcome to TL;DM You have been invited to join " + value.Workspace + " Your Temporary token is " + token + "Welcome Aboard!"
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

        public async Task OnboardUser(LoginViewModel value)
        {
            string token = SendMail(value);

            var workspace = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceName == value.Workspace);

           // var user = await _context.UserAccount.FirstAsync(x => x.Workspaces.FirstOrDefault(u => u.WorkspaceName == value.EmailId))
         
            if (workspace != null)
            {
                UserState user = new UserState() { EmailId = value.EmailId, Otp = token };
                //_context.Workspace.Where(x => x.WorkspaceName == value.Workspace).Include(x => x.UsersState);
               workspace.UsersState.Add(user);
               
                _context.Workspace.Update(workspace);
               // _context.UserState.Add(user);
                _context.SaveChanges();
            }
        }

        public async Task<Object> VerifyUser(string token)
        {
            //var user = await _context.UserAccount.FirstOrDefaultAsync(x => x.Password == token);
            var user = await _context.UserState.FirstOrDefaultAsync(x => x.Otp == token);
            //user.IsVerified = true;
            user.IsJoined = true;
            _context.SaveChanges();
            if (user != null)
            {
                var claims = new[]
                   {
                       new Claim(JwtRegisteredClaimNames.Email,user.EmailId),
                   };

                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));
                var Jwtoken = new JwtSecurityToken(
                    issuer: "http://oec.com",
                    audience: "http://oec.com",
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256));

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(Jwtoken),
                    expiration = Jwtoken.ValidTo
                };
            }

            return user;
        }

        public async Task OnboardUserFromWorkspace(LoginViewModel value)
        {
            var otp = SendMail(value);

            UserState user = new UserState() { EmailId = value.EmailId, Otp = otp };
            _context.UserState.Add(user);
            _context.SaveChanges();
        }

        public async Task CreateWorkspace(Workspace workspace)
        {
            //var  unique =  _context.UserAccount.Include(i => i.Workspaces).Where(x => x.Workspaces.TrueForAll(y => y.WorkspaceName == workspace.WorkspaceName));

            var unique = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceName == workspace.WorkspaceName);

           // var unique = await _context.Workspace.FirstOrDefaultAsync(i => workspace.Workspaces.Any(y => y.WorkspaceName == i.WorkspaceName));
            if (unique == null)
            {
                _context.Workspace.Add(workspace);
              // UserAccount user =  new UserAccount { new List<Workspace>() { new Workspace { WorkspaceName = workspace.WorkspaceName } } };
                _context.SaveChanges();
            }

        }

        //public async Task<IEnumerable> GetAllWorkspace(string value)
        //{
            //var list = _context.UserAccount.Include(x => x.Workspaces).Select(x => x.Workspaces.SelectMany(y => y.Name));

            //return list;
       // }

        public async Task PersonalDetails(UserAccount user)
        {
            await _context.UserAccount.AddAsync(user);
            _context.SaveChanges();
        }

        public async Task WorkSpaceDetails(Workspace workspace)
        {
            //var space = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceName == workspace.WorkspaceName);
            //workspace.Id = space.Id;
            _context.Workspace.Update(workspace);
            _context.SaveChanges();
        }

    }
}
