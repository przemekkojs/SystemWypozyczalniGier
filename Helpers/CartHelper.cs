using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SystemWypozyczalniGier.Helpers
{
	public static class CartHelper
	{
		private static readonly string CartKeySuffix = "-cart";
        private static string CartKey => UserHelper.LoggedUserEmail.Replace("@", ".at.") + CartKeySuffix;

		public static HashSet<int> GetCart(HttpRequest request)
		{
            var cartCookie = request.Cookies[CartKey];
            return cartCookie == null ? new HashSet<int>() :
                JsonConvert.DeserializeObject<HashSet<int>>(cartCookie) ?? new HashSet<int>();
        }

		public static void SaveCart(HttpResponse response, HashSet<int> cart)
		{
            var serialized = JsonConvert.SerializeObject(cart);

            CookieOptions options = new()
            {
                Expires = DateTime.Now + new TimeSpan(days: 7, hours: 0, minutes: 0, seconds: 0)
            };
            response.Cookies.Append(CartKey, serialized, options);
        }
    }
}

