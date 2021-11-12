﻿using Azure;
using Azure.Data.Tables;

namespace L04;

public static class Extensions
{
    public static async Task<IEnumerable<T>> AsEnumerable<T>(this AsyncPageable<T> result)
        where T : ITableEntity
    {
        var list = Enumerable.Empty<T>();
        await foreach (var item in result)
        {
            list = list.Append(item);
        }

        return list;
    }
}
