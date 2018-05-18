using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SnackBarRestApi.Controllers
{
    public class MessageInfoController : ApiController
    {
        private readonly InterfaceMessageInfoService _service;

        public MessageInfoController(InterfaceMessageInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(BoundMessageInfoModel model)
        {
            _service.AddElement(model);
        }
    }
}
