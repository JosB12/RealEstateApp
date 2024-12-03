namespace RealEstateApp.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IEndpointRouteBuilder routeBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var versionDescriptions = routeBuilder.DescribeApiVersions();

                if (versionDescriptions != null && versionDescriptions.Any())
                {
                    foreach (var apiVersion in versionDescriptions)
                    {
                        var url = $"/swagger/{apiVersion.GroupName}/swagger.json";
                        var name = $"Stock Api - {apiVersion.GroupName.ToUpperInvariant()}";
                        options.SwaggerEndpoint(url, name);
                    }
                }
            });
        }
    }
}
