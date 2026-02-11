using System;
using System.Collections.Concurrent;

namespace yotelollevo.Infrastructure
{
    public static class LoginThrottle
    {
        private static readonly ConcurrentDictionary<string, LoginAttempt> _attempts
            = new ConcurrentDictionary<string, LoginAttempt>(StringComparer.OrdinalIgnoreCase);

        private const int MaxAttempts = 5;
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(15);

        public static bool IsThrottled(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) return false;

            LoginAttempt attempt;
            if (!_attempts.TryGetValue(login, out attempt)) return false;

            if (DateTime.UtcNow - attempt.WindowStart > Window)
            {
                LoginAttempt removed;
                _attempts.TryRemove(login, out removed);
                return false;
            }

            return attempt.Count >= MaxAttempts;
        }

        public static void RecordFailure(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) return;

            _attempts.AddOrUpdate(login,
                new LoginAttempt { WindowStart = DateTime.UtcNow, Count = 1 },
                (key, existing) =>
                {
                    if (DateTime.UtcNow - existing.WindowStart > Window)
                        return new LoginAttempt { WindowStart = DateTime.UtcNow, Count = 1 };

                    existing.Count++;
                    return existing;
                });
        }

        public static void Reset(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) return;
            LoginAttempt removed;
            _attempts.TryRemove(login, out removed);
        }

        private class LoginAttempt
        {
            public DateTime WindowStart { get; set; }
            public int Count { get; set; }
        }
    }
}
