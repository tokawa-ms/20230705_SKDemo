namespace SK_Demo_Template;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Skills.Web;
using Microsoft.SemanticKernel.Skills.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Orchestration;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.SemanticKernel.Planning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

using Plugins;

class Program
{
    static void Main(string[] args)
    {
        //C# 7.0 までは Console App の Main で Async は使えなかったので、とりあえず　Async が必要なやつは全部 MainAsync に書いて、Main から呼び出すようにしている。
        Task task = MainAsync();
        task.Wait();
    }

    private static async Task MainAsync()
    {
        //VSCode の Secret Store から AOAI にアクセスするための情報を取得する
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var key = builder["APIKey"];
        var endpoint = builder["Endpoint"];
        var model = builder["DeployName"];

        // Semantic Kernel を、コンソールロガーを有効化したうえで初期化
        var kernel = Kernel.Builder
        .WithLogger(LoggerFactory.Create(b =>
        {
            b.AddFilter(_ => true);
            b.AddConsole();
        }).CreateLogger<Program>())
        .Build();

        // Azure OpenAI Service の Chat Completion Service を Kernel に登録する
        kernel.Config.AddAzureChatCompletionService(
            model,
            endpoint,
            key
        );

        // Semantic Function を主導で呼んでみる
        await Example1(kernel);

        // 手動で Function Chain を試してみる
        await Example2(kernel);

        // planner を使って自動で Function Chain を使ってみる。
        await Example3(kernel);

        //Console.WriteLine("実行終了するには何かキーを押してください。");
        //Console.ReadLine();
    }

    private static async Task Example3(IKernel kernel)
    {
        // 四則演算の出来る Native Function を MathPlugin として読み込む
        var mathPlugin = kernel.ImportSkill(new MathPlugin(), "MathPlugin");

        // Planner を初期化
        var planner = new SequentialPlanner(kernel);

        // Planner にプランを立てさせるための要求が ask に入っている
        var ask = "Aさんは500円の元手を競馬で1.5倍にした後、嬉しかったので200円のチョコを買ったあとに150円のジュースも買いました。Aさんの現在の所持金は何円ですか？";
        var plan = await planner.CreatePlanAsync(ask);

        //plan に入っているプランをすべてループで出力
        foreach (var step in plan.Steps)
        {
            Console.WriteLine(step.Name);
        }

        // プランを実行してその結果を Result_2 に入れる
        var result_2 = await plan.InvokeAsync();

        Console.WriteLine("Plan results:");
        Console.WriteLine(result_2.Result);
    }

    private static async Task Example2(IKernel kernel)
    {
        // Semantic Function をディレクトリから読み込まずに C# のコード内で定義しちゃうことも出来ます。
        string myJokePrompt = """
            {{$INPUT}} についての短いジョークを書いてください。
            """;
        string myPoemPrompt = """
            “{{$INPUT}}” を題材にして、短い詩を書いてください。
            """;
        string myMenuPrompt = """
            “{{$INPUT}}” の詩に着想を得て、喫茶店の三つのメニューを考えてください。
            メニューは、以下に列挙してください : 
            """;

        // 上で定義した Semantic Function のプロンプトをそれぞれ Semantic Function として kernel に登録して使えるようにする。
        var myJokeFunction = kernel.CreateSemanticFunction(myJokePrompt, maxTokens: 500);
        var myPoemFunction = kernel.CreateSemanticFunction(myPoemPrompt, maxTokens: 500);
        var myMenuFunction = kernel.CreateSemanticFunction(myMenuPrompt, maxTokens: 500);

        // 上で定義した Semantic Function を 3 つチェーンして実行する
        var myOutput = await kernel.RunAsync(
            "Microsoft Azure",
            myJokeFunction,
            myPoemFunction,
            myMenuFunction
        );
        Console.WriteLine(myOutput);
    }

    private static async Task Example1(IKernel kernel)
    {
        // MyPlugionsDirectory ディレクトリ配下の FunSkill プラグインを読み込み、
        // その中の Joke という Function を Kernel に登録して kernel から使えるようにする。
        var funSkillFunctions = kernel.ImportSemanticSkillFromDirectory("MyPluginsDirectory", "FunSkill");
        var jokeFunction = funSkillFunctions["Joke"];

        // Joke という Function にコンテキストを渡して実行
        var result = await jokeFunction.InvokeAsync("恐竜時代にタイムトラベルをする。");
        Console.WriteLine("Result : ");
        Console.WriteLine(result);
    }
}
