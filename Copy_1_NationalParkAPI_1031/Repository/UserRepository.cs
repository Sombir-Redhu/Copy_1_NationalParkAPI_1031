using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Copy_1_NationalParkAPI_1031.Data;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Copy_1_NationalParkAPI_1031.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly AppSettings _appSettings;  //JWT
        public UserRepository(ApplicationDbContext context/*, IOptions<AppSettings> appSettings*/) //JWT
        {
            _context = context;
            //_appSettings = appSettings.Value;  //JWT
        }
        public User Authenticate(string userName, string password)
        {
            var userInDb=_context.Users.FirstOrDefault(u=>u.UserName == userName && u.Password == password);
            if (userInDb == null) return null;
            //JWT
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor()
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, userInDb.Id.ToString()),
            //        new Claim(ClaimTypes.Role, userInDb.Role)
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            //    SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //userInDb.Token = tokenHandler.WriteToken(token);
            ////***
            userInDb.Password = "";
            return userInDb;

        }

        public bool IsUniqueUser(string userName)
        {
           var userInDb = _context.Users.FirstOrDefault(u=>u.UserName == userName);
           if (userInDb == null) return true;
           return false;
        }

        public User Register(string userName, string password)
        {
            var user = new User()
            {
                UserName = userName,
                Password = password,
                Role = "Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
