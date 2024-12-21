using Inventory.Models;
using Microsoft.Identity.Client;

namespace Inventory.Services
{
    public class IndexData
    {
        public Pager pager { get; set; }
        public dynamic data { get; set; }
    }
}
