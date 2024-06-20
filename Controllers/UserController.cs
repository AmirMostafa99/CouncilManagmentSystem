using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Reflection.PortableExecutable;
using CouncilsManagmentSystem.Migrations;

namespace CouncilsManagmentSystem.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IConfiguration _configuration;
        private readonly IMailingService _mailingService;
        private readonly IUserServies _userServies;
        private readonly IDepartmentServies _departmentServies;
        public readonly ICollageServies _collageServies;
        private readonly IWebHostEnvironment _environment;
        private readonly ICouncilMembersServies _councilMembersServies;
        private readonly IPermissionsServies _permissionsServies;

 


       

        // private readonly JwtConfig _jwtConfig;
        public UserController(UserManager<ApplicationUser> usermanager, ApplicationDbContext context, IConfiguration configuration, IMailingService mailingService , RoleManager<IdentityRole> rolemanager, ICollageServies collageServies, IWebHostEnvironment environment, IDepartmentServies departmentServies, IUserServies userServies, ICouncilMembersServies councilMembersServies  , IPermissionsServies permissionsServies )
        {
            _context = context;
            _usermanager = usermanager;
            _rolemanager = rolemanager;
            _configuration = configuration;
            //  _jwtConfig = jwtConfig;
            _mailingService = mailingService;

            _userServies = userServies;
            _environment = environment;
            _collageServies = collageServies;
            _departmentServies = departmentServies;
            _councilMembersServies = councilMembersServies;
            _permissionsServies=permissionsServies;
        }

       







        [Authorize]
       // [Authorize(Roles = "SuperAdmin,SubAdmin")]
        [Authorize(Policy = "RequireAddMembersPermission")]
        [HttpPost(template: "AddUserManual")]
        public async Task<IActionResult> Adduser( AddUserDTO user)
        {
            
            if (ModelState.IsValid)
            {
                var adduser = new ApplicationUser
                {
                    FullName = user.FullName,
                    UserName = user.Email,
                    Email = user.Email,
                    PhoneNumber = user.phone,
                    Birthday = user.Birthday,
                    academic_degree = user.academic_degree,
                    functional_characteristic = user.functional_characteristic,
                    DepartmentId = user.DepartmentId,
                    administrative_degree = user.administrative_degree

                };

                var password = Guid.NewGuid().ToString("N").Substring(0, 8);
                
                adduser.PasswordHash = password;
                adduser.IsVerified = false;
                await _usermanager.CreateAsync(adduser);
                await _context.SaveChangesAsync();
                return Ok("User successfully added");
            }

            return BadRequest("There is an error in your data.");
        }




        [Authorize]

        [Authorize(Policy = "RequireAddMembersByExcelPermission")]

        [HttpPost(template: "AddUsersBySheet")]
        public async Task<IActionResult> UploadFiles(string iduser ,IFormFile file)
        {
            var per=await _context.permissionss.FirstOrDefaultAsync(x=>x.userId==iduser);
            if(per.AddMembersByExcil!=true)
            {
                return BadRequest("Idont have this permission????????????????????????");
            }
            if (file != null && file.Length > 0)
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assume we're using the first sheet

                    int rowCount = worksheet.Dimension.Rows;
                    //for to all rows of sheet 
                    for (int row = 2; row <= rowCount; row++) // Skip header row
                    {
                        try
                        {
                            string FullName = worksheet.Cells[row, 1].Value?.ToString();
                            string email = worksheet.Cells[row, 2].Value?.ToString();
                            string academic_degree = worksheet.Cells[row, 3].Value?.ToString();
                            DateTime birthday = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString());
                            string phone_number = worksheet.Cells[row, 5].Value?.ToString();
                            string collage = worksheet.Cells[row, 6].Value?.ToString();
                            string department = worksheet.Cells[row, 7].Value?.ToString();
                            string functional_characteristic = worksheet.Cells[row, 8].Value?.ToString();
                            string administrative_degree = worksheet.Cells[row, 9].Value?.ToString();
                            //check if this collage is exist
                            var isCollage = await _collageServies.GetCollageByName(collage);
                            if (isCollage == null && collage != null)
                            {
                                return BadRequest("There is an error in your collage data.");
                            }
                            //check if this department is exist
                            var isDepartment = new Department();
                            if (isCollage != null)
                            {
                                isDepartment = await _departmentServies.Get_dep_idcollage(isCollage.Id, department);
                            }
                            if (isDepartment == null && department != null)
                            {
                                return BadRequest("There is an error in your department data.");
                            }


                            var user = new ApplicationUser
                            {

                                FullName = FullName,
                                UserName = email,
                                Email = email,
                                Birthday = birthday,
                                PhoneNumber = phone_number,
                                academic_degree = academic_degree,
                                functional_characteristic = functional_characteristic,
                                administrative_degree = administrative_degree
                            };
                            if (department != null)
                            {
                                user.DepartmentId = isDepartment.id;
                            }


                            // Generate a random password
                            var password = Guid.NewGuid().ToString("N").Substring(0, 8);


                            user.PasswordHash = password;
                            // Save changes to the database
                            await _userServies.CreateUserAsync(user);

                        }

                        catch (Exception ex)
                        {
                            // Log the exception for further investigation
                            return BadRequest($"An error occurred: {ex.Message}");
                        }
                    }

                    return Ok("Uploaded successfully!");
                }
            }
            return BadRequest("No file or file empty.");
        }

        [HttpGet(template: "GetAllUsers")]
        public async Task<IActionResult> getAlluser()
        {
            var users = await _userServies.getAllUser();
            return Ok(users);
        }
        //////Get user By name
        [HttpGet(template: "GetUserByname")]
        public async Task<IActionResult> getuserByname(string fullname)
        {
            var user = await _userServies.getuserByFullName(fullname);
            return Ok(user);
        }
        /// ////////////get user by email

        [HttpGet(template: "GetUserByEmail")]
        public async Task<IActionResult> getuserByEmail(string email)
        {
            var user = await _userServies.getuserByEmail(email);
            return Ok(user);
        }

        //////all user by Name
        [HttpGet(template: "GetAllUserByname")]
        public async Task<IActionResult> getAlluserByname(string fullname)
        {
            var users = await _userServies.getAllUserByname(fullname);
            return Ok(users);
        }


        //update user
        [HttpPut(template: "UpdateUser")]
        public async Task<IActionResult> updateUser(string id, [FromForm] updateuserDTO user)
        {
            //token
            //var userEmail = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            if (ModelState.IsValid)
            {
                var search = await _userServies.getuserByid(id);
                if (search == null)
                {
                    return BadRequest("This user not found !");
                }
                string path = Path.Combine(_environment.ContentRootPath, "images");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (user.img != null)
                {
                    path = Path.Combine(path, user.img.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await user.img.CopyToAsync(stream);


                        search.img = user.img.FileName;



                    }
                }
                search.FullName = user.FullName;
                search.Email = user.Email;
                search.Birthday = user.Birthday;
                search.PhoneNumber = user.phone;
                search.UserName = user.Email;
                search.administrative_degree = user.administrative_degree;
                search.functional_characteristic = user.functional_characteristic;
                search.academic_degree = user.academic_degree;
                _userServies.Updateusert(search);
                return Ok(search);

            }
            return BadRequest("you have wrong in your data. ");


        }

        [HttpGet(template: "GetAllUserByIdDepartment")]
        public async Task<IActionResult> getAlluserByIdDepartment(int id)
        {
            var users = await _userServies.getAllUserByIdDepartment(id);

            return Ok(users);
        }


        [HttpGet(template: "GetAllUserByIdCollage")]
        public async Task<IActionResult> getAlluserByIdCollage(int id)
        {
            var users = await _userServies.getAllUserByIdCollage(id);

            return Ok(users);
        }


        //[Authorize(Roles = "BasicUser,Secretary,ChairmanOfTheBoard")]
        [HttpPost("ActivateEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _usermanager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    // Generate a random password
                    var password = Guid.NewGuid().ToString("N").Substring(0, 8);

                    // Send the generated password to the user via email
                    var subject = "Activate Your Council Management System Account Not-replay";
                    var body = $"Your password is {password} Please use it to log in";
                    await _mailingService.SendEmailAsync(dto.Email, subject, body);

                    // Save the generated password for the user in the database
                    existingUser.PasswordHash = _usermanager.PasswordHasher.HashPassword(existingUser, password);
                    existingUser.IsVerified = true;  
                    await _usermanager.UpdateAsync(existingUser);

                    return Ok("Password successfully generated and sent via email.");
                }

                return BadRequest(new AuthenticationResault()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                         "Email does not exist."
                    }
                });
            }
            return BadRequest(new AuthenticationResault()
            {
                Errors = new List<string>()
                {
                     "Invalid payload."
                },
                Result = false
            });
        }

        //[AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                // check if the user exist
                var existing_user = await _usermanager.FindByEmailAsync(dto.Email);
                
                if (existing_user == null)
                {
                    return BadRequest("The User Not Exist");

                }
                if (existing_user.IsVerified == false)
                {
                    return BadRequest("The User Not Allow To Login");

                }
                var isCorrect = await _usermanager.CheckPasswordAsync(existing_user, dto.Password);
                if (!isCorrect)
                {
                    return BadRequest("Invalid Password");
                }
                var UserPermission = await _permissionsServies.getObjectpermissionByid(existing_user.Id);
                var jwrToken = GeneratJwtToken(existing_user);
               
                return Ok(new AuthenticationResault()
                {
                    Permission = UserPermission,
                    Token = jwrToken,
                    Result = true
                                 


                });

            }

            return BadRequest(new AuthenticationResault()
            {
                Errors = new List<string>()
                {
                    "Invalid Payload"
                },
                Result = false
            });
        }


        //[AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] UserForgetPasswordRequestDto dto)
        {
            if (ModelState.IsValid)
            {

                // check if the user exist
                var existing_user = await _usermanager.FindByEmailAsync(dto.Email);

                if (existing_user == null)
                {
                    return BadRequest("The User Not Exist");
                }
                // Generate a random 5-digit OTP
                Random rand = new Random();
                int otp = rand.Next(10000, 99999);
                // Send the generated OTP to the user via email
                var subject = "Council Management System Password Reset Not-replay";
                var body = $"Dear User,\n\nYou have requested to reset your password for the Council Management System. Please use this OTP to reset your password. Your (OTP) is: {otp}.";

                await _mailingService.SendEmailAsync(dto.Email, subject, body);


                // Save the generated OTP for the user in the database
                existing_user.OTP = otp;
                await _usermanager.UpdateAsync(existing_user);

                return Ok("OTP successfully generated and sent via email.");

            }
            return BadRequest(new AuthenticationResault()
            {
                Errors = new List<string>()
                {
                    "Invalid Payload"
                },
                Result = false
            });

        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var existing_user = await _usermanager.FindByEmailAsync(dto.Email);
            if (existing_user == null)
            {
                return BadRequest("The User Not Exist");
            }

            // Check if the OTP provided by the user matches the one in the database
            var otpFromDb = existing_user.OTP; // Assuming OTP is stored in the User entity
            if (otpFromDb == null || otpFromDb != dto.OTP)
            {
                return BadRequest("Invalid OTP");
            }
            var jwrToken = GeneratJwtToken(existing_user);
            // Reset the password for the user
            await _usermanager.ResetPasswordAsync(existing_user, jwrToken, dto.NewPassword);

            // Password reset successful, you can delete the OTP from the user entity
            existing_user.OTP = null;
            await _usermanager.UpdateAsync(existing_user);

            return Ok(new AuthenticationResault()
            {
                Token = jwrToken,
                Result = true,
                Errors = new List<string>()
                {
                    "ResetPassword successfully done"
                },
            });
        }
        //[AllowAnonymous]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found.");

            var result = await _usermanager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new AuthenticationResault()
            {
                Errors = new List<string>()
                    {
                        "Password changed successfully."
                    },
                Result = true,

            });
        }


        //[AllowAnonymous]
        //[AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok(new AuthenticationResault()
            {
                Result = true,
                Errors = new List<string>()
                {
                    "Logout successful."
                }
            });
        }

        // [Authorize(Roles = "SuperAdmin,SubAdmin")]
        [HttpPut("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser([FromBody] DeactivateUserRequestDto dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return NotFound("Invalid Email");
            }

            user.IsVerified = false;

            var result = await _usermanager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new AuthenticationResault()
            {
                Errors = new List<string>()
                {
                    "The User Deactivated"
                },
                Result = true,
                           
            });
        }

        //TODO: SuberAdmin add role to users
       // [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDto dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var roleExists = await _rolemanager.RoleExistsAsync(dto.RoleName);
            if (!roleExists)
            {
                return BadRequest("Role does not exist.");
            }

            var userHasRole = await _usermanager.IsInRoleAsync(user, dto.RoleName);

            if (dto.IsSelected && !userHasRole)
            {
                var result = await _usermanager.AddToRoleAsync(user, dto.RoleName);
                if (result.Succeeded)
                {
                    return Ok("Role assigned successfully.");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else if (!dto.IsSelected && userHasRole)
            {
                var result = await _usermanager.RemoveFromRoleAsync(user, dto.RoleName);
                if (result.Succeeded)
                {
                    return Ok("Role removed successfully.");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return Ok("No changes made to the role.");
            }
        }
        //TODO: suberadmin Add permssion to Users
        

        //TODO: when u assign role to user add this role in the token


        private string GeneratJwtToken(ApplicationUser user)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            // Token descriptor
            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, value: DateTime.Now.ToUniversalTime().ToString())
                }),


                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = JwtTokenHandler.CreateToken(TokenDescriptor);
            var jwtToken = JwtTokenHandler.WriteToken(token);
            return jwtToken;
        }




        [Authorize]
        [HttpGet(template: "GetAllNextCouncilforUser")]
        public async Task<IActionResult> getallnextcouncilbyiduser()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            
                var user = await _userServies.getuserByEmail(userEmail);

                var councils = await _councilMembersServies.GetAllNextCouncilsbyidmember(user.Id);
            return Ok(councils);
        }


        [Authorize]
        [HttpGet(template: "Profile")]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var user = await _userServies.getuserByEmail(userEmail);

            
            return Ok(user);
        }

    }
}
