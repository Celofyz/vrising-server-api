namespace VrisingApi;

internal class VrisingLog : IVrisingLog
{
    private readonly IConfig config;

    public VrisingLog(IConfig config)
    {
        this.config = config;
    }

    public VrisingServerData GetServerData()
    {
        var logFile = File.ReadLines(config.VrisingLogPath);

        var steamId = GetServerSteamId(logFile);
        var status = GetServerStatus(logFile);
        var startTime = GetStartTime(logFile);
        var version = GetVersion(logFile);
        var userCount = GetUserCount(logFile);

        return new VrisingServerData(steamId, status, startTime, version, userCount);
    }

    private string GetServerSteamId(IEnumerable<string> logFile)
    {
        const string searchPattern = "Successfully logged in with the SteamGameServer API. SteamID:";
        const int idIndex = 1;

        var splittedLine = logFile
            .Where(line => line.Contains(searchPattern))
            .Single()
            .Split(searchPattern);

        var steamId = splittedLine[idIndex].Trim();
        return steamId;
    }

    private string GetServerStatus(IEnumerable<string> logFile)
    {
        const string searchPattern = "OnApplicationQuit()";

        var ifOffline = logFile
            .Contains(searchPattern);

        return ifOffline ? "OFFLINE" : "ONLINE";
    }

    private string GetStartTime(IEnumerable<string> logFile)
    {
        const string searchPattern = "Bootstrap - Boot Time: ";
        const int dateIndex = 0;
        const int datePartIndex = 1;

        var splittedLine = logFile
            .Where(line => line.Contains(searchPattern))
            .Single()
            .Split(',');

        var datePart = splittedLine[dateIndex].Split(searchPattern);
        var date = datePart[datePartIndex].Trim();

        return date;
    }

    private string GetVersion(IEnumerable<string> logFile)
    {
        const string lineSearchPattern = "Bootstrap - Boot Time: ";
        const string versionSearchPattern = "Version: ";
        const int versionIndex = 1;
        const int versionPartIndex = 1;

        var splittedLine = logFile
            .Where(line => line.Contains(lineSearchPattern))
            .Single()
            .Split(',');

        var datePart = splittedLine[versionIndex].Split(versionSearchPattern);
        var date = datePart[versionPartIndex].Trim();

        return date;
    }

    private string GetUserCount(IEnumerable<string> logFile)
    {
        const string userConnectionPattern = "User '";
        const string connected = "connected as ID";
        const string disconnected = "disconnected";

        var connectionLogs = logFile
            .Where(line => line.Contains(userConnectionPattern));

        var connectCount = connectionLogs
            .Where(line => line.Contains(connected))
            .Count();

        var disconnectCount = connectionLogs
            .Where (line => line.Contains(disconnected))
            .Count();

        return (connectCount - disconnectCount).ToString();
    }
}
