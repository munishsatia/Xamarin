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
    public class QAService
    {
        public Repository repo;
        public QAService()
        {
            repo = new Repository();
        }

        public IList<QA> GetQA()
        {
            return repo.GetForm().ToList();
        }

        public void SaveForm(IList<QA> qa)
        {
            repo.SaveForm(qa);
        }

       
    }
}