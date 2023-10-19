using Stack.ServiceLayer.Primitives;

namespace Stack.API.Extensions
{
    public static class PrimitivesExtensions
    {

        public static void AddPrimitives(this IServiceCollection caller)
        {

            caller.AddScoped<IMediaUploader, MediaUploader>();
            caller.AddScoped<ISMSSender, SMSSender>();
            caller.AddScoped<IHTTPSender, HTTPSender>();
        }

    }

}
