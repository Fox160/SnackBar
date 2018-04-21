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
    public class CustomerController  : ApiController
    {
        private readonly InterfaceCustomerService _service;

        public CustomerController(InterfaceCustomerService service)
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
        public void AddElement(BoundCustomerModel model)
        {
            _service.addElement(model);
        }

        [HttpPost]
        public void UpdElement(BoundCustomerModel model)
        {
            _service.addElement(model);
        }

        [HttpPost]
        public void DelElement(BoundCustomerModel model)
        {
            _service.getElement(model.ID);
        }
    }
}
