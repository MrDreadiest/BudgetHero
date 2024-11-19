namespace BudgetHero.App.Models.Icon
{
    public static class IconHelper
    {
        public static Dictionary<string, List<(string Unicode, string Name)>> GetBudgetRelatedIcons()
        {
            return new Dictionary<string, List<(string Unicode, string Name)>>
            {
                ["Expenses and Income"] = new List<(string Unicode, string Name)>
                {
                    ("\ue2eb", "Savings"),
                    ("\uef63", "Payments"),
                    ("\ueb70", "Currency Exchange"),
                    ("\uebc5", "Bitcoin"),
                    ("\ue573", "Atm"),
                    ("\ue8f9", "Work"),
                    ("\ue4fc", "Stocks"),
                    ("\ue88a", "Home"),
                    ("\uea40", "Apartment"),
                    ("\ue064", "Subscriptions"),
                    ("\uea87", "Pharmacy "),
                    ("\ue73a", "Rental"),
                    ("\ue80c", "School "),
                    ("\ue548", "Hospital "),
                    ("\uef6e", "Receipt  "),
                    ("\ue91d", "Pets  "),
                },
                ["Events"] = new List<(string Unicode, string Name)>
                {
                    ("\uea23", "Events"),
                    ("\uea65", "Celebration"),
                    ("\ue540", "Local Bar"),
                    ("\uf1f3", "Pub"),
                    ("\uea62", "Nightlife"), 
                    //("\uef91", "Gifts"), // POPRAWIĆ BŁĘDY KOD
                    ("\uebcc", "Calendar"),
                    ("\ue53f", "Local Activity"),
                    ("\ueb90", "Stadium"),
                    ("\uea68", "Festival"),
                    ("\uf8d7", "Friends")
                },
                ["Shopping"] = new List<(string Unicode, string Name)>
                {
                    ("\ue8cc", "Shopping Cart"),
                    ("\ue870", "Card"),
                    ("\ue8d1", "Store"),
                    ("\ue8cb", "Shopping Basket"),
                    ("\uf19e", "Checkroom"),
                    ("\ue54a", "Laundry "),
                    ("\uf6da", "Shoes"),
                    ("\uea60", "Liquor"),
                    ("\ue54c", "Mall"),
                    ("\ue545", "Flowers"),
                    ("\uefed", "Chair"),
                    ("\ue9b4", "Imagesearch Roller"),
                    ("\uea3c", "Construction"),
                },
                ["Food and Restaurants"] = new List<(string Unicode, string Name)>
                {
                    ("\ue56c", "Restaurant"),
                    ("\uea61", "Lunch"),
                    ("\ue7e9", "Cake"),
                    ("\ueaac", "Cookie"),
                    ("\ue541", "Coffee"),
                    ("\uea1b", "Tea"),
                    ("\ue552", "Pizza"),
                    ("\uea53", "Bakery"),
                    ("\uea64", "Asian"),
                    ("\ue57a", "Fast food"),
                    ("\ueb47", "Kitchen"),
                    //("\uef97", "Grocery"), // POPRAWIĆ BŁĘDY KOD
                    ("\uea69", "Ice Cream"),
                    //("\ue110", "Nutrition"),// POPRAWIĆ BŁĘDY KOD
                    ("\uea74", "TakeOut"),
                },
                ["Transport"] = new List<(string Unicode, string Name)>
                {
                    ("\ue531", "Car"),
                    ("\ue539", "Flight"),
                    ("\ue546", "Gas Station"),
                    ("\ue559", "Taxi"),
                    ("\ue570", "Train"),
                    ("\ue530", "Bus"),
                    ("\ue532", "Boat"),
                    ("\ue56d", "Ev Station"),
                    ("\ue54f", "Parking"),
                    ("\ueb1f", "Scooter"),
                    //("\uf474", "Metro"), // POPRAWIĆ BŁĘDY KOD
                },
                ["Entertainment"] = new List<(string Unicode, string Name)>
                {
                    ("\ue02c", "Movie"),
                    ("\uea28", "Gaming"),
                    ("\uf49b", "Poker"),
                    ("\uea66", "Theater"),
                    ("\uea52", "Attractions"),
                    ("\ue02f", "Books"),
                    ("\ue030", "Music"),
                    ("\uea36", "Museum"),
                    ("\ue30c", "TV"),
                    ("\ueaae", "Church"),
                },
                ["Gym"] = new List<(string Unicode, string Name)>
                {
                    ("\ueb43", "Gym"),
                    //("\uf6e6", "Exercise"), // POPRAWIĆ BŁĘDY KOD
                    ("\ue566", "Running"),
                    ("\uebc4", "Yoga"),
                    ("\uea2f", "Soccer"),
                    ("\ue52f", "Bike"),
                    ("\ueb48", "Pool"),
                    ("\uea32", "Tennis")
                }
            };
        }
    }
}
