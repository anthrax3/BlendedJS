using Jint;
using MongoDB.Bson;
using System.Dynamic;
using Jint.Runtime.Interop;
using System;
using BlendedJS.Ftp;
using BlendedJS.Http;
using BlendedJS.Mongo;
using BlendedJS.Sql;
using Jint.Runtime.Debugger;

namespace BlendedJS
{
    public class BlendedJSEngine
    {
        [ThreadStatic]
        public static Console Console = null;
        public Engine Jint { get; private set; }

        public BlendedJSEngine()
        {
            Jint = new Jint.Engine(x =>
            {
                x.DebugMode();
                x.CatchClrExceptions();
            });

            Jint.Step += (sender, info) =>
            {
                Console.currentLine = info.CurrentStatement.Location.Start.Line;
                return StepMode.Into;
            };
            Jint.SetValue("SqlClient", TypeReference.CreateTypeReference(Jint, typeof(SqlClient)));
            Jint.SetValue("MongoClient", TypeReference.CreateTypeReference(Jint, typeof(MongoClient)));
            Jint.SetValue("HttpClient", TypeReference.CreateTypeReference(Jint, typeof(HttpClient)));
            Jint.SetValue("FtpClient", TypeReference.CreateTypeReference(Jint, typeof(FtpClient)));
            Jint.SetValue("ObjectId", new Func<string, object>(x => new ObjectId(x)));
            Jint.SetValue("ISODate", new Func<string, object>(x => new ISODate(x)));
            Jint.SetValue("tojson", new Func<object, object>(x => x));
            Jint.SetValue("print", new Action<object>(x => {
                Console.log(x);
            }));
            Jint.SetValue("printjson", new Action<object>(x => {
                Console.log(x);
            }));
            Jint.SetValue("log", new Action<object, object>((message, info) => {
                Console.log(message, info);
            }));
        }

        public BlendedJSResult ExecuteScript(string script)
        {
            Console = new Console();
            Jint.SetValue("console", Console);
            BlendedJSResult result = new BlendedJSResult();
            try
            {
                Jint.Execute(script);
                result.Value = Jint.GetCompletionValue().ToObject();
            }
            catch (Exception ex)
            {
                Console.log("ERROR: " + ex.Message);
            }
            result.Console = Console.logs;
            return result;
        }
    }
}