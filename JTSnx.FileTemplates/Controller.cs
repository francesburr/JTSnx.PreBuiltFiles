using JTSnx.BLL.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace $controllerNamespace$
{
    [Route("api/[controller]")]
    [ApiController]
    public class $basename$Controller : Controller
    {
        private readonly $facadeName$ _facade = new $facadeName$();
        private readonly string _connectionString;
        private readonly JsonSerializerSettings _settings;
        private readonly ILogger<$basename$Controller> _logger;

        public $basename$Controller(IOptions<JTSOptions> options, ILogger<$basename$Controller> logger)
        {
            _connectionString = options.Value.ConnString;
            _settings = options.Value.JsonSerializerSettings;
            _logger = logger;
        }



            /*********************EXAMPLE***PLEASE REMOVE************************************
             ***** This is just to show you how to set things up.  *****/

            /// <summary>
            /// Calls stored proc StoredProcName 
            /// </summary>
            /// <remarks>
            /// <para>Sends: XYZ as serialized object</para>
            /// <para>Returns: XYZ as JsonResult </para>
            /// </remarks>
            /// <param name="abcParam">Description of param</param>
            /// <returns>XYZ as JsonResult</returns>*/

            $apiComment$
            /*[HttpGet("**Enter the name of the call here**")]
             public JsonResult Example(object inputObj)
             {
	             var dsObj = JsonConverter.DeserializeObject<ObjectName>(inputObj.ToString());
                 var resultObj = _facade.interface.call(_connectionString, dsObj);
	             return Json(resultObj, _settings);
             }
            ****************End Example*******************************************************/

    }


}