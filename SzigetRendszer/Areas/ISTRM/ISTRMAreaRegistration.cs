using System.Web.Mvc;

namespace SzigetRendszer.Areas.ISTRM
{
    public class ISTRMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ISTRM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ISTRM_default",
                "ISTRM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}