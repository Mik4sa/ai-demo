using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class DateTool
{
    [McpServerTool, Description("Gets today's date.")]
    [return: Description("Returns today's date.")]
    public static DateTime GetDateToday()
    {
        return DateTime.Today;
    }
}