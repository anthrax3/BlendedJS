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
using System.Threading;
using BlendedJS.Cassandra;
using BlendedJS.Redis;
using BlendedJS.Types;
using Decimal = System.Decimal;
using Double = System.Double;

namespace BlendedJS
{
    public class BlendedJSEngine : IDisposable
    {
        public static AsyncLocal<Console> Console = new AsyncLocal<BlendedJS.Console>();
        public static AsyncLocal<List<object>> Clients = new AsyncLocal<List<object>>();
        public Engine Jint { get; private set; }

        public BlendedJSEngine()
        {
            Console.Value = new BlendedJS.Console();
            Clients.Value = new List<object>();
            Jint = new Jint.Engine(x =>
            {
                x.DebugMode();
                x.CatchClrExceptions();
            });

            Jint.Step += (sender, info) =>
            {
                Console.Value.currentLine = info.CurrentStatement.Location.Start.Line;
                return StepMode.Into;
            };
            Jint.SetValue("BigInteger", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.BigInteger)));
            Jint.SetValue("Decimal", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Decimal)));
            Jint.SetValue("Double", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Double)));
            Jint.SetValue("Float", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Float)));
            Jint.SetValue("Guid", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Guid)));
            Jint.SetValue("Int", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Int)));
            Jint.SetValue("Long", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Long)));
            Jint.SetValue("Short", TypeReference.CreateTypeReference(Jint, typeof(BlendedJS.Types.Short)));
            Jint.SetValue("SqlClient", TypeReference.CreateTypeReference(Jint, typeof(SqlClient)));
            Jint.SetValue("MongoClient", TypeReference.CreateTypeReference(Jint, typeof(MongoClient)));
            Jint.SetValue("HttpClient", TypeReference.CreateTypeReference(Jint, typeof(HttpClient)));
            Jint.SetValue("FtpClient", TypeReference.CreateTypeReference(Jint, typeof(FtpClient)));
            Jint.SetValue("SftpClient", TypeReference.CreateTypeReference(Jint, typeof(SftpClient)));
            Jint.SetValue("SshClient", TypeReference.CreateTypeReference(Jint, typeof(SshClient)));
            Jint.SetValue("CassandraClient", TypeReference.CreateTypeReference(Jint, typeof(CassandraClient)));
            Jint.SetValue("RedisClient", TypeReference.CreateTypeReference(Jint, typeof(RedisClient)));
            Jint.SetValue("ObjectId", new Func<string, object>(x => new ObjectId(x)));
            Jint.SetValue("ISODate", new Func<string, object>(x => new ISODate(x)));
            Jint.SetValue("tojson", new Func<object, object>(x => x));
            Jint.SetValue("print", new Action<object>(x => {
                Console.Value.log(x);
            }));
            Jint.SetValue("printjson", new Action<object>(x => {
                Console.Value.log(x);
            }));
            Jint.SetValue("log", new Action<object, object>((message, info) => {
                Console.Value.log(message, info);
            }));
            Jint.SetValue("console", Console.Value);
        }
    
        public BlendedJSResult ExecuteScript(string script)
        {
            BlendedJSResult result = new BlendedJSResult();
            try
            {
                Jint.Execute(script);
                result.Value = Jint.GetCompletionValue().ToObject();
            }
            catch (Exception ex)
            {
                Console.Value.log("ERROR, Line " + Console.Value.currentLine + " :" + ex.Message);
                result.Exception = ex;
            }
            result.Logs = Console.Value.logs;
            result.LastExecutedLine = Console.Value.currentLine;
            return result;
        }

        public void Dispose()
        {
            foreach (var client in Clients.Value)
            {
                if (client is IDisposable)
                    ((IDisposable)client).Dispose();
            }
        }
    }
}