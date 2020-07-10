using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ODataFilter
{
    class ODataFilterQueryTree
    {
        public ODataFilterGroup Group { get; set; }
        public string Operator { get; set; }
        public ODataFilterQueryTree Right { get; set; }

        public void SetQueryTree(string expr)
        {
            List<String> expressions = GroupExpr(expr);
            ODataFilterQueryTree tree = new ODataFilterQueryTree();

            while(expressions.Count > 0)
            {
                ODataFilterQueryTree newTree = new ODataFilterQueryTree();

                if (tree.Right != null)
                {
                    if (expressions[expressions.Count - 1].Equals("or"))
                    {
                        newTree.Operator = "OR";
                    }
                    else
                    {
                        newTree.Operator = "AND";
                    }

                    expressions.RemoveAt(expressions.Count - 1);
                }

                ODataFilterGroup group = new ODataFilterGroup();
                group.GroupString(expressions[expressions.Count - 1]);

                newTree.Right = tree.Right;
                newTree.Group = group;

                tree.Right = newTree;

                expressions.RemoveAt(expressions.Count - 1);
            }

            Group = tree.Right.Group;
            Operator = tree.Right.Operator;
            Right = tree.Right.Right;
        }

        private List<String> GroupExpr(string expr)
        {
            List<String> expressions = new List<String>();

            int index = expr.IndexOf("$filter=");
            expr = expr.Substring(index + 8);

            while (expr.Length != 0)
            {
                if (expr[0].Equals(' '))
                {
                    expr = expr.Substring(1);
                    int ind = expr.IndexOf(" ");
                    expressions.Add(expr.Substring(0, ind));
                    expr = expr.Substring(ind + 1);
                }

                int firstIndex = expr.IndexOf('(');
                int secondIndex = expr.IndexOf(')');

                if (firstIndex == 0)
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
