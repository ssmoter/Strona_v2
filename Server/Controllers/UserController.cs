using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data;
using Strona_v2.Server.Data.SqlData;
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


        public UserController(ILoginUser LogInUser,
            ITokenManager TokenManager,
            IProfileUser ProfileUser,
            IEditProfileUser EditProfileUser,
            IHashids hashids)
        {
            _LogInUser = LogInUser;
            _tokenManager = TokenManager;
            _ProfileUser = ProfileUser;
            _EditProfileUser = EditProfileUser;
            _hashids = hashids;
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

                    user.SecondId=_hashids.Encode(user.Id,11);
                    user.Id = -1;
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
        [Route("profile")]
        public async Task<IActionResult> ProfileData(/*int? id,*/ string? userName)
        {
            UserPublic userFull;
            try
            {
                //pobieranie po id 
                //if (id >= 0)
                //{
                //    userFull = await _ProfileUser.UserFullDataIntId((int)id);

                //    return Ok(userFull);
                //}
                //pobieranie po nick
                if (userName != null)
                {
                    userFull = await _ProfileUser.UserFullDataStringName(userName);

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



        // POST api/<UserController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

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
