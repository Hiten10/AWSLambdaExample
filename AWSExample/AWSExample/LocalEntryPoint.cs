using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AWSExample
{
    /// <summary>
    /// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    public class LocalEntryPoint
    {
        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        private static readonly LambdaEntryPoint LambdaEntryPoint = new LambdaEntryPoint();
        private static readonly Func<APIGatewayProxyRequest, ILambdaContext, Task<APIGatewayProxyResponse>> Func = LambdaEntryPoint.FunctionHandlerAsync;

        public static void Main(string[] args)
        {
#if DEBUG
            BuildWebHost(args).Run();
#else
            // Wrap the FunctionHandler method in a form that LambdaBootstrap can work with.
            using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(Func, new JsonSerializer()))

            // Instantiate a LambdaBootstrap and run it.
            // It will wait for invocations from AWS Lambda and call the handler function for each one.
            using (var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
#endif
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
