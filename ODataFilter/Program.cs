using System;

namespace ODataFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            ODataFilterGroup group = new ODataFilterGroup();

            group.GroupString("Price eq 5 or Price eq 6 or Price eq 8");

            Console.WriteLine("Filter:");
            foreach(string filter in group.Filter)
            {
                Console.WriteLine("\t" + filter);
            }
            Console.WriteLine("Operator: " + group.Operator);

            ODataFilterGroup group2 = new ODataFilterGroup();

            Console.ReadKey();
        }
    }
}
