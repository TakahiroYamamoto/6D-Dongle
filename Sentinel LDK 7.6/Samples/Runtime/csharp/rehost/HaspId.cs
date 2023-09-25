using System;
using System.Collections.Generic;
using System.Text;

namespace hasp_rehost
{
    class HaspId
    {
              private String id;

        public HaspId(String aId)
        {
          id = aId;
        }
         public String getId()
        {
           return id;
        }

        public override String ToString()
        {
          return id;
        }
    }

    }

