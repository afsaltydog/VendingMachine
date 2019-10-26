using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virtVendingMachine
{
    public class Inventory
    {
        enum Row { A, B, C, D, E, F };

        public Dictionary<string, Item> items = new Dictionary<string, Item>();

        public void AddItem(Item item, int Row, int Col)
        {
            if (!items.ContainsKey(item.Selector))
                items.Add(item.Selector, item);
            else
                items[item.Selector] = item;
        }

        public string GetItem(string selector, double balance)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                //get price and display price
                Item item = items[selector];
                
                if (item.Count < 1)
                {
                    message.Append(string.Format("Your selection {0} {1} is out of stock. Please select another item", item.Selector, item.Name));
                }
                else if (item.Count > 0)
                {
                    if (balance > item.Price)
                    {
                        item.Count -= 1;
                        message.Append(string.Format("Coin Return: ${0}. ", (balance - item.Price).ToString()));
                        message.Append(string.Format("Your selection, {0} is ready below. Please retrieve your change", item.Name));
                        items[selector] = item;
                    }
                    else if (balance == item.Price)
                    {
                        item.Count -= 1;
                        message.Append(string.Format("Your selection, {0} is ready below.", item.Name));
                        items[selector] = item;
                    }
                    else
                    {
                        message.Append(string.Format("You need ${0} more to make this selection", (item.Price - balance).ToString()));
                    }
                }
                else
                {
                    message.Append(string.Format("{0} is an invalid selection", selector));
                }
                return message.ToString();
            }
            catch (Exception x)
            {
                message.Append(string.Format("{0} is an invalid selecion", selector));
                return message.ToString();
            }
        }

        public string GetAvailableItems()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Item item in items.Values)
            {
                sb.Append(item.Name);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 1);
            return sb.ToString();
        }

        public string GetPriceList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Item item in items.Values)
            {
                sb.Append(string.Format("{0}: ${1}", item.Name, item.Price));
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 1);
            return sb.ToString();
        }

        public string GetQuantities()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Item item in items.Values)
            {
                sb.Append(string.Format("{0}: {1} in stock", item.Name, item.Count));
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 1);
            return sb.ToString();
        }

        public void FillInventory()
        {
            Int16 count = 10;
            string type = "";
            double price = 0;
            for (int row = 0; row < 6; row++)
            {
                List<string> names = new List<string>();
                switch (row)
                {
                    case 0:
                        type = "chips";
                        price = 0.85;
                        names.Add("Cheetos");
                        names.Add("Lays Potato");
                        names.Add("Lays Barbeque");
                        names.Add("Funyuns");
                        names.Add("Doritoes");
                        names.Add("Sun Chips");
                        break;
                    case 1:
                        type = "chips";
                        price = 0.85;
                        names.Add("Tostitos");
                        names.Add("Salt n Vinegar");
                        names.Add("Doritos Spicy");
                        names.Add("Cheese Puffs");
                        names.Add("Fritos");
                        names.Add("Fritos Chili");
                        break;
                    case 2:
                        type = "candy";
                        price = 1.00;
                        names.Add("Snickers");
                        names.Add("M&Ms");
                        names.Add("M&Ms Peanut");
                        names.Add("Butterfinger");
                        names.Add("Twizzlers");
                        names.Add("Candy Corn");
                        break;
                    case 3:
                        type = "candy";
                        price = 1.00;
                        names.Add("Hersheys Plain");
                        names.Add("Hersheys Almonds");
                        names.Add("Mr. Goodbar");
                        names.Add("3 Musketeers");
                        names.Add("Almond Joy");
                        names.Add("Mints");
                        break;
                    case 4:
                        type = "misc";
                        price = 0.75;
                        names.Add("Popcorn");
                        names.Add("Oreos");
                        names.Add("Pretzels");
                        names.Add("Beef Jerky");
                        names.Add("Nutter Butter");
                        names.Add("Oreos Chocolate");
                        break;
                    case 5:
                        type = "misc";
                        price = 0.75;
                        names.Add("Cheesy Popcorn");
                        names.Add("Chips Ahoy");
                        names.Add("Pretzels");
                        names.Add("Reeces Pieces");
                        names.Add("Famous Amos");
                        names.Add("Vanilla Wafers");
                        break;
                    case 6:
                        type = "misc";
                        price = 0.75;
                        names.Add("Spearmint Gum");
                        names.Add("Cinnamon Gum");
                        names.Add("Lifesavers");
                        names.Add("Mints");
                        names.Add("Skittles");
                        names.Add("Animal Crackers");
                        break;
                }

                for (int col = 0; col < 6; col++)
                {
                    Item item = new Item();
                    item.Name = names[col];
                    item.Price = price;
                    item.Count = count;
                    item.Type = type;
                    switch (row)
                    {
                        case 0:
                            item.Selector = string.Format("A{0}", col+1);
                            break;
                        case 1:
                            item.Selector = string.Format("B{0}", col+1);
                            break;
                        case 2:
                            item.Selector = string.Format("C{0}", col+1);
                            break;
                        case 3:
                            item.Selector = string.Format("D{0}", col+1);
                            break;
                        case 4:
                            item.Selector = string.Format("E{0}", col+1);
                            break;
                        case 5:
                            item.Selector = string.Format("F{0}", col+1);
                            break;
                    }
                    AddItem(item, row, col);
                }
            }
        }
    }
}
