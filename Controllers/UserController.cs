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

                //var password = Guid.NewGuid().ToString("N").Substring(0, 8);
                adduser.img = "defaultimage.png";
                //adduser.PasswordHash = password;
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
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
          
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
                           // var password = Guid.NewGuid().ToString("N").Substring(0, 8);

                            user.img = "defaultimage.png";
                            //user.PasswordHash = password;
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
        [Authorize]
        [Authorize(Policy = "RequireAddMembersPermission")]
        [HttpGet(template: "GetAllUsers")]
        public async Task<IActionResult> getAlluser()
        {
            var users = await _userServies.getAllUser();
            return Ok(users);
        }
        //////Get user By name
         [Authorize]
       
        [HttpGet(template: "GetUserByname")]
        public async Task<IActionResult> getuserByname(string fullname)
        {
            var user = await _userServies.getuserByFullName(fullname);
            return Ok(user);
        }
        /// ////////////get user by email
        [Authorize]
        [HttpGet(template: "GetUserByEmail")]
        public async Task<IActionResult> getuserByEmail(string email)
        {
            var user = await _userServies.getuserByEmail(email);
            return Ok(user);
        }

        //////all user by Name
        [Authorize]

        [HttpGet(template: "GetAllUserByname")]
        public async Task<IActionResult> getAlluserByname(string fullname)
        {
            var users = await _userServies.getAllUserByname(fullname);
            return Ok(users);
        }

        [Authorize]
        //update user
        [HttpPut(template: "UpdateUser")]
        public async Task<IActionResult> updateUser(string id ,[FromForm] updateuserDTO user )
        {

            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userEmail == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            var checkuser = await _userServies.getuserByEmail(userEmail);
            if (checkuser == null)
            {
                return BadRequest("This user not found !");
            }
            var per = await _permissionsServies.CheckPermissionAsync(checkuser.Id, "UpdateUser");
            if (!per || checkuser.Id != id)
            {
                return Unauthorized("User is not authenticated.");
            }


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





        [Authorize]

        [HttpGet(template: "GetAllUserByIdDepartment")]
        public async Task<IActionResult> getAlluserByIdDepartment(int id)
        {
            var users = await _userServies.getAllUserByIdDepartment(id);

            return Ok(users);
        }

        [Authorize]
        [HttpGet(template: "GetAllUserByIdCollage")]
        public async Task<IActionResult> getAlluserByIdCollage(int id)
        {
            var users = await _userServies.getAllUserByIdCollage(id);

            return Ok(users);
        }

        [Authorize]
        [Authorize(Roles = "BasicUser,Secretary,ChairmanOfTheBoard")]
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
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto dto)
        {
            if (ModelState.IsValid)
            {
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
                    return BadRequest("Invalid Credentials ");
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

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] UserForgetPasswordRequestDto dto)
        {
            if (ModelState.IsValid)
            {

                var existing_user = await _usermanager.FindByEmailAsync(dto.Email);

                if (existing_user == null)
                {
                    return BadRequest("The User Not Exist");
                }
                Random rand = new Random();
                int otp = rand.Next(100000, 999999);
      
                var subject = "Council Management System Password Reset Not-replay";
                var body = $"Dear User,\n\nYou have requested to reset your password for the Council Management System. Please use this OTP to reset your password. Your (OTP) is: {otp}.";

                await _mailingService.SendEmailAsync(dto.Email, subject, body);


                existing_user.OTP = otp;
                await _usermanager.UpdateAsync(existing_user);

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
        [Authorize]
        //[AllowAnonymous]
        [HttpPost("ConfirmOTP")]
        public async Task<IActionResult> ConfirmOTP([FromBody] ConfirmOTPDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"])),
                ClockSkew = TimeSpan.Zero // Remove delay of token when expire
            };

            try
            {

                var principal = tokenHandler.ValidateToken(dto.Token, validationParameters, out validatedToken);
                var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;


                var existingUser = await _usermanager.FindByEmailAsync(userEmail);
                if (existingUser == null)
                {
                    return BadRequest("The user does not exist.");
                }


                if (existingUser.OTP != dto.OTP)
                {
                    return BadRequest("Invalid OTP.");
                }


                existingUser.OTP = null;
                var newToken = GeneratJwtToken(existingUser);
                await _usermanager.UpdateAsync(existingUser);

                return Ok(new AuthenticationResault()
                {
                    Token = newToken,
                    Result = true,
                    Errors = new List<string>()
            {
                "OTP successfully Confirmed."
            },
                });
            }
            catch (SecurityTokenException)
            {
                return BadRequest("Invalid token.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        //[AllowAnonymous]
        [Authorize]
        [HttpPost("AddNewPassword")]
        public async Task<IActionResult> AddNewPassword([FromBody] AddNewPasswordWithTokenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload.");
            }

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                return BadRequest("The new password and confirmation password do not match.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"])),
                ClockSkew = TimeSpan.Zero // Remove delay of token when expire
            };

            try
            {

                var principal = tokenHandler.ValidateToken(dto.Token, validationParameters, out validatedToken);
                var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;


                var existingUser = await _usermanager.FindByEmailAsync(userEmail);
                if (existingUser == null)
                {
                    return BadRequest("The user does not exist.");
                }

                var resetToken = await _usermanager.GeneratePasswordResetTokenAsync(existingUser);

                var result = await _usermanager.ResetPasswordAsync(existingUser, resetToken, dto.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to reset the password.");
                }

                var newToken = GeneratJwtToken(existingUser);

                return Ok(new AuthenticationResault()
                {
                    Token = newToken,
                    Result = true,
                    Errors = new List<string>()
                    {
                        "The new password added successfully."
                    },
                });
            }
            catch (SecurityTokenException)
            {
                return BadRequest("Invalid token.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(LogoutDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload.");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"])),
                ClockSkew = TimeSpan.Zero // Remove delay of token when expire
            };

            try
            {

                var principal = tokenHandler.ValidateToken(dto.Token, validationParameters, out validatedToken);
                var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;


                var existingUser = await _usermanager.FindByEmailAsync(userEmail);
                if (existingUser == null)
                {
                    return BadRequest("The user does not exist.");
                }

                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                 return Ok(new AuthenticationResault()
                  {
                    Result = true,
                    Errors = new List<string>()
                    {
                          "Logout successful."
                    }
                  });
            }
            catch (SecurityTokenException)
            {
                return BadRequest("Invalid token.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }



        [Authorize]
        [Authorize(Policy = "RequireDeactiveUserPermission")]
        // [Authorize(Roles = "SuperAdmin,SubAdmin")]
        [HttpPut("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser([FromHeader] DeactivateUserRequestDto dto)
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
        [Authorize]
        [Authorize(Policy = "RequireDeactiveUserPermission")]
        [HttpPut("ActivateUserbyAdmin")]
        public async Task<IActionResult> ActivateUserbyAdmin([FromBody] ActivateUserByAdminRequestDto dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return NotFound("Invalid Email");
            }

            user.IsVerified = true;
            
            await _usermanager.UpdateAsync(user);
            
            return Ok(new AuthenticationResault()
            {
                
                Errors = new List<string>()
                {
                        "The User Activated successful"
                 },
                Result = true,

            });
        }

        [Authorize]
        [Authorize(Policy = "RequireUpdatepermission")]
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
            var user1 = await _userServies.getuserObjectByid(user.Id);


            return Ok(user1 );
        }

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


                Expires = DateTime.Now.AddHours(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = JwtTokenHandler.CreateToken(TokenDescriptor);
            var jwtToken = JwtTokenHandler.WriteToken(token);
            return jwtToken;
        }



    }
}
