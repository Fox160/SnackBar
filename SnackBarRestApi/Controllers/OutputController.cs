using System;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SnackBarRestApi.Controllers
{
    public class OutputController : ApiController
    {
        private readonly InterfaceOutputService _service;

        public OutputController(InterfaceOutputService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.getList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.getElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(BoundOutputModel model)
        {
            _service.addElement(model);
        }

        [HttpPost]
        public void UpdElement(BoundOutputModel model)
        {
            _service.updateElement(model);
        }

        [HttpPost]
        public void DelElement(BoundOutputModel model)
        {
            _service.deleteElement(model.ID);
        }
    }
}
