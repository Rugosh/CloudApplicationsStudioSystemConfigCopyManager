using CliFx;
using CliFx.Attributes;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Linq;

namespace CloudApplicationsStudioSystemConfigCopyCLI
{
    [Command]
    public class CloudStudioConfigMover : ICommand
    {
        [CommandOption("source", 's', Description = "Source Version of the SAP Cloud Application Studio")]
        public string SourceVersion { get; set; }

        [CommandOption("target", 't', Description = "Target Version of the SAP Cloud Application Studio")]
        public string TargetVersion { get; set; }

        [CommandOption("auto", 'a', Description = "Automatic choose the versions")]
        public bool AutoChoose { get; set; }

        public CloudStudioConfigMover()
        {
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            {
                Console.WriteLine("Function ony supported on Windows!");
                return default;
            }

            if (AutoChoose)
            {
                InitVersions();
            }

            if (SourceVersion == null || TargetVersion == null)
            {
                Console.WriteLine("Source and Target must be set!");
                return default;
            }

            console.Output.WriteLine("Cloud Studio Config will be moved");
            console.Output.WriteLine(string.Format("Source: {0}", SourceVersion));
            console.Output.WriteLine(string.Format("Target: {0}", TargetVersion));

            var targetPath = string.Format(@"Computer\HKEY_CURRENT_USER\SOFTWARE\SAP\SAPCloudApplicationsStudio\{0}\DialogPage\SAP.Copernicus.CopernicusOptionPage", SourceVersion);

            string sourceConfigString = null;
            using (RegistryKey sourceKey = Registry.CurrentUser.OpenSubKey(string.Format(@"SOFTWARE\SAP\SAPCloudApplicationsStudio\{0}\DialogPage\SAP.Copernicus.CopernicusOptionPage", SourceVersion)))
            {
                if (sourceKey != null)
                {
                    sourceConfigString = sourceKey.GetValue("SystemConnections").ToString();
                }
            }
            if (sourceConfigString != null)
            {
                using (RegistryKey targetKey = Registry.CurrentUser.OpenSubKey(string.Format(@"SOFTWARE\SAP\SAPCloudApplicationsStudio\{0}\DialogPage\SAP.Copernicus.CopernicusOptionPage", TargetVersion), true))
                {
                    if (targetKey != null)
                    {
                        targetKey.SetValue("SystemConnections", sourceConfigString);
                    }
                }
            }

            // Return empty task because our command executes synchronously
            return default;
        }

        private void InitVersions()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\SAP\SAPCloudApplicationsStudio"))
                {
                    if (key != null)
                    {
                        var keyNames = key.GetSubKeyNames();
                        if (keyNames != null)
                        {
                            foreach (var keyName in keyNames.OrderByDescending(k => k))
                            {
                                if (keyName.Length != 4)
                                {
                                    continue;
                                }

                                if (TargetVersion == null)
                                {
                                    TargetVersion = keyName;
                                }
                                else if (SourceVersion == null)
                                {
                                    SourceVersion = keyName;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
