using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DishesPuzzle
{
    class Program
    {
        

        static void Main(string[] args)
        {
            decimal targetPrice;
            Dictionary<string, decimal> dishesMenu;
            bool found;
            List<decimal> combinationDish;
            decimal[] dishesValue;
            Dictionary<string, decimal> result;
            string dishName;

            /***
             *created an infinite loop that will only break if user press "q"
             */
            while (true)
            {
                //initializing all variables at the beging of the infinite loop
                targetPrice = 0.0m;
                dishesMenu = new Dictionary<string, decimal>();
                found = false;
                combinationDish = new List<decimal>();
                dishesValue = null;
                result = new Dictionary<string, decimal>();
                dishName = "";

                Console.WriteLine("");
                Console.WriteLine("Enter the name of the csv file or press \"q \" and then ht enter if you want to exit");
                string optionName = Console.ReadLine();
                if (optionName == "q" || optionName == "Q")
                    return;
                readFile(optionName);
                //array of the values of the dishes
                dishesValue = dishesMenu.Select(z => z.Value).ToArray();
                //first call to recursive method
                recursive(new List<decimal>(), dishesValue);

                //decimal test = dishesMenu.Sum(d=>d.Value);
                //Console.WriteLine(test + " testing sum method " + sumArray(dishesMenu.Values.ToList()));

                //check if found a combination of dishes
                if (found)
                {
                    foreach (var a in combinationDish)
                    {
                        dishName = dishesMenu.FirstOrDefault(d => d.Value == a).Key;
                        result.Add(dishName, a);
                        dishesMenu.Remove(dishName);
                    }
                    Console.WriteLine("For a Target Price = $"+targetPrice+" you can buy ");

                    foreach (var item in result)
                    {
                        Console.WriteLine(item.Key + "    " + item.Value);
                    }
                }
                else {
                    Console.WriteLine("there is no combination of dishes that is equal to the target price.");
                }
                
            }
            /***
             * method to read from a file and assign values to variables
             * 
             */
            void readFile(string fileName)
            {
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader streamReader = new StreamReader(fileName))
                    {
                        // Read the stream to a string
                        String line = streamReader.ReadLine();
                        string[] elemLine = line.Split(',');
                        string value = "";
                        //checking because in the sample data target price have a white space after comma
                        if (elemLine[1][0] == ' ')
                            value = elemLine[1].Substring(2, elemLine[1].Length - 2);
                        else
                            value = elemLine[1].Substring(1, elemLine[1].Length - 1);
                        targetPrice = Decimal.Parse(value);

                        while ( ( line = streamReader.ReadLine() ) != null) {
                            if (line == "")
                                continue;

                            elemLine = line.Split(',');

                            //Console.WriteLine(elemLine[1]);
                            value = elemLine[1].Substring(1, elemLine[1].Length-1);
                            //Console.WriteLine(value);
                            dishesMenu.Add(elemLine[0], decimal.Parse(value));
                            
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an error reading the file, or the format is incorrect");
                    Console.WriteLine(e.Message);
                }


            }
            
            /***
             * 
             * recursive algoritm that has 2 params, the actual combination of dishes to try and the rest of the dishes to combine
             * first i check if i found a solution before, if not then check that combination the sum of all actual dishes 
             * if not then check if the sum is bigger than targetPrice, if not then i loop
             * to try all the rest of the dishes calling the recursive method
             * 
             */
            void recursive(List<decimal> actual, decimal[] dishes)
            {
                if (found)
                    return;
                if (actual.Sum(a=>a) == targetPrice)
                {
                    found = true;
                    combinationDish = actual.ToList();
                }
                if (actual.Sum(a => a) > targetPrice)
                    return;
                
                for (int i = 0; i < dishes.Length; i++)
                {
                    actual.Add(dishes[i]);
                    recursive(actual, dishes.Where((asd, elemt) => elemt != i).ToArray());
                    actual.Remove(dishes[i]);
                }
            }
        }
    }

    



}
