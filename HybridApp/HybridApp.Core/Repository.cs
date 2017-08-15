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
using System.IO;

namespace HybridApp.Core
{
    public class Repository
    {
        protected static LocalDBStore db; 

        public Repository()
        {
            db = new LocalDBStore();
        }


        public void SaveForm(IList<QA> qa)
        {
            db.SaveForm(qa.ToList());
        }

        public IEnumerable<QA> GetForm()
        {
            return db.GetQAs();
        }
    }
}