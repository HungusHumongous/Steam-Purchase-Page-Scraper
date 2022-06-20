using HtmlAgilityPack;
using System.Linq;


Console.WriteLine("youll soon find out how much youve spent on steam");


// make sure the program has an easier way to get the directory other than the whole directory of the file. maybe some weird wizardry where it looks in the current folder
static void MyMethod()
{

    HtmlNode pageContent = RetrieveData();
    GetTotal(pageContent);


}

static HtmlNode RetrieveData()
{
    while (true)
    {
        try
        {
            //promt for user to insert file name or path
            Console.WriteLine("Please enter the path for your file\neg: C:\\Users\\Hungus\\Desktop\\Programming\\Projects\\Steam Purchase Scraper\\Hungus's purchases.html\n");
            string userPath = Console.ReadLine();
            //path = @"C:\Users\Hungus\Desktop\Programming\Projects\Steam Purchase Scraper\scraper\nachotoaststeam.html";    // put the path for your program here
            HtmlDocument document = new HtmlDocument();
            document.Load(userPath);
            HtmlNode pageContent = document.DocumentNode.SelectSingleNode("//div[@id='main_content']"); // selects a important, but not the root node.
            return pageContent;
        }
        catch
        {
            Console.WriteLine("\ninvalid file path, please try again.\nMake sure to check if the file is at the end of the path.\n");
        }
    }

}

static void GetTotal(HtmlNode page)
{
    HtmlNodeCollection TrNodes = page.SelectNodes(".//tr[@data-panel=@*]"); // selects all rows of the table
    string nodeText = "";
    string nodeValue;
    double sum = 0.00;
    double refundedSum = 0.00;
    double salesSum = 0.00;
    double walletsum = 0.00;
    int counterDebit = 0;
    int counterCredit = 0;
    int walletCredit = 0;
    int counterRefund = 0;
    foreach (HtmlNode tr in TrNodes) // TrNodes are each row of a table, Tr is the parent node of each row. so each Tr holds one transaction
    {

        try
        {
            nodeText = tr.SelectSingleNode(".//td[@data-tooltip-text=@*]").InnerText;
            nodeValue = tr.SelectSingleNode(".//td[@class='wht_total ']").InnerText;    // selects two table lines, the line where the description for the transaction is and the line where the ammount is
            //Console.WriteLine(nodeValue);

            String dubiousNode = nodeValue.Trim();                                      // basically the easist way to prevent errors in legitimate transactions, just removes all of the junk space we dont want
            dubiousNode = String.Concat(dubiousNode.Where(c => !Char.IsWhiteSpace(c)));

            if (nodeText.Contains("Wallet Credit"))
            {
                walletCredit++;
                nodeValue = nodeValue.Trim().Remove(0, 4).Trim();
                walletsum += Convert.ToDouble(nodeValue);
                continue;
            }
            else if (dubiousNode.Contains("Credit"))
            {

                nodeValue = nodeValue.Trim().Remove(0, 4).Trim().TrimEnd('C', 'r', 'e', 'd', 'i', 't');
                salesSum += Convert.ToDouble(nodeValue);
                counterCredit++;
            }
            else
            {
                //Console.WriteLine(nodeValue);
                nodeValue = nodeValue.Trim().Remove(0, 4).Trim();
                sum += Convert.ToDouble(nodeValue);
                counterDebit++;
                //Console.WriteLine(nodeValue);
            }
        }
        catch
        {
            try
            {
                nodeText = tr.SelectSingleNode("//td[@data-tooltip-text=@*]").InnerText;
                nodeValue = tr.SelectSingleNode(".//td[@class='wht_total wht_refunded']").InnerText;
                nodeValue = nodeValue.Trim().Remove(0, 4).Trim();
                refundedSum += Convert.ToDouble(nodeValue);
                counterRefund++;
                //Console.WriteLine(nodeValue + " refund");
            }
            catch
            {
                Console.WriteLine("An error occured by an unusual transaction");
                continue;
            }
        }
        //Console.WriteLine("loop");
    }
    refundedSum = Math.Round(refundedSum, 2);
    salesSum = Math.Round(salesSum, 2);
    sum = Math.Round(sum, 2);
    Console.WriteLine(String.Format(
        "\nThere are {0} debting transactions" +
        "\nyou have credited your wallet {1} times" +
        "\nyouve made {2} marketplace Sales and youve refunded {3} games",
        counterDebit, walletCredit, counterCredit, counterRefund));

    Console.WriteLine(string.Format(
        "\nYour total refund ammount is: ${0}" +
        "\nyour total market Sales is: ${1}" +
        "\nyour total purchases is: ${2}" +
        "\nyour total wallet credit purchases ${3}",
        refundedSum, salesSum, sum, walletsum));

    double total = Math.Round(sum-refundedSum-salesSum, 2);
    if (total <500)
    {
        Console.WriteLine(String.Format("\nThe total ammount of money youve spent on steam is ${0}", total));
    }
    else if (total < 2000)
    {
        Console.WriteLine(String.Format("\nThe total ammount of money youve wasted on steam is ${0}", total));
    }
    else
    {
        Console.WriteLine(String.Format("\nThe total ammount of money youve wasted on steam is ${0}. I hope it was worth it.\n", total));
    }


    return;                                         //this program took me faar tooo long to make, let me know if you manage to catch an error

}




MyMethod();






