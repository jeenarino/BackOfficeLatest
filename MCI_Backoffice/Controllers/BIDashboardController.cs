using PowerBIEmbedded_AppOwnsData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class BIDashboardController : Controller
    {
        private readonly IEmbedService m_embedService;

        public BIDashboardController()
        {
            m_embedService = new EmbedService();
        }


        public async Task<ActionResult> EmbedReport(string username, string roles)
        {
            var embedResult = await m_embedService.EmbedReport(username, roles);
            if (embedResult)
            {
                return View(m_embedService.EmbedConfig);
            }
            else
            {
                return View(m_embedService.EmbedConfig);
            }
        }


        // GET: BIDashboard
        public async Task<ActionResult> Index()
        {
            string username = string.Empty; string roles = string.Empty;
            var embedResult = await m_embedService.EmbedReport(username, roles);
            if (embedResult)
            {
                return View(m_embedService.EmbedConfig);
            }
            else
            {
                return View(m_embedService.EmbedConfig);
            }


            //var embedResult = await m_embedService.EmbedDashboard();
            //if (embedResult)
            //{
            //    return View(m_embedService.EmbedConfig);
            //}
            //else
            //{
            //    return View(m_embedService.EmbedConfig);
            //}
        }
    }
}