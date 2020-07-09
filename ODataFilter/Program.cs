using System;
using System.Collections.Generic;

namespace ODataFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> str = 
                GroupExpr("http://host/service/Products?$filter=" +
                "(Price eq 5 or Price eq 6 or Price eq 8) " +
                "and contains(CompanyName,'Alfreds')");

            foreach(String s in str)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }

        static List<String> GroupExpr(string expr)
        {
            List<String> expressions = new ArrayList<String>();

            int index = expr.IndexOf("$filter=");
            expr = expr.Substring(index + 8);

            while(expr.Length != 0)
            {
                if (expr[0].Equals(' '))
                {
                    expr = expr.Substring(1);
                    int ind = expr.IndexOf(" ");
                    expr = expr.Substring(ind + 1);
                }

                int firstIndex = expr.IndexOf('(');
                int secondIndex = expr.IndexOf(')');

                if(firstIndex == 0)
                {
                    expressions.Add(expr.Substring(firstIndex + 1, secondIndex - 1));
                }
                else
                {
                    expressions.Add(expr.Substring(0, secondIndex + 1));
                }

                expr = expr.Substring(secondIndex + 1);
            }

            return expressions;
        }
    }
}
