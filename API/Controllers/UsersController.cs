using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUploadPhotoService _photoService;

        public UsersController(IUserRepository userRepository,
         IMapper mapper, 
         IUploadPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<MemberDto>> GetUser(int id)
        // {
        //     var user = await _userRepository.GetUserByIdAsync(id);
        //     var member = _mapper.Map<MemberDto>(user);
        //     return Ok(member);
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllMembersDtoAsync();
            var members = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> AddUser(AppUser appUser)
        {
            await _userRepository.AddUser(appUser);
            return Created("", appUser);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            var users = await _userRepository.GetMemberDtoByUserName(username);
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto member)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetUserByUserName(username);

            _mapper.Map(member, user);

            _userRepository.Update(user);

            var resutl = await _userRepository.SaveAllAsync();

            if(!resutl){
                return BadRequest("Something went wrong when updating user");
            }

            return NoContent();
        }

        [HttpPost("Photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm]IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetUserByUserName(username);

            var photoUploadResult = await _photoService.UploadPhoto(file);

            if (photoUploadResult.Error != null)
            {
                return BadRequest($"Error uploadind the photo {photoUploadResult.Error.Message}");
            }

            user.Photos.Add(new Photo
            {
                Url = photoUploadResult.SecureUrl.AbsoluteUri,
                PublicId = photoUploadResult.PublicId,
                IsMain = HasUserOtherPhotos(user)
            });

            _userRepository.Update(user);
            await _userRepository.SaveAllAsync();

            var useDto = _mapper.Map<PhotoDto>(user.Photos.Last<Photo>());
            return CreatedAtRoute("GetUser", new { username = username }, useDto);
        }

        [HttpPut("photos/{photoId}")]
        public async Task<ActionResult> UpdatePhoto(int photoId)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userRepository.GetUserByUserName(username);
            var photo = user.Photos.First(x => x.PhotoId == photoId);
            if(photo.IsMain){
                return BadRequest("Photo is already main photo");
            }

            var currentMainPhoto = user.Photos.First(x => x.IsMain);
            currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()){
                return NoContent();
            }

            return BadRequest("Something went wrong while updating user photo");

        }

        private static bool HasUserOtherPhotos(AppUser user)
        {
            return (user.Photos.Count == 0) ? true : false;
        }

    }
}