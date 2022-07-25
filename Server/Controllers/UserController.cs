using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data;
using Strona_v2.Server.Data.SqlData;
using Strona_v2.Server.EmailC;
using Strona_v2.Server.Filtres;
using Strona_v2.Server.TokenAuthentication;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.Controllers
{

    // For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILoginUser _LogInUser;
        private readonly ITokenManager _tokenManager;
        private readonly IProfileUser _ProfileUser;
        private readonly IEditProfileUser _EditProfileUser;
        private readonly IHashids _hashids;
        private readonly IEmailSend _emailSend;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailVerification _emailVerification;
        public UserController(ILoginUser LogInUser,
            ITokenManager TokenManager,
            IProfileUser ProfileUser,
            IEditProfileUser EditProfileUser,
            IHashids hashids,
            IEmailSend emailSend,
            ILogger<UserController> logger,
            IEmailVerification emailVerification)
        {
            _LogInUser = LogInUser;
            _tokenManager = TokenManager;
            _ProfileUser = ProfileUser;
            _EditProfileUser = EditProfileUser;
            _hashids = hashids;
            _emailSend = emailSend;
            _logger = logger;
            _emailVerification = emailVerification;
        }

        // GET api/<UserController>/5
        //Logowanie użytkownika i zapisywanie jego tokena dla filtru
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                UserLogin user = await _LogInUser.UserLoginAsync(email, password);

                if (_tokenManager.Authenticate(user))
                {
                    user.Password = password;
                    cToken token = await _tokenManager.NewToken(user);
                    user.Token = token.Token;
                    user.ExpiryDate = token.ExpiryDate;

                    user.SecondId = _hashids.Encode(user.Id, 11);
                    // user.Id = -1;

                    return Ok(user);
                    //return Ok(new { Token = _tokenManager.NewToken(user) });
                }
                ModelState.AddModelError("Unauthorized", "You are not unautorized.");
                return Unauthorized(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Pobieranie danych do obejrzenia profilu
        [HttpGet]
        [Route("profileID")]
        public async Task<IActionResult> ProfileDataId(string ID)
        {
            UserPublic userFull;
            try
            {
                int id = -1;
                if (!string.IsNullOrEmpty(ID))
                {
                    var n = _hashids.Decode(ID);
                    id = n[0];
                }

                //pobieranie po id
                if (id >= 0)
                {
                    userFull = await _ProfileUser.UserFullDataIntId(id);
                    if (userFull == null)
                    {
                        return NotFound();
                    }
                    userFull.Id = _hashids.Encode(int.Parse(userFull.Id), 11);
                    return Ok(userFull);
                }

                //bledne dane/ nie ma usera
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Pobieranie danych do obejrzenia profilu
        [HttpGet]
        [Route("profileName")]
        public async Task<IActionResult> ProfileDataName(string userName)
        {
            UserPublic userFull;
            try
            {
                //pobieranie po nick
                if (userName != null)
                {
                    userFull = await _ProfileUser.UserFullDataStringName(userName);
                    if (userFull == null)
                    {
                        return NotFound();
                    }
                    userFull.Id = _hashids.Encode(int.Parse(userFull.Id), 11);

                    return Ok(userFull);
                }
                //bledne dane/ nie ma usera
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        //modyfikacja profilu
        [HttpPatch]
        [Route("profile/patch")]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> PatchProfil([FromBody] UserEditProfile userEditProfile, string email, string password)
        {
            if (userEditProfile == null)
            {
                return NoContent();
            }
            try
            {
                EnableProfil enableProfil = new();
                enableProfil.ListEmail = await _EditProfileUser.GetAllEmail();

                if (enableProfil.EnableEmail(userEditProfile.Email))
                {
                    return BadRequest(EnableProfilEnum.Email);
                }


                UserLogin loginUser = new();
                loginUser.Email = email;
                loginUser.Password = password;

                if (!string.IsNullOrEmpty(userEditProfile.Email))
                {
                    await _EditProfileUser.EditEmail(loginUser, userEditProfile);
                }
                if (!string.IsNullOrEmpty(userEditProfile.Name))
                {
                    await _EditProfileUser.EditName(loginUser, userEditProfile);
                }
                if (!string.IsNullOrEmpty(userEditProfile.AvatarNameString))
                {
                    await _EditProfileUser.EditAvatarNameString(loginUser, userEditProfile);
                }
                if (!string.IsNullOrEmpty(userEditProfile.Password))
                {
                    await _EditProfileUser.EditPassword(loginUser, userEditProfile);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("lastonline")]
        [TokenAuthenticationFilter]
        //sprawdzanie kiedy user był ostatnio online i czy token nie był modyfikowany
        public async Task<IActionResult> UpdateLastOnline()
        {
            string token = HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;
            if (token != null)
            {
                DateTimeOffset LastOnline = DateTimeOffset.Now;

                bool ItIsCorrect = await _EditProfileUser.UpdatelastOnline(token, LastOnline);
                if (ItIsCorrect)
                    return Ok();
                else
                    return BadRequest();
            }
            return NotFound();
        }

        [HttpGet]
        [Route("emails")]
        //sprawdzenie czy Email się powtarza
        public async Task<IActionResult> IndividualEmail(string email)
        {
            IList<string> emails = await _LogInUser.IndividualEmail();

            foreach (string item in emails)
            {
                if (email == item)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }


        //POST api/<UserController>
        //Api od rejestracji
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterClient client)
        {
            try
            {
                if (client != null)
                {
                    UserRegisterServer server = new();
                    server = server.CastFromClient(client);
                    server.DataCreat = DateTimeOffset.Now;

                    await _LogInUser.Register(server);

                    UserLogin login = new();
                    login = login.CastFromUserRegisterServer(server);

                    var EmailToken = _emailVerification.NewToken();
                    EmailToken += login.Id;
                    EmailToken = _hashids.Encode(int.Parse(EmailToken));
                    await _emailSend.SendTemplateRegister(login.Email, "Rejestracja", EmailBody.RegisterTemplate(login.Email, EmailToken));

                    return Ok(login);
                }
                return NoContent(); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string EmailToken)
        {
            var intToken = _hashids.Decode(EmailToken);

            int id = int.Parse(intToken[0].ToString().Substring(8, 1));
            string token = intToken[0].ToString().Substring(0, 8);

            if (_emailVerification.ConfirmToken(token))
            {
                await _EditProfileUser.EmailVerification(id);
                return Ok(true);
            }

            return BadRequest(false);
        }

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }


}
