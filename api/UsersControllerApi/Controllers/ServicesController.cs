using BaseProjectApi.Models;
using BaseProjectApi.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace BaseProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ServicesController : Controller
    {
        private readonly IUserServices _ius;

        private ServiceModel _response;
        public ServicesController(IUserServices ius)
        {
            _response = new ServiceModel();
            _ius = ius;
        }

        [HttpGet("Ping")]
        public async Task<IActionResult> PingMe()
        {
            try
            {
                _response.Code = 200;
                _response.Status = true;
                _response.Message = "PingMe Answered";
                _response.Payload = "";

            }
            catch (Exception e)
            {
                _response.Code = 501;
                _response.Status = true;
                _response.Message = "PingMe exception";
                _response.Payload = "";

            }

            return Ok(_response);
        }

        [HttpPost("UsersControl")]
        public async Task<IActionResult> UsersControl(RequestModel uiReq)
        {
            var Action = uiReq.Action;

            switch (Action)
            {
                case "RegisterUser":
                    _response = await _ius.RegisterUser(uiReq);
                    break;
                case "UserLogin":
                    _response = await _ius.UserLogin(uiReq);
                    break;
                case "UserLogout":
                    _response = await _ius.UserLogout();
                    break;
                case "GetSingleUser":
                    _response = await _ius.GetSingleUser(uiReq);
                    break;
                case "GetAllUsers":
                    _response = await _ius.GetAllUsers(uiReq);
                    break;
                case "UpdateUser":
                    _response = await _ius.UpdateUser(uiReq);
                    break;
                case "DeleteSingleUser":
                    _response = await _ius.DeleteSingleUser(uiReq);
                    break;
                case "DeleteAllUsers":
                    _response = await _ius.DeleteAllUsers(uiReq);
                    break;
                case "DecryptUserToken":
                    _response = await _ius.DecryptUserToken(uiReq);
                    break;
                default:
                    break;
            }

            if (!_response.Status)
            {
                Console.WriteLine("");
            }

            return Ok(_response);
        }
    }
}
