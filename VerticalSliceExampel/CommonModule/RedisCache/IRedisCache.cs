﻿namespace VerticalSliceExample.CommonModule.RedisCache;

public interface IRedisCache
{
    Task SetAsync<T>(string key, T value);
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}
