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
            int index = expr.IndexOf("$filter=");
            expr = expr.Substring(index + 8);

            expr = CheckExpr(expr);
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
                int secondIndex;

                string newExpr = expr;
                int countBrackets = 0;
                int indexOfChar = firstIndex + 1;
                
                while(true)
                {
                    if(newExpr[indexOfChar].Equals('('))
                    {
                        countBrackets++;
                    }
                    else if (newExpr[indexOfChar].Equals(')') && countBrackets > 0)
                    {
                        countBrackets--;
                    }
                    else if (newExpr[indexOfChar].Equals(')') && countBrackets == 0)
                    {
                        secondIndex = indexOfChar;
                        break;
                    }

                    indexOfChar++;
                }

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

        public string CheckExpr(string expr)
        {
            expr = expr.Trim();
            int index = 0;
            int countBrackets = 0;

            while(index != expr.Length)
            {
                if(expr[index].Equals('('))
                {
                    countBrackets++;
                }
                else if (expr[index].Equals(')'))
                {
                    countBrackets--;
                }

                if (index != (expr.Length - 1) && expr[index].Equals(')')
                    && !expr[index + 1].Equals(' ') && !expr[index + 1].Equals(')'))
                {
                    expr = expr.Substring(0, index + 1) + " " + expr.Substring(index + 1);
                }
                else if (index != 0 && expr[index].Equals('(') && !expr[index - 1].Equals(' ') &&
                    !expr[index - 1].Equals('(') &&
                    (expr.Substring(index - 4, 4).Equals(" and") || expr.Substring(index - 3, 3).Equals(" or")))
                {
                    expr = expr.Substring(0, index) + " " + expr.Substring(index);
                }

                if(index != 0 && expr[index].Equals(' ') && expr[index - 1].Equals(' '))
                {
                    expr = expr.Substring(0, index - 1) + expr.Substring(index);
                    index--;
                }

                index++;
            }

            if (countBrackets != 0)
            {
                throw new Exception("Количество открывающих и закрывающих скобок не совпадает");
            }

            return expr;
        }
    }
}
