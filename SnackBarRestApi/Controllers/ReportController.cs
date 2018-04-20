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
    public class ReportController : ApiController
    {
        private readonly InterfaceReportService _service;

        public ReportController(InterfaceReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetReservesLoad()
        {
            var list = _service.GetReservesLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetCustomerBookings(BoundReportModel model)
        {
            var list = _service.GetCustomerBookings(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveOutputPrice(BoundReportModel model)
        {
            _service.SaveOutputPrice(model);
        }

        [HttpPost]
        public void SaveReservesLoad(BoundReportModel model)
        {
            _service.SaveReservesLoad(model);
        }

        [HttpPost]
        public void SaveCustomerBookings(BoundReportModel model)
        {
            _service.SaveCustomerBookings(model);
        }
    }
}
