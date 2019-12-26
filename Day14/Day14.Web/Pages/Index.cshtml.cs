using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Day14.Web.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Day14.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IOptions<SignalRConfiguration> _signalRConfig;

        public string SignalRBaseApiUrl { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IOptions<SignalRConfiguration> signalRConfig)
        {
            _logger = logger;
            _signalRConfig = signalRConfig;
        }

        public void OnGet()
        {
            SignalRBaseApiUrl = _signalRConfig.Value.BaseApiUrl;
        }
    }
}
