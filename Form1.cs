using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

static double Evaluate(string expression)
{
    var operands = new Stack<double>();
    var operators = new Stack<char>();

    for (int i = 0; i < expression.Length; i++)
    {
        if (char.IsDigit(expression[i]))
        {
            double operand = 0;
            while (i < expression.Length && char.IsDigit(expression[i]))
            {
                operand = (operand * 10) + (expression[i] - '0');
                i++;
            }
            operands.Push(operand);
            i--;
        }
        else if (expression[i] == '(')
        {
            operators.Push(expression[i]);
        }
        else if (expression[i] == ')')
        {
            while (operators.Peek() != '(')
            {
                double result = ApplyOperator(operators.Pop(), operands.Pop(), operands.Pop());
                operands.Push(result);
            }
            operators.Pop();
        }
        else if (expression[i] == '*' || expression[i] == '/')
        {
            while (operators.Count > 0 && (operators.Peek() == '*' || operators.Peek() == '/'))
            {
                double result = ApplyOperator(operators.Pop(), operands.Pop(), operands.Pop());
                operands.Push(result);
            }
            operators.Push(expression[i]);
        }
        else if (expression[i] == '+' || expression[i] == '-')
        {
            while (operators.Count > 0 && operators.Peek() != '(')
            {
                double result = ApplyOperator(operators.Pop(), operands.Pop(), operands.Pop());
                operands.Push(result);
            }
            operators.Push(expression[i]);
        }
    }

    while (operators.Count > 0)
    {
        double result = ApplyOperator(operators.Pop(), operands.Pop(), operands.Pop());
        operands.Push(result);
    }

    return operands.Pop();
}

static double ApplyOperator(char op, double b, double a)
{
    switch (op)
    {
        case '+': return a + b;
        case '-': return a - b;
        case '*': return a * b;
        case '/': return a / b;
        default: throw new ArgumentException("Invalid operator: " + op);
    }
}

string inputFilePath = args[0];
string outputFilePath = Path.ChangeExtension(inputFilePath, ".res");

string expression = File.ReadAllText(inputFilePath);
double result = Evaluate(expression);

File.WriteAllText(outputFilePath, result.ToString());
}
}
