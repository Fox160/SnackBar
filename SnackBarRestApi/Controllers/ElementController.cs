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
    public class ElementController : ApiController
    {
        private readonly InterfaceComponentService _service;

        public ElementController(InterfaceComponentService service)
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
        public void AddElement(BoundElementModel model)
        {
            _service.addElement(model);
        }

        [HttpPost]
        public void UpdElement(BoundElementModel model)
        {
            _service.addElement(model);
        }

        [HttpPost]
        public void DelElement(BoundElementModel model)
        {
            _service.getElement(model.ID);
        }
    }
}
