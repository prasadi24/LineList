using Microsoft.Extensions.Caching.Memory;

public class SessionTracker
{
	private readonly IMemoryCache _cache;
	private readonly object _lock = new object();

	public SessionTracker(IMemoryCache cache)
	{
		_cache = cache;
	}

	public int GetActiveSessionCount()
	{
		_cache.TryGetValue("ActiveSessions", out HashSet<string> activeSessions);
		return activeSessions?.Count ?? 0;
	}

	public void AddSession(string sessionId)
	{
		lock (_lock)
		{
			if (!_cache.TryGetValue("ActiveSessions", out HashSet<string> activeSessions))
			{
				activeSessions = new HashSet<string>();
			}

			activeSessions.Add(sessionId);
			_cache.Set("ActiveSessions", activeSessions);
		}
	}

	public void RemoveSession(string sessionId)
	{
		lock (_lock)
		{
			if (_cache.TryGetValue("ActiveSessions", out HashSet<string> activeSessions))
			{
				activeSessions.Remove(sessionId);
				_cache.Set("ActiveSessions", activeSessions);
			}
		}
	}
}