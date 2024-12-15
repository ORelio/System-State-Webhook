using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Windows.Forms;
using SharpTools;

namespace SystemStateWebhook
{
    /// <summary>
    /// System State Webhook
    /// Monitor startup, session leave, session resume and shutdown events, and pass them to a webhook.
    /// By ORelio (c) 2024 - CDDL 1.0
    /// </summary>
    static class Program
    {
        const string Version = "1.0.0";

        /// <summary>
        /// Output log to console with timestamp
        /// </summary>
        /// <param name="message">Message to print to console</param>
        private static void LogWithTimestamp(object message)
        {
            Console.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
        }

        /// <summary>
        /// The main entry point for the application
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Contains("--hide-console") && ConsoleWindow.IsAllocated())
                ConsoleWindow.Hide();

            if (args.Contains("--show-console") && !ConsoleWindow.IsAllocated())
                ConsoleWindow.Allocate();

            if (ConsoleWindow.IsAllocated())
                Console.Title = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);

            if (Settings.Load())
            {
                LogWithTimestamp(typeof(Program).Namespace + " v" + Version + " - By ORelio");
                LogWithTimestamp(String.Format("Startup Webhook: {0}", Settings.WebhookStartup));
                LogWithTimestamp(String.Format("Logon Webhook: {0}", Settings.WebhookLogon));
                LogWithTimestamp(String.Format("Logoff Webhook: {0}", Settings.WebhookLogoff));
                LogWithTimestamp(String.Format("Shutdown Webhook: {0}", Settings.WebhookShutdown));
                Application.Run(new SessionEventMonitor(EventCallback, typeof(Program).Namespace, "Calling Webhook"));
            }
            else
            {
                LogWithTimestamp("Generating Webhooks config file, please fill it and relaunch.");
                Settings.Save();
                LogWithTimestamp("Press any key to exit");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Handle session event callback
        /// </summary>
        /// <param name="eventType">Session event type</param>
        private static void EventCallback(SessionEventMonitor.SessionEvent eventType)
        {
            LogWithTimestamp(String.Format("System Event: {0}", eventType));

            try
            {
                switch (eventType)
                {
                    case SessionEventMonitor.SessionEvent.Startup:
                        CallWebHook(Settings.WebhookStartup);
                        break;

                    case SessionEventMonitor.SessionEvent.Logon:
                        CallWebHook(Settings.WebhookLogon);
                        break;

                    case SessionEventMonitor.SessionEvent.Logoff:
                        CallWebHook(Settings.WebhookLogoff);
                        break;

                    case SessionEventMonitor.SessionEvent.Shutdown:
                        CallWebHook(Settings.WebhookShutdown);
                        break;

                    default:
                        throw new NotImplementedException(String.Format("Unknown event type: {0}", eventType));
                }
            }
            catch (System.Net.WebException e)
            {
                LogWithTimestamp(String.Format("Failed to call Webhook: {0}: {1}", e.GetType(), e.Message));
            }
        }

        /// <summary>
        /// Call specified Webhook URL
        /// </summary>
        /// <param name="url">Webhook URL</param>
        private static void CallWebHook(string url)
        {
            LogWithTimestamp(String.Format("Requesting: {0}", url));

            //Configure System.Net to use TLS 1.2 - disabled by default for .NET 4.0 - Stackoverflow 33761919
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072; //Tls12

            using (WebClient wc = new WebClient())
            {
                wc.DownloadString(url);
            }
        }
    }
}
