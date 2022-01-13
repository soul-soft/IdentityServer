namespace IdentityServer.Models
{
    public interface IClaimLite
    {
        string Name { get; set; }
        string Value { get; set; }
        string ValueType { get; set; }
    }
}
