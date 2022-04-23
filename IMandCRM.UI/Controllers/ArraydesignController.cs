using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize]
    public class ArraydesignController : Controller
    {
        IMapper _mapper;
        IArrayDesignService _arrayDesignService;
        IArrayMeasurementService _arrayMeasurementService;
        public ArraydesignController(IArrayDesignService arrayDesignService,IArrayMeasurementService arrayMeasurementService, IMapper mapper)
        {
            _arrayDesignService = arrayDesignService;
            _arrayMeasurementService = arrayMeasurementService;
            _mapper = mapper;
        }
        public IActionResult CreateArrayDesign()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArrayDesign(CreateArraydesignModel createArraydesignModel)
        {
            ArrayDesign arrayDesign = _mapper.Map<CreateArraydesignModel, ArrayDesign>(createArraydesignModel);

            IDataResult<ArrayDesign> result = await _arrayDesignService.Add(arrayDesign);

     
            for (int i = 0; i < createArraydesignModel.CartesianPointName.Length; i++)
            {
                ArrayMeasurement arrayMeasurement = new ArrayMeasurement();
                arrayMeasurement.PointName = createArraydesignModel.CartesianPointName[i];
                arrayMeasurement.CartesianX = createArraydesignModel.CartesianX[i];
                arrayMeasurement.CartesianY = createArraydesignModel.CartesianY[i];
                arrayMeasurement.IsWindy = false;
                arrayMeasurement.IsRainy = false;
                arrayMeasurement.MeasurementDate = DateTime.Now;
                arrayMeasurement.UserId = result.Data.CreatedPersonalId;
                arrayMeasurement.MeasurementImg = Constants.Constants.DefaultMeasurementPhoto;
                if (createArraydesignModel.GpsPointName.Length>0)
                {
                    arrayMeasurement.GpsXDesign = createArraydesignModel.GpsX[i];
                    arrayMeasurement.GpsYDesign = createArraydesignModel.GpsY[i];
                    arrayMeasurement.GpsXField = createArraydesignModel.GpsX[i];
                    arrayMeasurement.GpsYField = createArraydesignModel.GpsY[i];
                }
                if(createArraydesignModel.DesignNumber.Length>0)
                {
                    arrayMeasurement.DesignNumber = createArraydesignModel.DesignNumber[i];
                }
                arrayMeasurement.ArrayDesignIdKod = result.Data.IdKod;
                IResult resultArrMeasurement = await _arrayMeasurementService.Add(arrayMeasurement);
            }


            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("ArrayDesigns", "Arraydesign", null);

        }

        public async Task<IActionResult> ArrayDesigns()
        {
            var listArrayDesignAsync = await _arrayDesignService.GetListArrayDesign();
            List<ArrayDesignDto> listArrayDesign = listArrayDesignAsync.Data.OrderByDescending(x => x.ArrayCreatedDate).ToList();
            var arrayDesignListModel = _mapper.Map<List<ArrayDesignDto>, List<CreateArraydesignModel>>(listArrayDesign);

            return View(arrayDesignListModel);
        }

        public async Task<JsonResult> ArrayDesignDelete(string idKod, string userId)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var arrayDesign = await _arrayDesignService.GetByIdKod(idKod);

                List<string> roles = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
                bool isRole = false;
                foreach (var role in roles)
                {
                    if (role == "Manager" || role == "Admin")
                    {
                        isRole = true;
                    }
                }

                if (arrayDesign.Data.CreatedPersonalId == userId || isRole)
                {
                    if (arrayDesign.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _arrayDesignService.Delete(arrayDesign.Data);
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


        [Authorize(Roles = "Admin, Manager")]
        public async Task<JsonResult> ArrayDesignsDelete(string[] DeleteArrayDesigns)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteArrayDesigns)
                {
                    var measurement = await _arrayDesignService.GetByIdKod(idKod);
                    if (measurement.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _arrayDesignService.Delete(measurement.Data);
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


        public ActionResult Location()
        {
           
            List<ArrayDesign> arrayDesigns = _arrayDesignService.GetList().Result.Data;

            string designListArr = "[";
            string designs;
            string marker;
            foreach (var arrayDesign in arrayDesigns)
            {
                List<ArrayMeasurement> measurements = _arrayMeasurementService.GetList().Result.Data.Where(x => x.ArrayDesignIdKod == arrayDesign.IdKod).ToList();

                if (arrayDesign.ArrayFormat == 0)
                {
                    designs = "[";
                    marker = "[";
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
                    designs += "],";
                }
                else
                {
                    var designNumbers = measurements.GroupBy(p => p.DesignNumber).Select(g => g.First()).ToList();
                    designs = "[";
                    foreach (var item in designNumbers)
                    {
                        marker = "[";
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
                    designs += "],";

                }

                designListArr += designs;
            }

            designListArr += "]";


            ViewBag.DesignArray = designListArr;


            return View();
        }

    }
}
