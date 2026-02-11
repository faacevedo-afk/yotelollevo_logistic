using System.Web.Mvc;
using yotelollevo.Filter;
using yotelollevo.Services;

namespace yotelollevo.Controllers
{
    [RoleAuthorize]
    public class HomeController : BaseController
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();
        private readonly IDashboardService _dashboardService;

        public HomeController()
        {
            _dashboardService = new DashboardService(db, new PaqueteService(db));
        }

        public ActionResult Index()
        {
            var viewModel = _dashboardService.GetDashboardData();
            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
