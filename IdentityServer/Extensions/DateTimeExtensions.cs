using System.Diagnostics;
namespace IdentityServer
{
    internal static class DateTimeExtensions
    {
        [DebuggerStepThrough]
        public static bool HasExpired(this DateTime expirationTime, DateTime now)
        {
            if (now > expirationTime)
            {
                return true;
            }

            return false;
        }
    }
}
