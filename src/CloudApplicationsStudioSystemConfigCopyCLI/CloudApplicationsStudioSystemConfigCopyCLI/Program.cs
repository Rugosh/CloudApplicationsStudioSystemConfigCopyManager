using CliFx;
using System;
using System.Threading.Tasks;

namespace CloudApplicationsStudioSystemConfigCopyCLI
{
    public static class Program
    {
        public static async Task<int> Main() =>
            await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
    }
}
