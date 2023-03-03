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
    public class OutletUserMasterController : BaseController
    {
        SmartTapLogic smartlogic = new SmartTapLogic();

        public async Task<ActionResult> Create()
        {
            GetRequestModel get =new GetRequestModel();
            get.id = null;
            var outlet = await smartlogic.GetOutlets(get);
            if (outlet != null)
            {
                ViewBag.Outlets = (List<Outlet>)outlet.Data;  
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(OutletUser outlet)
        {
            
            if (ModelState.IsValid)
            {
                //OutletIDDetail ou =new OutletIDDetail();
                //ou.OutletID = 1;
                ////ou.OutletName = "sfds";
                //List<OutletIDDetail> outs=new List<OutletIDDetail>();
                //outs.Add(ou);
                //outlet.Outlets=outs;
                if(outlet.ID != null)
                {
                    outlet.UpdatedDateTime = DateTime.Now;
                }
                else
                {
                    outlet.CreatedDateTime = DateTime.Now;
                }
               
                     var ouletcreateresponse = await smartlogic.InsertOuletUser(outlet);
                if (ouletcreateresponse != null)
                {

                    if (ouletcreateresponse.Result == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Response = ouletcreateresponse.Message;
                        GetRequestModel get = new GetRequestModel();
                        get.id = null;
                        var outlets = await smartlogic.GetOutlets(get);
                        if (outlets != null)
                        {
                            ViewBag.Outlets = (List<Outlet>)outlets.Data;
                        }
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
        public async Task<ActionResult> GetOutletUserListAjax(DataTableParameters model, Search search)
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
                sortColumn = "User Name";
            }
            if (sortBy == "1")
            {
                sortColumn = "Created Date";
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
            var spResponse = await smartlogic.GetOutletUsers(pagingrequestmodel);

            if (spResponse != null)
            {
                var outlet = new List<OutletUser>();
                try
                {
                    outlet = (List<OutletUser>)spResponse.Data;

                    if (outlet!=null)
                    { var TotalCount = outlet[0].TotalRecords;
                   
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
                            data = new List<OutletUser>(),
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


        public async Task<ActionResult> Edit(int id)
        {
            GetRequestModel get = new GetRequestModel();

            get.id = id;
            var spResponse = await smartlogic.GetOutletUserById(get);

            var outletuser = new OutletUser();
            if (spResponse != null)
            {

                try
                {
                  
                    outletuser = (OutletUser)spResponse.Data;
                    if(outletuser!=null)
                    {
                       spResponse = await smartlogic.GetOutletsByUser(id);
                        if (spResponse != null)
                        {


                            var outlets = (List<Outlet>)spResponse.Data;
                            if (outlets.Count() > 0)
                            {
                                outletuser.SelectedOutlets=
                                    
                                (string.Join(",", outlets.ToList().Select(x => x.OutletID)));
                            }
                        }
                    }
                    get.id = null;
                    var outlet = await smartlogic.GetOutlets(get);
                    if (outlet != null)
                    {
                        ViewBag.Outlets = (List<Outlet>)outlet.Data;
                    }
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(outletuser);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(OutletUser outlet)
        {
          

                var spResponse = await smartlogic.InsertOuletUser(outlet);
                if (spResponse != null)
                {
                    return RedirectToAction("Index");
                }

            
            return View("Edit");

        }


        public async Task<ActionResult> Delete(int id)
        {

            GetRequestModel get = new GetRequestModel();

            get.id = id;
            var spResponse = await smartlogic.GetOutletUserById(get);
            var outlet = new OutletUser();
            if (spResponse != null)
            {

                try
                {
                    outlet = (OutletUser)spResponse.Data;
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
               
                var spResponse = await smartlogic.InActiveOutletUsers(model);
                if (spResponse != null)
                {
                    return RedirectToAction("Index");
                }

            }
            return View("Edit");
        }
        public async Task<ActionResult> Details(int id)
        {
            GetRequestModel get = new GetRequestModel();

            get.id = id;
            var spResponse = await smartlogic.GetOutletUserById(get);
            var outlet = new OutletUser();
            if (spResponse != null)
            {

                try
                {
                    outlet = (OutletUser)spResponse.Data;
                    if(outlet!=null)
                    { 
                    spResponse = await smartlogic.GetOutletsByUser(id);

                        if (spResponse != null)
                        {

                            try
                            {
                                outlet.Outlet = (List<Outlet>)spResponse.Data;
                                
                            }
                            catch (Exception ex)
                            {
                                return PartialView();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return View(outlet);
        }
        [HttpPost]
        public async Task<ActionResult> GetOutletsDetails(int userid)
        {
         
            var spResponse = await smartlogic.GetOutletsByUser(userid);
            var outlet = new List<Outlet>();
            if (spResponse != null)
            {

                try
                {
                    outlet = (List<Outlet>)spResponse.Data;
                    return PartialView(outlet);
                }
                catch (Exception ex)
                {
                    return PartialView();
                }
            }
            return PartialView();
        }

    }
}