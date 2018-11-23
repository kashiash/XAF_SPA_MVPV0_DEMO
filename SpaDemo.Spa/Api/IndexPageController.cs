using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace SpaDemo.Spa.Controllers {
    public class IndexPageController : Controller {
        IHostingEnvironment env;
        public IndexPageController(IHostingEnvironment env) {
            this.env = env;
        }
        public IActionResult Index() {
            return new PhysicalFileResult(Path.Combine(env.WebRootPath, "index.html"), "text/html");
        }
    }
}
