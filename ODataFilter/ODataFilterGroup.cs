using System;
using System.Collections.Generic;
using System.Text;

namespace ODataFilter
{
    class ODataFilterGroup
    {
        public string[] Filter { get; set; }
        public string Operator { get; set; }

        public void GroupString(string expr)
        {
            if (expr.Contains("or"))
            {
                Filter = expr.Split(" or ");
                Operator = "OR";
            }
            else if(expr.Contains("and"))
            {
                Filter = expr.Split(" and ");
                Operator = "AND";
            }
            else
            {
                Filter = new string[] { expr };
            }
        }
    }
}
