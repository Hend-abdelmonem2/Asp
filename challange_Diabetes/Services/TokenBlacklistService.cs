using System.Collections.Concurrent;

namespace challange_Diabetes.Services
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklist = new();
        public void BlacklistToken(string token)
        {
            _blacklist[token] = DateTime.UtcNow;
        }

        public bool IsTokenBlacklisted(string token)
        {
            var isBlacklisted = _blacklist.ContainsKey(token);
            return isBlacklisted;

        }
        public void RemoveToken(string token)
        {
            if (_blacklist.TryRemove(token, out _))
            {
                Console.WriteLine($"Token removed from blacklist: {token}");
            }
            else
            {
                Console.WriteLine($"Token not found in blacklist: {token}");
            }
        }
    }
}
