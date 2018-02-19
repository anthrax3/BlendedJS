using Jint;
using MongoDB.Bson;
using System.Dynamic;
using Jint.Runtime.Interop;
using System;

namespace BlendedJS.Mongo
{
    public class BlendedJSEngine
    {
        public static IConsole console = new Console();

        public BlendedJSResult ExecuteScript(string script)
        {
            console = new Console();
            BlendedJSResult result = new BlendedJSResult();
            var jint = new Jint.Engine(x =>
            {
                //x.AllowDebuggerStatement(true);
                x.DebugMode();
                x.CatchClrExceptions();
            });
            jint.Step += (sender, info) =>
            {
                console.CurrentLine = info.CurrentStatement.Location.Start.Line;
                return Jint.Runtime.Debugger.StepMode.Into;
            };
            jint.SetValue("MongoClient", TypeReference.CreateTypeReference(jint, typeof(MongoClient)));
            jint.SetValue("ObjectId", new Func<string, object>(x => {
                return new ObjectId(x);
                }));
            jint.SetValue("ISODate", new Func<string, object>(x => {
                return new ISODate(x);
            }));
            jint.SetValue("tojson", new Func<object, object>(x => {
                return x;
            }));
            jint.SetValue("print", new Action<object>(x => {
                console.Log(x);
            }));
            jint.SetValue("printjson", new Action<object>(x => {
                console.Log(x);
            }));
            jint.SetValue("log", new Action<object, object>((message, info) => {
                console.Log(message, info);
            }));
            jint.SetValue("console.log", new Action<object, object>((message, info) => {
                console.Log(message, info);
            }));
            try
            {
                jint.Execute(script);
                result.Value = jint.GetCompletionValue().ToObject();
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