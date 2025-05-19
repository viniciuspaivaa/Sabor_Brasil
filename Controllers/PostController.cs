using Microsoft.AspNetCore.Mvc;
using Sabor_Brasil.Services;

namespace Sabor_Brasil.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{usuarioId}")]
        public IActionResult GetPosts(int usuarioId)
        {
            var posts = _postService.ObterPostsPorUsuario(usuarioId);
            return Ok(posts);
        }
    }
}
