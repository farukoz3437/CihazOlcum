using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.HelperMethods;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    public class ArrayMeasurementController : Controller
    {
        private IArrayMeasurementService _arrayMeasurementService;
        private IArrayDesignService _arrayDesignService;
        private IAspNetUserService _userService;
        private IMapper _mapper;
        public ArrayMeasurementController(IArrayMeasurementService arrayMeasurementService, IAspNetUserService userService, IArrayDesignService arrayDesignService, IMapper mapper)
        {
            _arrayMeasurementService = arrayMeasurementService;
            _arrayDesignService = arrayDesignService;
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<IActionResult> ArrayMeasurements(string arrayDesignIdKod)
        {
            var listArrayMeasurementAsync = await _arrayMeasurementService.GetList();
            List<ArrayMeasurement> listArrayMeasurement = listArrayMeasurementAsync.Data.Where(x => x.ArrayDesignIdKod == arrayDesignIdKod).ToList();

            ArrayMeasurementListModel arrayMeasurementListModel = new ArrayMeasurementListModel();
            arrayMeasurementListModel.ArrayDesignIdKod = arrayDesignIdKod;
            arrayMeasurementListModel.arrayMeasurements = listArrayMeasurement;
            return View(arrayMeasurementListModel);
        }

        public async Task<IActionResult> ArrayMeasurementEdit(string idKod, string userId)
        {

            var resultMeasuremnt = await _arrayMeasurementService.GetByIdKod(idKod);

            ArrayMeasurement measurement = resultMeasuremnt.Data;

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

                ArrayMeasurementModel measurementModel = _mapper.Map<ArrayMeasurement, ArrayMeasurementModel>(measurement);
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
        public async Task<IActionResult> ArrayMeasurementEdit(ArrayMeasurementModel measurementModel, List<IFormFile> Files, IFormFile MeasurementImg)
        {
            ArrayMeasurement measurement = _mapper.Map<ArrayMeasurementModel, ArrayMeasurement>(measurementModel);
            var resultMeasuremnt = await _arrayMeasurementService.GetByIdKod(measurementModel.IdKod);
            List<string> fileNames = new List<string>();
            if (MeasurementImg != null)
            {
                measurement.MeasurementImg = await ImageUpload.Upload(MeasurementImg, "wwwroot\\assets\\media\\arraymeasurements\\photos");
            }
            else
            {
                measurement.MeasurementImg = resultMeasuremnt.Data.MeasurementImg;
            }


            measurement.ArrayMeasurementId = resultMeasuremnt.Data.ArrayMeasurementId;
            measurement.MeasurementDate = resultMeasuremnt.Data.MeasurementDate;



            if (Files != null && Files.Count > 0)
            {
                foreach (IFormFile file in Files)
                {
                    fileNames.Add(measurement.FileN = await FileUpload.Upload(file, "wwwroot\\assets\\media\\arraymeasurements"));
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

            IResult result = await _arrayMeasurementService.Update(measurement);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return Redirect("/ArrayMeasurement/ArrayMeasurements?arrayDesignIdKod=" + resultMeasuremnt.Data.ArrayDesignIdKod);
        }

        public async Task<IActionResult> ArrayMeasurementDetail(string idKod)
        {
            var resultMeasuremnt = await _arrayMeasurementService.GetByIdKod(idKod);
            ArrayMeasurement arrayMeasurement = resultMeasuremnt.Data;

            var user = await _userService.GetById(arrayMeasurement.UserId);

            ArrayMeasurementModel measurementModel = _mapper.Map<ArrayMeasurement, ArrayMeasurementModel>(arrayMeasurement);
            measurementModel.UserFullName = user.Data.FirstName + " " + user.Data.LastName;

            return View(measurementModel);
        }

        public async Task<ActionResult> DownloadZip(string idKod)
        {
            try
            {
                var resultMeasuremnt = await _arrayMeasurementService.GetByIdKod(idKod);
                List<string> fileNames = new List<string>();
                fileNames.Add(resultMeasuremnt.Data.FileE);
                fileNames.Add(resultMeasuremnt.Data.FileN);
                fileNames.Add(resultMeasuremnt.Data.FileZ);


                string zipName = resultMeasuremnt.Data.MeasurementDate?.ToString("ddMMyyyHHmm") + "_" + resultMeasuremnt.Data.PointName;
                if (resultMeasuremnt.Data.FileE == null || resultMeasuremnt.Data.FileN == null || resultMeasuremnt.Data.FileZ == null)
                {
                    TempData["message"] = "Dosya bulunamadı.|warning";
                    return RedirectToAction("Measurements", "Measurement", null);
                }

                bool zip = HelperMethods.CreateZipFile.CreateZip(fileNames, zipName, "wwwroot\\assets\\media\\arraymeasurements\\");

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

        public ActionResult Location(string arrayIdKod)
        {
            List<ArrayMeasurement> measurements = _arrayMeasurementService.GetList().Result.Data.Where(x => x.ArrayDesignIdKod == arrayIdKod).ToList();

            ArrayDesign arrayDesign = _arrayDesignService.GetByIdKod(arrayIdKod).Result.Data;
            CreateArraydesignModel arrayDesignModel = _mapper.Map<ArrayDesign, CreateArraydesignModel>(arrayDesign);

            if (arrayDesign.ArrayFormat == 0)
            {
                string designs = "[";
                string marker = "[";
                foreach (var measurement in measurements)
                {
                    marker += "{";
                    marker += string.Format("'title': '{0}',", measurement.PointName);
                    marker += string.Format("'lat': '{0}',", Convert.ToDouble(measurement.GpsXField));
                    marker += string.Format("'lng': '{0}',", Convert.ToDouble(measurement.GpsYField));
                    marker += string.Format("'description': '{0}',", measurement.PointName);
                    marker += string.Format("'idkod': '{0}'", measurement.IdKod);
                    marker += "},";
                }
                marker += "]";
                designs += marker;
                designs += "]";

                ViewBag.Markers = designs;
            }
            else
            {


                var designNumbers = measurements.GroupBy(p => p.DesignNumber).Select(g => g.First()).ToList();
                string designs = "[";
                foreach (var item in designNumbers)
                {
                    string marker = "[";
                    var measurementList = measurements.Where(x => x.DesignNumber == item.DesignNumber).ToList();
                    foreach (var measurement in measurementList)
                    {
                        marker += "{";
                        marker += string.Format("'title': '{0}',", measurement.PointName);
                        marker += string.Format("'lat': '{0}',", Convert.ToDouble(measurement.GpsXField));
                        marker += string.Format("'lng': '{0}',", Convert.ToDouble(measurement.GpsYField));
                        marker += string.Format("'description': '{0}',", measurement.PointName);
                        marker += string.Format("'idkod': '{0}'", measurement.IdKod);
                        marker += "},";
                    }
                    if (item.DesignNumber == 1)
                    {

                        marker += "{";
                        marker += string.Format("'title': '{0}',", measurementList[1].PointName);
                        marker += string.Format("'lat': '{0}',", Convert.ToDouble(measurementList[1].GpsXField));
                        marker += string.Format("'lng': '{0}',", Convert.ToDouble(measurementList[1].GpsYField));
                        marker += string.Format("'description': '{0}',", measurementList[1].PointName);
                        marker += string.Format("'idkod': '{0}'", measurementList[1].IdKod);
                        marker += "}";
                    }
                    else
                    {
                        marker += "{";
                        marker += string.Format("'title': '{0}',", measurementList[0].PointName);
                        marker += string.Format("'lat': '{0}',", Convert.ToDouble(measurementList[0].GpsXField));
                        marker += string.Format("'lng': '{0}',", Convert.ToDouble(measurementList[0].GpsYField));
                        marker += string.Format("'description': '{0}',", measurementList[0].PointName);
                        marker += string.Format("'idkod': '{0}'", measurementList[0].IdKod);
                        marker += "}";
                    }


                    marker += "],";

                    designs += marker;
                }
                designs += "]";

                ViewBag.Markers = designs;
            }
            return View(arrayDesignModel);
        }

       
    }
}
