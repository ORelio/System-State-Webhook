using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpTools;

namespace SystemStateWebhook
{
    /// <summary>
    /// Holds Webhook settings
    /// </summary>
    class Settings
    {
        public static readonly string ExeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static readonly string ConfigFile = Path.Combine(ExeDir, "Webhooks.ini");

        /// <summary>
        /// Startup Webhook URL
        /// </summary>
        public static string WebhookStartup { get; set; }

        /// <summary>
        /// Logon / Unlock Webhook URL
        /// </summary>
        public static string WebhookLogon { get; set; }

        /// <summary>
        /// Logoff / Lock Webhook URL
        /// </summary>
        public static string WebhookLogoff { get; set; }

        /// <summary>
        /// Shutdown Webhook URL
        /// </summary>
        public static string WebhookShutdown { get; set; }

        /// <summary>
        /// Load settings from disk
        /// </summary>
        /// <returns>TRUE if the config file exists and seems valid</returns>
        public static bool Load()
        {
            if (File.Exists(ConfigFile))
            {
                Dictionary<string, Dictionary<string, string>> config = INIFile.ParseFile(ConfigFile);
                if (config.ContainsKey("webhooks"))
                {
                    if (config["webhooks"].ContainsKey("startup") && !String.IsNullOrEmpty(config["webhooks"]["startup"]))
                        WebhookStartup = config["webhooks"]["startup"];
                    if (config["webhooks"].ContainsKey("logon") && !String.IsNullOrEmpty(config["webhooks"]["logon"]))
                        WebhookLogon = config["webhooks"]["logon"];
                    if (config["webhooks"].ContainsKey("logoff") && !String.IsNullOrEmpty(config["webhooks"]["logoff"]))
                        WebhookLogoff = config["webhooks"]["logoff"];
                    if (config["webhooks"].ContainsKey("shutdown") && !String.IsNullOrEmpty(config["webhooks"]["shutdown"]))
                        WebhookShutdown = config["webhooks"]["shutdown"];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Save current settings to disk
        /// </summary>
        public static void Save()
        {
            Dictionary<string, Dictionary<string, string>> config = new Dictionary<string, Dictionary<string, string>>();
            config["webhooks"] = new Dictionary<string, string>();
            config["webhooks"]["startup"] = String.Format("http://192.168.1.123/webhook/pcstate?computer={0}&state=startup", Environment.MachineName);
            config["webhooks"]["logon"] = String.Format("http://192.168.1.123/webhook/pcstate?computer={0}&state=logon", Environment.MachineName);
            config["webhooks"]["logoff"] = String.Format("http://192.168.1.123/webhook/pcstate?computer={0}&state=logoff", Environment.MachineName);
            config["webhooks"]["shutdown"] = String.Format("http://192.168.1.123/webhook/pcstate?computer={0}&state=shutdown", Environment.MachineName);
            INIFile.WriteFile(ConfigFile, config, " " + String.Join(Environment.NewLine + "# ", new [] {
                "Webhooks Configuration File",
                "",
                "Define Webhooks here for the following system events:",
                "- Startup: Happens on first session start after system start",
                "- Logon: Happens on system start, session start, session resume",
                "- Logoff: Happens on system shutdown, session end, session lock, sleep and hibernate",
                "- Shutdown: Happens on system shutdown",
                "",
                "To disable a webhook, remove the setting or leave it empty"
            }));
        }
    }
}
