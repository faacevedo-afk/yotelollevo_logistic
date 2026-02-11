using System.Web;
using yotelollevo.Constants;

namespace yotelollevo.Infrastructure
{
    public interface IUserSession
    {
        int UserId { get; }
        string UserName { get; }
        string Rol { get; }
        int? IdTienda { get; }
        int? IdRepartidor { get; }
        bool IsAdmin { get; }
        bool IsTienda { get; }
        bool IsRepartidor { get; }
    }

    public class HttpUserSession : IUserSession
    {
        private readonly HttpSessionStateBase _session;

        public HttpUserSession(HttpSessionStateBase session)
        {
            _session = session;
        }

        public int UserId
        {
            get
            {
                var v = _session["UserId"];
                if (v == null) return 0;
                int id;
                return int.TryParse(v.ToString(), out id) ? id : 0;
            }
        }

        public string UserName => (_session["UserName"] as string) ?? "";

        public string Rol => (_session["Rol"] as string ?? "").Trim().ToUpperInvariant();

        public int? IdTienda
        {
            get
            {
                var v = _session["IdTienda"];
                if (v == null) return null;
                int id;
                return int.TryParse(v.ToString(), out id) ? (int?)id : null;
            }
        }

        public int? IdRepartidor
        {
            get
            {
                var v = _session["IdRepartidor"];
                if (v == null) return null;
                int id;
                return int.TryParse(v.ToString(), out id) ? (int?)id : null;
            }
        }

        public bool IsAdmin => Rol == RoleNames.Admin;
        public bool IsTienda => Rol == RoleNames.Tienda;
        public bool IsRepartidor => Rol == RoleNames.Repartidor;

        public static void SetLogin(HttpSessionStateBase session, int userId, string userName, string rol, int? idTienda, int? idRepartidor)
        {
            session.Clear();
            session["UserId"] = userId;
            session["UserName"] = userName;
            session["Rol"] = rol;
            session["IdTienda"] = idTienda;
            session["IdRepartidor"] = idRepartidor;
        }
    }
}
