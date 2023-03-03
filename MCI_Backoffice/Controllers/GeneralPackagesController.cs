using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.SmartTap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
     public class GeneralPackagesController : BaseController
    {
        SmartTapLogic smartlogic = new SmartTapLogic();

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(GeneralPackages  general)
        {
            if (ModelState.IsValid)
            {


                var generalcreateresponse = await smartlogic.InsertGeneralPackage(general);
                if (generalcreateresponse != null)
                {
                    if (generalcreateresponse.Result == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Response = generalcreateresponse.Message;
                        return View(general);

                    }
                }
            }

            return View(general);
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> GetPackageListAjax(DataTableParameters model, Search search)
        {
            int start = 0;

            if (model.Start > 0)
            {
                start = model.Start / model.Length;
            }

            start += 1;

            string filterby = string.Empty;
            string soryOrder = "DESC";
            string sortBy = "";
            string sortColumn = "";

            if (Request.Params["search[value]"] != null)
            {
                filterby = Request.Params["search[value]"].ToString();
            }



            if (Request.Params["order[0][column]"] != null)
            {
                sortBy = Request.Params["order[0][column]"].ToString();

            }
            if (sortBy == "0")
            {
                sortColumn = "PackageDescription";
            }
            if (sortBy == "1")
            {
                sortColumn = "PackageCode";
            }



            if (Request.Params["order[0][dir]"] != null)
            {

                soryOrder = Request.Params["order[0][dir]"].ToString();

            }
            var pagingrequestmodel = new PagingRequestModel
            {
                SortBy = sortColumn,
                Sort = soryOrder,
                PageNumber = start,
                PageSize = model.Length,
                search = filterby
            };
            var spResponse = await smartlogic.GetPackages(pagingrequestmodel);

            if (spResponse != null)
            {
                var outlet = new List<GeneralPackages>();
                try
                {
                    outlet = (List<GeneralPackages>)spResponse.Data;
                    if (outlet != null)
                    {
                        var TotalCount = outlet[0].TotalRecords;

                        var response = new
                        {
                            draw = model.draw,
                            data = outlet,
                            recordsFiltered = TotalCount,
                            recordsTotal = TotalCount
                        }; return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var response = new
                        {
                            draw = model.draw,
                            data = new List<GeneralPackages>(),
                            recordsFiltered = 0,
                            recordsTotal = 0
                        }; return Json(response, JsonRequestBehavior.AllowGet);

                    }


                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                var response = new
                {
                    draw = model.draw,
                    data = spResponse,
                    recordsFiltered = 0,
                    recordsTotal = 0
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> Details(int id)
        {


            GetRequestModel get = new GetRequestModel();
            get.id = id;
            var spResponse = await smartlogic.GetPackageById(get);
            var general = new GeneralPackages();
            if (spResponse != null)
            {

                try
                {
                    general = (GeneralPackages)spResponse.Data;
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(general);
        }


        public async Task<ActionResult> Edit(int id)
        {

            GetRequestModel get = new GetRequestModel();
            get.id = id;
            var spResponse = await smartlogic.GetPackageById(get);
            var general = new GeneralPackages();
            if (spResponse != null)
            {

                try
                {
                    general = (GeneralPackages)spResponse.Data;
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(general);
        }
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Exclude = "TotalRecords")] GeneralPackages  general)
        {
            if (ModelState.IsValid)
            {
                general.IsActive = true;
                var spResponse = await smartlogic.InsertGeneralPackage(general);
                if (spResponse != null)
                {
                    return RedirectToAction("Index");
                }

            }
            var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
            return View(general);

        }


        public async Task<ActionResult> Delete(int id)
        {

            GetRequestModel get = new GetRequestModel();
            get.id = id;
            var spResponse = await smartlogic.GetPackageById(get);
            var general = new GeneralPackages();
            if (spResponse != null)
            {

                try
                {
                    general = (GeneralPackages)spResponse.Data;
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(general);
        }


        [HttpPost, ActionName("Delete")]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                DeleteRequestModel model = new DeleteRequestModel();
                model.id = id;
                var spResponse = await smartlogic.InActivePackages(model);
                if (spResponse != null)
                {
                    return RedirectToAction("Index");
                }

            }
            return View("Edit");
        }

    }
}