using System.Web.UI;

namespace Adf.Web.Extensions
{
    public static class ViewStateExtensions
    {
        public static T GetOrDefault<T>(this StateBag viewstate, string key, T defaultValue = default(T))
        {
            object v = viewstate[key];
            if (v != null) return (T) v;
            if (Equals(defaultValue, default(T))) return defaultValue;

            viewstate[key] = defaultValue;
            return defaultValue;
        }
    }
}
