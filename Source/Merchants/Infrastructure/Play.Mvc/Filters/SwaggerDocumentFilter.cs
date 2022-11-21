using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Play.Mvc.Attributes;

using PlPlay.Mvc.Attributeswagger;

using Swashbuckle.AspNetCore.SwaggerGen;

usSwashbuckle.AspNetCore.SwaggerGenrGen;

namespace Play.Mvc.Filters
{
    public class SwaggerDocumentFilter : IDocumentFilter
 
        #region Instance Membersbers

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription description in context.ApiDescriptions)
            {
                description.TryGetMethodInfo(out MethodInfo info);

                if (!IsIgnoreAttributePresent(info))
                    continue;

                string? kepath = description.RelativePath;

                if (kepath is null)
                    continue;

                List<KeyValuePair<string, OpenApiPathItem>> removeRoutes = swaggerDoc.Paths.Where(x => x.Key.ToLower().Contains(kepath.ToLower())).ToList();

                removeRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
            }
        }

        /// <exception cref="TypeLoadException"></exception>
        private static bool IsIgnoreAttributePresent(MethodInfo info)
        {
            if (info.DeclaringType?.GetCustomAttributes(true).OfType<SwaggerIgnoreAttribute>().Any() ?? false)
                return true;

            if (info.GetCustomAttributes(true).OfType<SwaggerIgnoreAttribute>().Distinct().Any())
                return true;

            return false;
     

        #endregion
        }
}