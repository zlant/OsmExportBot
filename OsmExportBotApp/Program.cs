
Timer timer;
long interval = 1000;

OsmExportBot.Config.Token = "TOKEN";
timer = new Timer(new TimerCallback(OsmExportBot.Bot.Run), null, 0, interval);
//OsmExportBot.Bot.Run( null );
Console.ReadLine();
