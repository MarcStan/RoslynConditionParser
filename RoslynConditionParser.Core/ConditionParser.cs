using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoslynConditionParser.Core
{
    public class ConditionParser
    {
        public async Task<Result> EvaluateConditionAsync(string condition, Sensor[] sensors)
        {
            var options = GetDefaultOptions();

            var globals = new Globals
            {
                Sensors = sensors.ToList()
            };
            try
            {
                // ' is just more pleasant in json than \"
                // but it's not valid for C# strings, so replace before evaluating
                condition = condition.Replace("'", "\"");

                var expression = MakeGlobalsAccessible(condition);

                bool result = await CSharpScript.EvaluateAsync<bool>(expression, options, globals, null, CancellationToken.None);
                return Result.Success(result);
            }
            catch (CompilationErrorException e)
            {
                return Result.Failure(e.Message);
            }
        }

        private ScriptOptions GetDefaultOptions()
        {
            return ScriptOptions.Default
                   // need access to the assembly to use functions
                   .WithReferences(typeof(ScriptFunctions).Assembly)
                   .WithImports(
                   // import all classes from this namespace to be used directly as part of script engine
                   // needed to create the inline globals instance below
                   typeof(ScriptFunctions).Namespace,
                   // allow access to all static methods without prefix
                   typeof(ScriptFunctions).FullName);
        }

        private string MakeGlobalsAccessible(string condition)
        {
            // roslyn "globals" aren't accessible from normal C# as it takes all (instance) properties of a class and makes
            // them statically available at the topmost level

            // workaround to make them accessible in statically typed C#: assign the global to a static instance
            // to do so, a bit of code needs to be added

            // the line below is not safe for refactoring, but easily readable, could use nameof(..) for all class/property names to make it refactoring safe

            return "ScriptFunctions.Globals = new Globals { Sensors = Sensors };" + Environment.NewLine +
                       // make sure the original condition still works by wrapping it in "return ..;"
                       $"return {condition};";
        }
    }
}
