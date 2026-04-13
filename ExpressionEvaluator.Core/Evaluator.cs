namespace ExpressionEvaluator.Core;

using System.Globalization;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var output = new Queue<string>();
        var operators = new Stack<char>();
        var numberBuffer = string.Empty;

        for (int i = 0; i < infix.Length; i++)
        {
            var ch = infix[i];
            if (char.IsWhiteSpace(ch))
                continue;

            if (char.IsDigit(ch) || ch == '.')
            {
                
                numberBuffer += ch;
            }
            else if (IsOperator(ch))
            {
                if (!string.IsNullOrEmpty(numberBuffer))
                {
                    output.Enqueue(numberBuffer);
                    numberBuffer = string.Empty;
                }

                if (ch == '(')
                {
                    operators.Push(ch);
                    continue;
                }

                if (ch == ')')
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                        output.Enqueue(operators.Pop().ToString());

                    if (operators.Count == 0)
                        throw new Exception("Sintax error: mismatched parentheses.");

                    operators.Pop();
                    continue;
                }

            
                while (operators.Count > 0 && PriorityStack(operators.Peek()) >= PriorityInfix(ch))
                {
                    output.Enqueue(operators.Pop().ToString());
                }

                operators.Push(ch);
            }
            else
            {
                // unknown character
                throw new Exception($"Sintax error: invalid character '{ch}'.");
            }
        }

        if (!string.IsNullOrEmpty(numberBuffer))
            output.Enqueue(numberBuffer);

        while (operators.Count > 0)
        {
            var op = operators.Pop();
            if (op == '(' || op == ')')
                throw new Exception("Sintax error: mismatched parentheses.");
            output.Enqueue(op.ToString());
        }

        return string.Join(' ', output);
    }

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(string postfix)
    {
        var stack = new Stack<double>();
        var tokens = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var token in tokens)
        {
            if (token.Length == 1 && IsOperator(token[0]) && token[0] != '(' && token[0] != ')')
            {
                if (stack.Count < 2) throw new Exception("Sintax error");
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(token[0] switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                // parseo
                if (!double.TryParse(token, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                    throw new Exception($"Sintax error: invalid number '{token}'.");
                stack.Push(value);
            }
        }

        if (stack.Count != 1) throw new Exception("Sintax error: invalid expression.");
        return stack.Pop();
    }

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);
}
