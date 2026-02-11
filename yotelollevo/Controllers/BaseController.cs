using System.Web.Mvc;
using yotelollevo.Infrastructure;

namespace yotelollevo.Controllers
{
    public abstract class BaseController : Controller
    {
        private IUserSession _currentUser;

        protected IUserSession CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = new HttpUserSession(Session);
                return _currentUser;
            }
        }
    }
}
