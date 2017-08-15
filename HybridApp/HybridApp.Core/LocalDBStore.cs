using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

namespace HybridApp.Core
{
    public class LocalDBStore
    {
        static object locker = new object();
        public string LocalDBPath;
        public SqliteConnection connection;

        public string path;
        public string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "qaans.db3";
                var folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //folderPath = Android.OS.Environment.DataDirectory.AbsolutePath;
                folderPath = "/storage/emulated/0/Android/Data/HybridApp.HybridApp/Files/";
                   // / data / user / 0 / HybridApp.HybridApp / files
                  // folderPath = "/android/data/com.hybrid/";
                var fullpath = Path.Combine(folderPath, sqliteFilename);
                path = fullpath;
                return fullpath;
            }
        }
        public LocalDBStore()
        {
            
            var dbPath = DatabaseFilePath;
            // create the tables
            bool exists = File.Exists(dbPath);

            if (!exists)
            {
                var connection = new SqliteConnection("Data Source=" + dbPath);

                connection.Open();
                var commands = new[] {
                    "CREATE TABLE [QuestionAnswers] (Id INTEGER PRIMARY KEY ASC, Question NTEXT, Answer NTEXT, UserName INTEGER);"
                };
                foreach (var command in commands)
                {
                    using (var c = connection.CreateCommand())
                    {
                        c.CommandText = command;
                        var i = c.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // already exists, do nothing. 
            }
            
        }

      

        public IEnumerable<QA> GetQAs()
        {
            var qaList = new List<QA>();

            lock (locker)
            {
                var connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var contents = connection.CreateCommand())
                {
                    contents.CommandText = "SELECT [Id], [Question], [Answer], [UserName] from [QuestionAnswers]";
                    var r = contents.ExecuteReader();
                    while (r.Read())
                    {
                        var qa = new QA();
                        qa.Id = Convert.ToInt32(r["Id"]);
                        qa.Question= r["Question"].ToString();
                        qa.Answer= r["Answer"].ToString();
                        qa.UserName= r["UserName"].ToString();
                        qaList.Add(qa);                        
                    }
                }
                connection.Close();
            }
            return qaList;
        }

        public void SaveForm(List<QA> questionAns)
        {
            questionAns.ForEach(q => SaveItem(q));
        }
     

         public int SaveItem(QA question)
        {
            int r;
            lock (locker)
            {

                
                if (question.Id != 0)
                {
                    connection = new SqliteConnection("Data Source=" + path);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE [QuestionAnswers] SET [Question] = ?, [Answer] = ?, [UserName] = ? WHERE [Id] = ?;";
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = question.Question});
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = question.Answer });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = question.UserName});
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = question.Id});
                        r = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return r;
                }
                else
                {
                    connection = new SqliteConnection("Data Source=" + path);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [QuestionAnswers] ([Question], [Answer], [UserName]) VALUES (? ,?, ?)";
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = question.Question });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = question.Answer });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = question.UserName });
                        r = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return r;
                }

            }
        }
    }
}