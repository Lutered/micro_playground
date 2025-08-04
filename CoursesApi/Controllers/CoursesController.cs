using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        [HttpGet]
        public async Task GetCourses()
        {

        }
    }
}
