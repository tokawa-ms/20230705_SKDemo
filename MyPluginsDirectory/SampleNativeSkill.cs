using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;


namespace Plugins;

public class SampleNativeSkill
{
    [SKFunction("引数に取った二つの値を足す")]
    [SKFunctionContextParameter(Name = "input", Description = "一つ目の値")]
    [SKFunctionContextParameter(Name = "number2", Description = "二つ目の値")]
    public string Add(SKContext context)
    {
        Console.WriteLine("Add : " + context["input"] + " + " + context["number2"]);

        return (
            Convert.ToDouble(context["input"]) + Convert.ToDouble(context["number2"])
        ).ToString();
    }
}

public class MathPlugin
{
    [SKFunction("Take the square root of a number")]
    public string Sqrt(string number)
    {
        return Math.Sqrt(Convert.ToDouble(number)).ToString();
    }

    [SKFunction("Add two numbers")]
    [SKFunctionContextParameter(Name = "input", Description = "The first number to add")]
    [SKFunctionContextParameter(Name = "number2", Description = "The second number to add")]
    public string Add(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) + Convert.ToDouble(context["number2"])).ToString();
    }

    [SKFunction("Subtract two numbers")]
    [SKFunctionContextParameter(Name = "input", Description = "The first number to subtract from")]
    [SKFunctionContextParameter(Name = "number2", Description = "The second number to subtract away")]
    public string Subtract(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) - Convert.ToDouble(context["number2"])).ToString();
    }

    [SKFunction("Multiply two numbers. When increasing by a percentage, don't forget to add 1 to the percentage.")]
    [SKFunctionContextParameter(Name = "input", Description = "The first number to multiply")]
    [SKFunctionContextParameter(Name = "number2", Description = "The second number to multiply")]
    public string Multiply(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) * Convert.ToDouble(context["number2"])).ToString();
    }

    [SKFunction("Divide two numbers")]
    [SKFunctionContextParameter(Name = "input", Description = "The first number to divide from")]
    [SKFunctionContextParameter(Name = "number2", Description = "The second number to divide by")]
    public string Divide(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) / Convert.ToDouble(context["number2"])).ToString();
    }
}

/* 
public class MathPlugin
{
    [SKFunction("入力された数字の平方根（ルート）を計算する")]
    public string Sqrt(string number)
    {
        return Math.Sqrt(Convert.ToDouble(number)).ToString();
    }

    [SKFunction("入力された 2 つの値を足し算します。")]
    [SKFunctionContextParameter(Name = "input", Description = "足される元の数となる一つ目の入力")]
    [SKFunctionContextParameter(Name = "number2", Description = "足す数となる二つ目の入力")]
    public string Add(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) + Convert.ToDouble(context["number2"])).ToString();
    }

    [SKFunction("入力された 2 つの値を引き算します。")]
    [SKFunctionContextParameter(Name = "input", Description = "引かれる元の数となる一つ目の入力")]
    [SKFunctionContextParameter(Name = "number2", Description = "引く数となる二つ目の入力")]
    public string Subtract(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) - Convert.ToDouble(context["number2"])).ToString();
    }

    [SKFunction("入力された 2 つの値を掛け算します。掛ける数にパーセント(%)が含まれている場合には、掛ける数に 1 を足した後に、掛け算の計算をすることを忘れないでください。")]
    [SKFunctionContextParameter(Name = "input", Description = "掛けられる数となる一つ目の入力")]
    [SKFunctionContextParameter(Name = "number2", Description = "書ける和となる二つ目の入力")]
    public string Multiply(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) * Convert.ToDouble(context["number2"])).ToString();
    }

    [SKFunction("入力された一つ目の値を二つ目の値で割り算します。")]
    [SKFunctionContextParameter(Name = "input", Description = "割られる数となる一つ目の入力")]
    [SKFunctionContextParameter(Name = "number2", Description = "割る数となる二つ目の入力")]
    public string Divide(SKContext context)
    {
        return (Convert.ToDouble(context["input"]) / Convert.ToDouble(context["number2"])).ToString();
    }
} 
*/
