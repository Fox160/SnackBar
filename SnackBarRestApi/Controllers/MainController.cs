using System;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SnackBarRestApi.Services;

namespace SnackBarRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly InterfaceMainService _service;

        public MainController(InterfaceMainService service)
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

        [HttpPost]
        public void CreateBooking(BoundBookingModel model)
        {
            _service.createOrder(model);
        }

        [HttpPost]
        public void TakeBookingInWork(BoundBookingModel model)
        {
            _service.takeOrderInWork(model);
        }

        [HttpPost]
        public void FinishBooking(BoundBookingModel model)
        {
            _service.finishOrder(model.ID);
        }

        [HttpPost]
        public void PayBooking(BoundBookingModel model)
        {
            _service.payOrder(model.ID);
        }

        [HttpPost]
        public void PutElementOnReserve(BoundResElementModel model)
        {
            _service.putComponentOnReserve(model);
        }

        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            ReflectionService service = new ReflectionService();
            var list = service.GetInfoByAssembly();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}
