using System;
using System.Net.Http;
using System.Threading.Tasks;
using InternalPortal.Configuration;
using InternalPortal.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace InternalPortal.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseExternalServices(this IServiceCollection services, IConfiguration configuration)
        {

            var api = configuration.GetSection("Services:Api").Get<ApiConfig>();

            services.AddHttpClient<IGetOrganisationsService, GetOrganisationsService>(client
                =>
            { client.BaseAddress = new Uri(api.InternalApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));

            services.AddHttpClient<IGetOrganisationDetailsService, GetOrganisationDetailsService>(client
                =>
            { client.BaseAddress = new Uri(api.InternalApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));

            services.AddHttpClient<IGetApplicationsService, GetApplicationsService>(client
                =>
            { client.BaseAddress = new Uri(api.InternalApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));

            services.AddHttpClient<IGetDocumentService, GetDocumentService>(client 
                        => 
                    { client.BaseAddress = new Uri(api.DocumentsApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));
            
            services.AddHttpClient<IGetApplicationDetailsService, GetApplicationDetailsService>(client
                        =>
                    { client.BaseAddress = new Uri(api.InternalApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));
            
            services.AddHttpClient<IUpdateOrganisationStatusService, UpdateOrganisationStatusService>(client
                        =>
                    { client.BaseAddress = new Uri(api.InternalApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));
            
            services.AddHttpClient<IUpdateApplicationStatusService, UpdateApplicationStatusService>(client
                        =>
                    { client.BaseAddress = new Uri(api.InternalApiBaseUri); })
                .AddPolicyHandler(GetRetryPolicy(api.RetryCount, api.RetryIntervalSeconds));
            
            services.AddLogging();

            return services;
        }
        
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, double retryInterval)
        {
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound);

            if (retryCount != default || retryInterval != default)
            {
                return policy
                    .WaitAndRetryAsync(retryCount, retryAttempt
                        => TimeSpan.FromSeconds(Math.Pow(retryInterval, retryAttempt)));
            }

            return Task.FromResult(policy) as IAsyncPolicy<HttpResponseMessage>;
        }
    }
}