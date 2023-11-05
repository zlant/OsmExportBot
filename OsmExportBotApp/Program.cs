OsmExportBot.Config.Token = "TOKEN";

while (true)
{
    OsmExportBot.Bot.Run(null);
    await Task.Delay(TimeSpan.FromSeconds(1));
}
