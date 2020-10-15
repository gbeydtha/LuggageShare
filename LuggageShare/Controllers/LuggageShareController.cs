using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LuggageShare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LuggageShareController : ControllerBase
    {

        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper; 
        public LuggageShareController(IRepositoryWrapper repositoryWrapper, ILoggerManager logger)
        {
            _repoWrapper = repositoryWrapper;
            _logger = logger; 
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {

            DateTime dr = DateTime.Today;
            string dt = dr.ToString();
            string dt1 = dr.ToLongDateString();
            string dt2 = dr.ToShortDateString();


            _logger.LogDebug(" Debug message from the controller");
            _logger.LogError("Error message from the controller");
            _logger.LogInfo("Error message from the Controller");
            _logger.LogWarn("Warning message from the controller");

            var domesticAccounts = _repoWrapper.Account.FindByCondition( x => x.AccountType.Equals("Domestic"));
            var owners = _repoWrapper.Owner.FindAll(); 


            return new string[] { "value1", "value1" };
        }
    }
}
