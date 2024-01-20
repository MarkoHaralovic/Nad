namespace StoZelisCitati.Helpers;

public static class HtmxHelper
{
    public static bool PartialHtmx(this HttpRequest request)
    {
        return request.Headers.TryGetValue("HX-Request", out _) &&
               !(request.Headers.TryGetValue("HX-History-Restore-Request", out var value) && value == "true");
    }
}