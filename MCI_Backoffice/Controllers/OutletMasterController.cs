using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
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
    public class OutletMasterController : BaseController
    {
        SmartTapLogic smartlogic = new SmartTapLogic();
        
        public ActionResult Create()
        {
         
            return View();
        }
       
        [HttpPost]
        public async Task<ActionResult> Create(Outlet outlet)
        {
            if (ModelState.IsValid)
            {
               
                outlet.CreatedDateTime = DateTime.Now;
                outlet.IsActive = true;
                var ouletcreateresponse = await smartlogic.InsertOulet(outlet);
                if (ouletcreateresponse != null)
                {
                    if (ouletcreateresponse.Result == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Response = ouletcreateresponse.Message;
                        return View(outlet);

                    }
                }
            }
            
            return View(outlet);
        }

        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetOutletListAjax(DataTableParameters model, Search search)
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
                sortColumn = "Outlet Name";
            }
            if (sortBy == "1")
            {
                sortColumn = "Created Date";
            }
           


            if (Request.Params["order[0][dir]"] != null)
            {

                soryOrder = Request.Params["order[0][dir]"].ToString();

            }
            var pagingrequestmodel=new PagingRequestModel
                {
                SortBy = sortColumn,
                Sort=soryOrder,
                PageNumber=start,
                PageSize=model.Length,
                search=filterby
            };
            var spResponse =  await smartlogic.GetOutlets(pagingrequestmodel);

            if (spResponse != null && spResponse.Data!=null)
            {
                var outlet = new List<Outlet>();
                try
                {
                    outlet = (List<Outlet>)spResponse.Data;
                    if (outlet.Count > 0)
                    {
                        var TotalCount = outlet[0].TotalRecords;

                        var response = new
                        {
                            draw = model.draw,
                            data = outlet,
                            recordsFiltered = TotalCount,
                            recordsTotal = TotalCount
                        }; 
                        return Json(response, JsonRequestBehavior.AllowGet);
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
            var spResponse = await smartlogic.GetOutletById(get);
            var outlet = new Outlet();
            if (spResponse != null)
            {

                try
                {
                    outlet = (Outlet)spResponse.Data;
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(outlet);
        }

        public async Task<ActionResult> Edit(int id)
        {
            GetRequestModel get=new GetRequestModel();

            get.id = id;
            var spResponse = await smartlogic.GetOutletById(get);
            var outlet = new Outlet();
            if (spResponse != null)
            {
                
                try
                {
                outlet = (Outlet)spResponse.Data;
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
                return View(outlet);
        }
        [HttpPost]
         public async Task<ActionResult> Edit(Outlet outlet)
        {
            if (ModelState.IsValid)
            {
                outlet.IsActive = true;
                outlet.UpdatedDateTime=DateTime.Now;
                var spResponse = await smartlogic.InsertOulet(outlet);
               if(spResponse != null)
                {
                    return RedirectToAction("Index");
                }

            }
            return View("Edit");
        
        }

      
        public async Task<ActionResult> Delete(int id)
        {

            GetRequestModel get = new GetRequestModel();

            get.id = id;
            var spResponse = await smartlogic.GetOutletById(get);
            var outlet = new Outlet();
            if (spResponse != null)
            {

                try
                {
                    outlet = (Outlet)spResponse.Data;
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(outlet);
        }

     
        [HttpPost, ActionName("Delete")]
     
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                DeleteRequestModel model = new DeleteRequestModel();
                model.id = id;
                
                var spResponse = await smartlogic.InActiveOutlet(model);
                if (spResponse != null)
                {
                    return RedirectToAction("Index");
                }

            }
            return View("Edit");
        }

    }
}