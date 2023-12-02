using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Json;
using test_intern;
using static System.Net.Mime.MediaTypeNames;



public class Program
{
    private static void Main(string[] args)
    {
        string checkIn = "2019-12-25";
        string input = "C:\\Users\\GF\\Documents\\Nam_4\\test_intern\\test_intern\\input.json";
        string ouput = "C:\\Users\\GF\\Documents\\Nam_4\\test_intern\\test_intern\\output.json";
        //read and convert file json from path input to object
        OffersData offersData = new OffersData();
        offersData = ReadInput(input);

        //filter

        //convert string checkIn to date time and add 5 days
        DateTime checkInDate = DateTime.Parse(checkIn).AddDays(5);

        Console.WriteLine(checkInDate.ToString());

        List<Offer> result = new List<Offer>();

        foreach (Offer offers in offersData.Offers)
        {

            if (offers.ValidTo >= checkInDate && (offers.Category == 1 || offers.Category == 2 || offers.Category == 4))
            {
                Console.WriteLine(offers.ValidTo.ToString());

                List<Merchant> merchantList = new List<Merchant>();
                if (offers.Merchants.Count == 1)
                {
                    merchantList.Add(offers.Merchants[0]);

                }
                else
                {
                    offers.Merchants.Sort();
                    merchantList.Add(offers.Merchants[0]);
                }
                Offer offer = new Offer()
                {
                    Id = offers.Id,
                    Title = offers.Title,
                    Description = offers.Description,
                    Category = offers.Category,
                    Merchants = merchantList,
                    ValidTo = offers.ValidTo,
                };
                result.Add(offer);
            }
        }
        result.Sort((offer1, offer2) =>
            offer1.Merchants.First().Distance.CompareTo(offer2.Merchants.First().Distance));
        Dictionary<int, bool> myDictionary = new Dictionary<int, bool>();
        List<Offer> resultAll = new List<Offer>();
        int i = 0;
        foreach (Offer offer in result)
        {
            if (!myDictionary.ContainsKey(offer.Category))
            {
                Console.WriteLine("offer: " + offer.Id);
                Console.WriteLine("Title: " + offer.Title);
                Console.WriteLine("Description: " + offer.Description);
                Console.WriteLine("Category: " + offer.Category);
                Console.WriteLine("merchants: ");
                foreach (Merchant item in offer.Merchants)
                {
                    Console.WriteLine("     id: " + item.Id);
                    Console.WriteLine("     Name: " + item.Name);
                    Console.WriteLine("     Distance: " + item.Distance);
                }
                Console.WriteLine("offer.ValidTo: " + offer.ValidTo.ToString());
                Console.WriteLine("...........................................");
                myDictionary[offer.Category] = true;
                resultAll.Add(offer);
            }
            i++;
            if (i == 2) {
                break;
            }
        }
        SaveData(resultAll, ouput);


    }

    public static OffersData ReadInput(string path)
    {
        OffersData result = new OffersData();
        string input = File.ReadAllText(path);
        JObject jsonObject = JObject.Parse(input);

        // get array offers
        JArray offersArray = (JArray)jsonObject["offers"];

        List<Offer> offers = new List<Offer>();

        // loop item in offers
        foreach (JObject offerObject in offersArray)
        {
            Offer offer = new Offer
            {
                Id = (int)offerObject["id"],
                Title = (string)offerObject["title"],
                Description = (string)offerObject["description"],
                Category = (int)offerObject["category"],
                ValidTo = (DateTime)offerObject["valid_to"],
                Merchants = new List<Merchant>()
            };

            // get arrray merchants
            JArray merchantsArray = (JArray)offerObject["merchants"];

            foreach (JObject merchantObject in merchantsArray)
            {
                Merchant merchant = new Merchant
                {
                    Id = (int)merchantObject["id"],
                    Name = (string)merchantObject["name"],
                    Distance = (double)merchantObject["distance"]
                };

                offer.Merchants.Add(merchant);
            }

            offers.Add(offer);
        }
        result.Offers = offers;
        return result;
    }

    static void SaveData(List<Offer> filteredOffers, string path)
    {
        OffersData filteredData = new OffersData { Offers = filteredOffers };
        string jsonOutput = JsonConvert.SerializeObject(filteredData, new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd",
            Formatting = Formatting.Indented
        });
        File.WriteAllText(path, jsonOutput);
    }

}