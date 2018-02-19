using Jint;
using MongoDB.Bson;
using System.Dynamic;
using Jint.Runtime.Interop;
using System;
using BlendedJS.Http;
using BlendedJS.Mongo;
using BlendedJS.Sql;
using Jint.Runtime.Debugger;

namespace BlendedJS
{
    public class BlendedJSEngine
    {
        public static IConsole console = new Console();
        public Engine Jint { get; private set; }

        public BlendedJSEngine()
        {
            Jint = new Jint.Engine(x =>
            {
                //x.AllowDebuggerStatement(true);
                x.DebugMode();
                x.CatchClrExceptions();
            });

            Jint.Step += (sender, info) =>
            {
                console.CurrentLine = info.CurrentStatement.Location.Start.Line;
                return StepMode.Into;
            };
            Jint.SetValue("MongoClient", TypeReference.CreateTypeReference(Jint, typeof(MongoClient)));
            Jint.SetValue("HttpClient", TypeReference.CreateTypeReference(Jint, typeof(HttpClient)));
            Jint.SetValue("SqlClient", TypeReference.CreateTypeReference(Jint, typeof(SqlClient)));
            Jint.SetValue("ObjectId", new Func<string, object>(x => new ObjectId(x)));
            Jint.SetValue("ISODate", new Func<string, object>(x => new ISODate(x)));
            Jint.SetValue("tojson", new Func<object, object>(x => x));
            Jint.SetValue("print", new Action<object>(x => {
                console.Log(x);
            }));
            Jint.SetValue("printjson", new Action<object>(x => {
                console.Log(x);
            }));
            Jint.SetValue("log", new Action<object, object>((message, info) => {
                console.Log(message, info);
            }));
            Jint.SetValue("console.log", new Action<object, object>((message, info) => {
                console.Log(message, info);
            }));
        }

        public BlendedJSResult ExecuteScript(string script)
        {
            console = new Console();
            BlendedJSResult result = new BlendedJSResult();
            try
            {
                Jint.Execute(script);
                result.Value = Jint.GetCompletionValue().ToObject();
            }
            catch (Exception ex)
            {
                console.Log("ERROR: " + ex.Message);
            }
            result.Output = console.Logs;
            return result;
        }
    }
}