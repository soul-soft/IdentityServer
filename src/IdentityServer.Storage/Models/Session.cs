namespace IdentityServer.Models
{
    public class Session
    {
        public SessionState State { get; set; }
    }

    public enum SessionState
    {
        Inactive,
    }
}
