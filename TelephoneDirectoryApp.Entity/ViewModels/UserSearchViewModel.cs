using System;
using System.Collections.Generic;
using System.Text;

namespace TelephoneDirectoryApp.Entity.ViewModels
{
    public class UserSearchViewModel
    {
        public string UsernameSearch { get; set; }
        public string FirstNameSearch { get; set; }
        public string LastNameSearch { get; set; }
        public string EmailSearch { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
