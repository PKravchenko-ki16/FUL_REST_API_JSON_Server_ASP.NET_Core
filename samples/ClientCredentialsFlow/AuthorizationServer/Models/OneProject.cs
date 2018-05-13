using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationServer.Models
{
    public class OneProject
    {
        public int id { get; set; }

        public string name { get; set; }

        public Item[] items { get; set; }
    }
    public class Item
    {
        public int id { get; set; }

        public int project_id { get; set; }

        public string content { get; set; }

        public string user_id { get; set; }

        public int indent { get; set; }

        public int priority { get; set; }

        public int item_order { get; set; }

        //public int "checked" {get;set;}

        public int is_deleted { get; set; }

        public string date_string { get; set; }
    }
}
