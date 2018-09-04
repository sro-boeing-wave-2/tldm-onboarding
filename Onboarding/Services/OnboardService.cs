using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Onboarding.Contract;
using Onboarding.Models;
using System;
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

            UserAccount user = new UserAccount() { EmailId = value.EmailId, Password = token };
            _context.UserAccount.Add(user);
            _context.SaveChanges();
        }

        public async Task<Object> VerifyUser(string token)
        {
            var user = await _context.UserAccount.FirstOrDefaultAsync(x => x.Password == token);
            var person = await _context.UserState.FirstOrDefaultAsync(x => x.Otp == token);
            if (user != null || person != null)
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
            _context.Workspace.Add(workspace);
            _context.SaveChanges();
        }

        public async Task GetAllWorkspace(string value)
        {
            var list = _context.UserAccount.Include(x => x.Workspaces).Select(x => x.Workspaces.SelectMany(y => y.WorkspaceName));
        }

    }
}
