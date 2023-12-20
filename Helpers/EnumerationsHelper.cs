using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using SystemWypozyczalniGier.Enumerations;

namespace SystemWypozyczalniGier.Helpers
{
	public static class EnumerationsHelper
	{
        public static List<SelectListItem> AccessibilitiesSelectItems { get; private set; }
        public static List<SelectListItem> CategoriesSelectItems { get; private set; }
        public static List<SelectListItem> PegiSelectItems { get; private set; }
        public static List<SelectListItem> RentalsStatusSelectItems { get; private set; }
        public static List<SelectListItem> RolesSelectItems { get; private set; }

        static EnumerationsHelper()
        {
            AccessibilitiesSelectItems = GetSelectItems<Accessibility>();
            CategoriesSelectItems = GetSelectItems<Category>();
            PegiSelectItems = GetSelectItems<Pegi>();
            RentalsStatusSelectItems = GetSelectItems<RentalStatus>();
            RolesSelectItems = GetSelectItems<Role>();
        }

        private static List<SelectListItem> GetSelectItems<T>() => Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new SelectListItem
            {
                Value = e!.ToString(),
                Text = Enum.GetName(typeof(T), e!)
            })
            .ToList();
    }
}

