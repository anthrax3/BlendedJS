using Jint;
using MongoDB.Bson;
using System.Dynamic;
using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using BlendedJS.Ftp;
using BlendedJS.Http;
using BlendedJS.Mongo;
using BlendedJS.Sql;
using Jint.Runtime.Debugger;
using BlendedJS.Sftp;
using BlendedJS.Ssh;

namespace BlendedJS
{
    public class BlendedJSEngine : IDisposable
    {
        [ThreadStatic]
        public static Console Console = null;
        [ThreadStatic]
        public static List<object> Clients = new List<object>();
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
            Jint.SetValue("SftpClient", TypeReference.CreateTypeReference(Jint, typeof(SftpClient)));
            Jint.SetValue("SshClient", TypeReference.CreateTypeReference(Jint, typeof(SshClient)));
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
                result.Exception = ex;
            }
            result.Console = Console.logs;
            return result;
        }

        public void Dispose()
        {
            foreach (var client in Clients)
            {
                if (client is IDisposable)
                    ((IDisposable)client).Dispose();
            }
        }
    }
}