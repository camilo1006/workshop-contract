namespace Inventory.Dto
{
    using System;
    using System.Collections.Generic;

    public class ResponsePreserver
    {
        public List<ItemToExpire> items { get; set; }

        public DateTime date { get; set; }
    }

    public class ItemToExpire
    {
        public string name { get; set; }

        public double quantity { get; set; }
    }
}
