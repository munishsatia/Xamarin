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

namespace HybridApp.Core
{
    public class QAModel
    {
        public QAModel()
        {
            QuestionsAnswers = new List<QA>();
        }
        public List<QA> QuestionsAnswers { get; set; }
        
    }

    public class QA
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string UserName { get; set; }
    }
}