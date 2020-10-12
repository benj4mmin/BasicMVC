using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicMVC.Models
{
    public class ViewModelLogin
    {
        public List<Userlogin> allLogins { get; set; }
        public List<SiteScheduler> allSiteMessages { get; set; }
    }
}