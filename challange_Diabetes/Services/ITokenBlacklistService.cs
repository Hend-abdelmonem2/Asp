namespace challange_Diabetes.Services
{
    public interface ITokenBlacklistService
    {
        void BlacklistToken(string token);
        bool IsTokenBlacklisted(string token);
        void RemoveToken(string token);
    }
}
