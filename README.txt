System State Webhook v1.0.0 - By ORelio - Microzoom.fr
https://github.com/ORelio/System-State-Webhook/

Thanks for downloading System State Webhook!
System State Webhook is free software allowing to call the desired URL (WebHook) on Windows startup, logon, logoff, shutdown.

Events
-------

The following events are implemented:

* Startup: Happens on first session start after system start
* Logon: Happens on system start, session start, session resume
* Logoff: Happens on system shutdown, session end, session lock, sleep and hibernate
* Shutdown: Happens on system shutdown

Note: For logoff event when AFK, simply configure a lock screen timeout on your system.

Setup
------

1. Launch SystemStateWebhook.exe once to generate configuration file
2. Edit Webhooks.ini and set desired webhooks for each event
3. Launch SystemStateWebhook.exe and try locking your session to see if it works well
4. Create a shortcut to SystemStateWebhookHidden.exe inside this folder to have it run on startup/logon:
   %appdata%\Microsoft\Windows\Start Menu\Programs\Startup

Credits
-------

System State Webhook has been created using the following resources:

 - Sharp-Tools library, by ORelio - https://github.com/ORelio/Sharp-Tools
 - Webhook icon, by Austin Andrews, with slight color changes (Icon)
 - Handel Gothic font, by Donald J. Handel & Robert Trogman (Logo)

-------------
© 2024 ORelio
-------------
