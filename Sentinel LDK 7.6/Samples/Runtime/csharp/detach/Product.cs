using System;
using System.Collections.Generic;
using System.Text;

namespace detach_cs
{
    class Product
    {
        private String id;
        private String name;

        public Product(String aId, String aName)
        {
            id = aId;
            name = aName;
        }

        public String getName()
        {
            return name;
        }

        public String getId()
        {
            return id;
        }

        public override String ToString()
        {
            return name + " (" + id + ")";
        }
    }
}
