using System;
using System.Collections.Generic;
using System.Text;

namespace TelephoneDirectoryApp.Entity.ViewModels
{
    public class AllContactsViewModel
    {
        public int PageIndex { get; set; }
        public int TotalItems { get; set; }
        public List<UserContact> Items { get; set; }
    }
}
