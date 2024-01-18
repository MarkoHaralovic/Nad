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

    public static TOut Map<TIn, TOut>(this TIn obj, Func<TIn, TOut> map)
        => map(obj);
}