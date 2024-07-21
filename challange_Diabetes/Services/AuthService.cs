using challange_Diabetes.DTO;
using challange_Diabetes.Services;
using challenge_Diabetes.Data;
using challenge_Diabetes.DTO;
using challenge_Diabetes.Helpers;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace challenge_Diabetes.Services
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CloudinaryService _cloudinaryService;
        private readonly JWT _jwt;




        public AuthService( UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, CloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _cloudinaryService = cloudinaryService;
            
        }
        public async Task<AuthModel> RegisterAsync(RegisterModelDTO model)
        {


            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };
            var newuser = await _userManager.FindByNameAsync(model.Username);
            if (newuser != null)
            {
                // توليد اسم مستخدم فريد
                string newUserName;
                do
                {
                    newUserName = $"{model.Username}{new Random().Next(1000, 9999)}";
                    newuser = await _userManager.FindByNameAsync(newUserName);
                } while (newuser != null);

                model.Username = newUserName; // تعيين اسم المستخدم الجديد للنموذج
            }

            // إنشاء مستخدم جديد
            //var newUser = new ApplicationUser { UserName = model.Username, Email = model.Email };
            //var result = await _userManager.CreateAsync(newUser, model.Password);


            //if (await _userManager.FindByNameAsync(model.Username) is not null)
            //  return new AuthModel { Message = "Username is already registered!" };
            var uploadResult = await _cloudinaryService.UploadImageAsync(model.Photo);
            if (uploadResult.Error != null)
            {
                return new AuthModel { Message = uploadResult.Error.Message };

            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                PhoneNumber = model.Phone,
                Email = model.Email,
                address=model.Address,
                Photo = uploadResult.SecureUrl.ToString()
                


            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        continue;
                    }

                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Message = "User registered successfully!",
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                PhotoUrl = uploadResult.SecureUrl.ToString(),
                
            };
        }
        public async Task<AuthModel> Login(TokenRequestModelDTO model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.Message = "success";
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            authModel.PhotoUrl = user.Photo;
            authModel.phone = user.PhoneNumber;

            return authModel;

        }
      
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {       new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
               signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<string> AddRoleAsync(AddRoleModelDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";


        }

        public async Task LogoutAsync()
        {
            var emptycontent = new StringContent("{}", Encoding.UTF8, "application/json");

        }
        

    }
   

}
