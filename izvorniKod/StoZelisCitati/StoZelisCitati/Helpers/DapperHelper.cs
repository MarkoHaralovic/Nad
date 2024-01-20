using Dapper;

namespace StoZelisCitati.Helpers;

public static class DapperHelper
{
    public static DynamicParameters Append(this DynamicParameters dynamicParameters, params object?[] objects)
    {
        foreach (object? o in objects)
            dynamicParameters.AddDynamicParams(o);

        return dynamicParameters;
    }
}