using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ODataFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            ODataFilterQueryTree tree = new ODataFilterQueryTree();
            tree.SetQueryTree("http://host/service/Products?$filter=" +
                "(Price eq 5 or Price eq 6 or Price eq 8) " +
                "and contains(CompanyName,'Alfreds')");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize<ODataFilterQueryTree>(tree, options);
            
            Console.WriteLine(json);

            Console.ReadKey();
        }
        
    }
}
