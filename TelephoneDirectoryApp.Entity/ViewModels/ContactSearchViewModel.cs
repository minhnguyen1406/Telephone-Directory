using System;
using System.Collections.Generic;
using System.Text;

namespace TelephoneDirectoryApp.Entity.ViewModels
{
    public class ContactSearchViewModel
    {
        public string FirstNameSearch { get; set; }
        public string LastNameSearch { get; set; }
        public string PhoneSearch { get; set; }
        public int UserId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
