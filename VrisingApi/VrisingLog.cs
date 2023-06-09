namespace VrisingApi;

internal class VrisingLog : IVrisingLog
{
    private readonly IConfig config;

    public VrisingLog(IConfig config)
    {
        this.config = config;
    }

    public long ServerSteamId
    {
        get
        {
            const string searchPattern = "Successfully logged in with the SteamGameServer API. SteamID:";
            const int idIndex = 1;

            var splittedLine = File.ReadLines(config.VrisingLogPath)
                .Where(line => line.Contains(searchPattern)).Single()
                .Split(searchPattern);

            var steamId = splittedLine[idIndex].Trim();
            return long.Parse(steamId);
        }
    }
}
