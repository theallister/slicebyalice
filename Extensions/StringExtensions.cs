namespace SliceByAlice.Extensions
{
    public static class StringExtensions
    {
        public static string ToUrlSegment(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Trim().ToLower().Replace(" ", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o");
        }
    }
}