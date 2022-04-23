using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using IMandCRM.UI.HelperMethods;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize]
    public class MeasurementController : Controller
    {
        private IMeasurementService _measurementService;
        private IAspNetUserService _userService;
        private IMapper _mapper;
        public MeasurementController(IMeasurementService measurementService, IAspNetUserService userService, IMapper mapper)
        {
            _measurementService = measurementService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Measurements()
        {

            var listMeasurementAsync = await _measurementService.GetListMeasurementDto();
            List<MeasurementDto> listMeasurement = listMeasurementAsync.Data.OrderByDescending(x => x.MeasurementDate).ToList();
            var measurementListModel = _mapper.Map<List<MeasurementDto>, List<MeasurementModel>>(listMeasurement);

            return View(measurementListModel);
        }

        [Authorize(Roles = "Admin, Manager, Operator")]
        public IActionResult MeasurementAdd()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MeasurementAdd(MeasurementModel measurementModel, List<IFormFile> Files, IFormFile MeasurementImg)
        {
            Measurement measurement = _mapper.Map<MeasurementModel, Measurement>(measurementModel);
            List<string> fileNames = new List<string>();

            if (MeasurementImg != null)
            {
                measurement.MeasurementImg = await ImageUpload.Upload(MeasurementImg, "wwwroot\\assets\\media\\measurements\\photos");
            }
            else
            {
                measurement.MeasurementImg = Constants.Constants.DefaultMeasurementPhoto;
            }

            if (Files != null && Files.Count > 0)
            {
                foreach (IFormFile file in Files)
                {
                    fileNames.Add(measurement.FileN = await FileUpload.Upload(file, "wwwroot\\assets\\media\\measurements"));
                }
            }
            foreach (string item in fileNames)
            {
                if (item.Contains("E"))
                {
                    measurement.FileE = item;
                }
                if (item.Contains("N"))
                {
                    measurement.FileN = item;
                }
                if (item.Contains("Z"))
                {
                    measurement.FileZ = item;
                }
            }
            IResult result = await _measurementService.Add(measurement);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("Measurements", "Measurement", null);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<JsonResult> MeasurementsDelete(string[] DeleteMeasurements)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteMeasurements)
                {
                    var measurement = await _measurementService.GetByIdKod(idKod);
                    if (measurement.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _measurementService.Delete(measurement.Data);
                }


                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Kayıtlar başarıyla silindi.";
                alertMessage.MessageType = "success";

                return Json(alertMessage);
            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Kayıt silinirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);

            }

        }

        public async Task<IActionResult> MeasurementEdit(string idKod, string userId)
        {
            var resultMeasuremnt = await _measurementService.GetByIdKod(idKod);

            Measurement measurement = resultMeasuremnt.Data;

            List<string> roles = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value).ToList();
            bool isRole = false;
            foreach (var role in roles)
            {
                if (role == "Manager" || role == "Admin")
                {
                    isRole = true;
                }
            }

            if (measurement.UserId == userId || isRole)
            {
                var user = await _userService.GetById(measurement.UserId);

                MeasurementModel measurementModel = _mapper.Map<Measurement, MeasurementModel>(measurement);
                measurementModel.UserFullName = user.Data.FirstName + " " + user.Data.LastName;

                return View(measurementModel);
            }
            else
            {
                return RedirectToAction("accessdenied", "Account", null);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MeasurementEdit(MeasurementModel measurementModel, List<IFormFile> Files, IFormFile MeasurementImg)
        {
            Measurement measurement = _mapper.Map<MeasurementModel, Measurement>(measurementModel);
            var resultMeasuremnt = await _measurementService.GetByIdKod(measurementModel.IdKod);
            List<string> fileNames = new List<string>();
            if (MeasurementImg != null)
            {
                measurement.MeasurementImg = await ImageUpload.Upload(MeasurementImg, "wwwroot\\assets\\media\\measurements\\photos");
            }
            else
            {
                measurement.MeasurementImg = resultMeasuremnt.Data.MeasurementImg;
            }


            measurement.MeasurementId = resultMeasuremnt.Data.MeasurementId;
            measurement.MeasurementDate = resultMeasuremnt.Data.MeasurementDate;



            if (Files != null && Files.Count > 0)
            {
                foreach (IFormFile file in Files)
                {
                    fileNames.Add(measurement.FileN = await FileUpload.Upload(file, "wwwroot\\assets\\media\\measurements"));
                }
            }
            else
            {
                measurement.FileE = resultMeasuremnt.Data.FileE;
                measurement.FileN = resultMeasuremnt.Data.FileN;
                measurement.FileZ = resultMeasuremnt.Data.FileZ;
            }
            foreach (string item in fileNames)
            {
                if (item.Contains("E"))
                {
                    measurement.FileE = item;
                }
                if (item.Contains("N"))
                {
                    measurement.FileN = item;
                }
                if (item.Contains("Z"))
                {
                    measurement.FileZ = item;
                }
            }

            IResult result = await _measurementService.Update(measurement);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("Measurements", "Measurement", null);
        }

        public async Task<IActionResult> MeasurementDetail(string idKod)
        {
            var resultMeasuremnt = await _measurementService.GetByIdKod(idKod);
            Measurement measurement = resultMeasuremnt.Data;

            var user = await _userService.GetById(measurement.UserId);

            MeasurementModel measurementModel = _mapper.Map<Measurement, MeasurementModel>(measurement);
            measurementModel.UserFullName = user.Data.FirstName + " " + user.Data.LastName;

            return View(measurementModel);
        }
        public async Task<JsonResult> MeasurementDelete(string idKod, string userId)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var measurement = await _measurementService.GetByIdKod(idKod);

                List<string> roles = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
                bool isRole = false;
                foreach (var role in roles)
                {
                    if (role == "Manager" || role == "Admin")
                    {
                        isRole = true;
                    }
                }

                if (measurement.Data.UserId == userId || isRole)
                {
                    if (measurement.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _measurementService.Delete(measurement.Data);
                    alertMessage.ResponseStatus = true;
                    alertMessage.MessageText = "Kayıt başarıyla silindi.";
                    alertMessage.MessageType = "success";

                    return Json(alertMessage);
                }
                else
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme yetkiniz bulunmamaktadır.";
                    alertMessage.MessageType = "warning";
                    return Json(alertMessage);
                }

            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Kayıt silinirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);
            }

        }

        public async Task<ActionResult> DownloadZip(string idKod)
        {
            try
            {
                var resultMeasuremnt = await _measurementService.GetByIdKod(idKod);
                List<string> fileNames = new List<string>();
                fileNames.Add(resultMeasuremnt.Data.FileE);
                fileNames.Add(resultMeasuremnt.Data.FileN);
                fileNames.Add(resultMeasuremnt.Data.FileZ);


                string zipName = resultMeasuremnt.Data.MeasurementDate.ToString("ddMMyyyHHmm") + "_" + resultMeasuremnt.Data.PointName;
                if (resultMeasuremnt.Data.FileE == null || resultMeasuremnt.Data.FileN == null || resultMeasuremnt.Data.FileZ == null)
                {
                    TempData["message"] = "Dosya bulunamadı.|warning";
                    return RedirectToAction("Measurements", "Measurement", null);
                }

                bool zip = HelperMethods.CreateZipFile.CreateZip(fileNames, zipName, "wwwroot\\assets\\media\\measurements\\");

                if (!zip)
                {
                    TempData["message"] = "Zip dosyası oluşturulurken hata oluştu.|error";
                    return RedirectToAction("Measurements", "Measurement", null);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\media\\zipfile\\" + zipName + ".zip");

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                if (System.IO.File.Exists("wwwroot\\assets\\media\\zipfile\\" + zipName + ".zip"))
                {
                    System.IO.File.Delete("wwwroot\\assets\\media\\zipfile\\" + zipName + ".zip");
                }
                return File(memory, "application/zip", zipName + ".zip");

            }
            catch (Exception)
            {

                TempData["message"] = "Dosya indirilirken hata oluştu.|error";
                return RedirectToAction("Measurements", "Measurement", null);
            }


        }

        public ActionResult Location()
        {
            List<Measurement> measurements = _measurementService.GetList().Result.Data;

            string markers = "[";
            foreach (var measurement in measurements)
            {
                markers += "{";
                markers += string.Format("'title': '{0}',", measurement.PointName);
                markers += string.Format("'lat': '{0}',", Convert.ToDouble(measurement.Lat));
                markers += string.Format("'lng': '{0}',", Convert.ToDouble(measurement.Lon));
                markers += string.Format("'description': '{0}',", measurement.PointName);
                markers += string.Format("'idkod': '{0}'", measurement.IdKod);
                markers += "},";
            }
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }

    }

}
