﻿namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder ConfigureClient<TClientOptions>(
            this IHttpClientBuilder httpClientBuilder,
            IConfiguration config,
            Action<OptionsBuilder<TClientOptions>> optionsConfigurator) 
            where TClientOptions : BaseClientOptions
        {
            if(httpClientBuilder == null)
                throw new ArgumentNullException(nameof(httpClientBuilder));
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (optionsConfigurator == null)
                throw new ArgumentNullException(nameof(optionsConfigurator));

            var services = httpClientBuilder.Services;
            services.Configure<TClientOptions>(config);
            optionsConfigurator(services.AddOptions<TClientOptions>());

            return httpClientBuilder;
        }

        public static IHttpClientBuilder UseDefaultPrimaryMessageHandler(
            this IHttpClientBuilder httpClientBuilder,
            Func<HttpClientHandler, HttpClientHandler> handlerConfigurator)
        {
            if (httpClientBuilder == null)
                throw new ArgumentNullException(nameof(httpClientBuilder));

            return httpClientBuilder.ConfigurePrimaryHttpMessageHandler(
                x => handlerConfigurator(new HttpClientHandler())
            );
        }
    }
}