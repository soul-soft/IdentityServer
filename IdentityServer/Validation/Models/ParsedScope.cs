namespace IdentityServer.Validation
{
    public class ParsedScope
    {
        public string ParsedName { get; }
        public string RawValue { get; }

        public ParsedScope(string parsedName, string rawValue)
        {
            ParsedName = parsedName;
            RawValue = rawValue;
        }
    }
}
