using ASPNETCoreForBeginners.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASPNETCoreForBeginners.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        // private readonly AttachmentOptions _attachmentOptions;
        // private readonly IOptions<AttachmentOptions> _attachmentOptions;
        //private readonly IOptionsSnapshot<AttachmentOptions> _attachmentOptions;
        private readonly IOptionsMonitor<AttachmentOptions> _attachmentOptions;

        public ConfigController(IConfiguration configuration,
            IOptionsMonitor<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
            var Value = _attachmentOptions.CurrentValue;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetConfig()
        {
            Thread.Sleep(10000);
            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                ConnectionStrings = _configuration["ConnectionStrings:DefaultConnection"],
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"],
                Singningkey = _configuration["SingningKey"],
                AttachmentOptions = _attachmentOptions.CurrentValue,

            };
            return Ok(config);
        }
    }
}
