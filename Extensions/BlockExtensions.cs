using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace SliceByAlice
{
    public static class BlockExtensions
    {
        private const string DEFAULT_HEADING_SIZE = "heading--2";
        private const string DEFAULT_TEXT_SIZE = "text--1";
        private const string DEFAULT_HEADING_COLOR_CLASS = "color--blue";
        private const string DEFAULT_TEXT_COLOR_CLASS = "color--blue";
        public static string GetHeadingSize(this IPublishedElement block, string defaultValue = DEFAULT_HEADING_SIZE)
        {
            if (block == null) return defaultValue;

            switch (block.Value<string>("headingSize"))
            {
                case "H1":
                    return "heading--1";
                case "H2":
                    return "heading--2";
                case "H3":
                    return "heading--3";
                case "H4":
                    return "heading--4";
                default:
                    return defaultValue;
            }
        }   
        public static string GetHeadingColor(this IPublishedElement block, string defaultValue = DEFAULT_HEADING_COLOR_CLASS)
        {
            if (block == null) return defaultValue;

            return block.Value<string>("headingColor") ?? defaultValue;
        }
        public static string GetHeadingColorClass(this IPublishedElement block, string defaultValue = DEFAULT_HEADING_COLOR_CLASS)
        {
            if (block == null) return defaultValue;
            switch (block.Value<string>("headingColor")?.ToLower())
            {
                case "#f4f1de":
                    return "color--white";
                case "#e07a5f":
                    return "color--red";
                case "#81b29a":
                    return "color--green";
                case "#f2cc8f":
                    return "color--yellow";
                case "#3d405b":
                    return "color--blue";
                default:
                    return defaultValue;
            }
        }
        public static string GetHeadingStyle(this IPublishedElement block, string defaultValue = "")
        {
            if (block == null || string.IsNullOrWhiteSpace(block.Value<string>("headingStyle"))) return defaultValue;

            return "text--" + block.Value<string>("headingStyle")?.ToLower() ?? defaultValue;
        }
        public static string GetTextSize(this IPublishedElement block, string defaultValue = DEFAULT_TEXT_SIZE)
        {
            if (block == null) return defaultValue;

            switch (block.Value<string>("textSize"))
            {
                case "0.875rem":
                    return "text--0875";
                case "1rem":
                    return "text--1";
                case "1.125rem":
                    return "text--1125";
                case "1.25rem":
                    return "text--125";
                default:
                    return defaultValue;
            }
        }
        public static string GetTextColor(this IPublishedElement block, string defaultValue = DEFAULT_TEXT_COLOR_CLASS)
        {
            if (block == null) return defaultValue;

            return block.Value<string>("textColor") ?? defaultValue;
        }
        public static string GetTextColorClass(this IPublishedElement block, string defaultValue = DEFAULT_TEXT_COLOR_CLASS)
        {
            if (block == null) return defaultValue;
            switch (block.Value<string>("textColor")?.ToLower())
            {
                case "#f4f1de":
                    return "color--white";
                case "#e07a5f":
                    return "color--red";
                case "#81b29a":
                    return "color--green";
                case "#f2cc8f":
                    return "color--yellow";
                case "#3d405b":
                    return "color--blue";
                default:
                    return defaultValue;
            }
        }
        public static string GetTextStyle(this IPublishedElement block, string defaultValue = "text--standard")
        {
            if (block == null) return defaultValue;

            return "text--" + block.Value<string>("textStyle")?.ToLower() ?? defaultValue;
        }
        public static string GetDropShadowClass(this IPublishedElement block, string defaultValue = "")
        {
            if (block == null) return defaultValue;

            switch (block.Value<string>("dropShadow")?.ToLower())
            {
                case "ingen drop shadow":
                    return "";
                case "small":
                    return "drop-shadow--small";
                case "medium":
                    return "drop-shadow--medium";
                case "large":
                    return "drop-shadow--large";
                default:
                    return defaultValue;
            }
        }
        public static string GetSvgPathClass(this IPublishedElement block, string colorClass)
        {
            switch (colorClass)
            {
                case "color--white":
                case "heading--white":
                    return "stroke--white";
                case "color--red":
                case "heading--red":
                    return "stroke--red";
                case "color--green":
                case "heading--green":
                    return "stroke--green";
                case "color--yellow":
                case "heading--yellow":
                    return "stroke--yellow";
                case "color--blue":
                case "heading--blue":
                default:
                    return "stroke--blue";
            }
        }
        public static string GetBackgroundColorClass(this IPublishedElement block, string defaultValue = "")
        {
            if (block == null) return defaultValue;

            if (defaultValue.StartsWith("color--"))
                return "background--" + defaultValue.Substring("color--".Length);
            if (defaultValue.StartsWith("heading--"))
                return "background--" + defaultValue.Substring("heading--".Length);
            if (defaultValue.StartsWith("background--"))
                return defaultValue;

            switch (block.Value<string>("backgroundColor")?.ToLower())
            {
                case "#f4f1de":
                case "color--white":
                    return "background--white";
                case "#e07a5f":
                case "color--red":
                    return "background--red";
                case "#81b29a":
                case "color--green":
                    return "background--green";
                case "#f2cc8f":
                case "color--yellow":
                    return "background--yellow";
                case "#3d405b":
                case "color--blue":
                    return "background--blue";
                case "#ffffff":
                    return "background--complete-white";
                default:
                    return defaultValue;
            }
        }
        public static string GetBorderColorClass(this IPublishedElement block, string colorKey)
        {
            if (block == null) return "";

            switch (colorKey?.ToLower())
            {
                case "white":
                case "text--white":
                case "heading--white":
                    return "border--white";
                case "text--red":
                case "heading--red":
                case "red":
                    return "border--red";
                case "text--green":
                case "heading--green":
                case "green":
                    return "border--green";
                case "text--yellow":
                case "heading--yellow":
                case "yellow":
                    return "border--yellow";
                case "text--blue":
                case "heading--blue":
                case "blue":
                    return "border--blue";
                default:
                    return "";
            }
        }
        public static string GetOutlineColorClass(this IPublishedElement block, string colorKey)
        {
            if (block == null) return "";

            switch (colorKey?.ToLower())
            {
                case "white":
                case "color--white":
                case "heading--white":
                    return "outline--white";
                case "complete-white":
                case "color--complete-white":
                case "heading--complete-white":
                    return "outline--complete-white";
                case "color--red":
                case "heading--red":
                case "red":
                    return "outline--red";
                case "color--green":
                case "heading--green":
                case "green":
                    return "outline--green";
                case "color--yellow":
                case "heading--yellow":
                case "yellow":
                    return "outline--yellow";
                case "color--blue":
                case "heading--blue":
                case "blue":
                    return "outline--blue";
                default:
                    return "";
            }
        }
    }
}