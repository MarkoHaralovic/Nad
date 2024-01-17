namespace StoZelisCitati.Helpers;

public static class TemplatingHelper
{
    public static string SelectedIf(this string? query, string? value) => query == value ? "selected" : "";
}