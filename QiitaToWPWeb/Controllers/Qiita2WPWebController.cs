using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestProject.QiitaToWP;

namespace TestProject.QiitaToWPWeb.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Qiita2WPWebController : ControllerBase
    {
        private readonly ILogger<Qiita2WPWebController> _logger;

        public Qiita2WPWebController(ILogger<Qiita2WPWebController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var q2wp = new Qiita2WP();

            // Qiita2WPプロジェクトの方の処理を使いまわす
            await q2wp.Qiita2WPArticle();

            return Ok();
        }
    }
}
