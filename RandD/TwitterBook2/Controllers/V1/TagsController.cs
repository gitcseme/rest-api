using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterBook2.Contracts.V1;
using TwitterBook2.Contracts.V1.Requests;
using TwitterBook2.Contracts.V1.Responses;
using TwitterBook2.Domain;
using TwitterBook2.Extensions;
using TwitterBook2.Services;

namespace TwitterBook2.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class TagsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public TagsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all the tags in the system
        /// </summary>
        /// <response code="200">Returns all tags</response>
        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = "TagViewer")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _postService.GetAllTagsAsync();

            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        /// <summary>
        /// Creates a tag
        /// </summary>
        /// <response code="201">Creates a tag</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.TagName, 
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _postService.CreateTagAsync(newTag);
            if (!created)
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "Unable to create tag" }));

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);
            return Created(locationUri, _mapper.Map<TagResponse>(newTag));
        }

    }
}
