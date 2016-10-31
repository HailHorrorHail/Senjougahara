namespace TaskManager
{
    using TaskManager.Models;
    using System.Web.Http;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Event>("Events").EntityType.HasKey(k => k.Id);

            config.MapODataServiceRoute("ODataRoute", null, builder.GetEdmModel());
        }
    }
}
