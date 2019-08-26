using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageProcessingSimulator
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureHostConfiguration(
                    (config) =>
                    {
                        config
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("hostsettings.json")
                            .AddEnvironmentVariables();
                    })
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        builder
                            .AddJsonFile("appsettings.json", true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args);
                    })
                .ConfigureServices(
                    (context, collection) =>
                    {
                        collection
                            .AddOptions()
                            .Configure<AppOption>(options => context.Configuration.GetSection("App").Bind(options))
                            .AddSingleton<IMessageQueue, MessageQueue>()
                            .AddScoped<IMessageGenerator, MessageGenerator>()
                            .AddScoped<ISingleTypeMessageConsumer, SingleTypeMessageConsumer>()
                            .AddScoped<IMessageConsumerFactory, MessageConsumerFactory>()
                            .AddTransient<IFileLogger, FileLogger>()
                            .AddHostedService<MessagePublisherService>()
                            .AddHostedService<MessageConsumerService>();
                    })
                .ConfigureLogging(
                    (context, builder) =>
                    {
                        builder
                            .AddConfiguration(context.Configuration.GetSection("Logging"));

                        if (context.HostingEnvironment.EnvironmentName == EnvironmentName.Development)
                        {
                            builder.AddConsole();
                        }
                    });
    }
}
