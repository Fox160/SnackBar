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
    public class ReserveController : ApiController
    {
        private readonly InterfaceReserveService _service;

        public ReserveController(InterfaceReserveService service)
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
        public void AddElement(BoundReserveModel model)
        {
            _service.addElement(model);
        }

        [HttpPost]
        public void UpdElement(BoundReserveModel model)
        {
            _service.updateElement(model);
        }

        [HttpPost]
        public void DelElement(BoundReserveModel model)
        {
            _service.deleteElement(model.ID);
        }
    }
}
