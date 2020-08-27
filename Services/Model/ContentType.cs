using System;
using System.Collections.Generic;
using System.Text;

namespace DevOps.ManagementServices.APIManagement.Model
{
    public class ContentType
    {
        public string id { get; set; }
    }

    public class ContentTypeResponse
    {
        public List<ContentType> value { get; set; }

        public int count { get; set; }

        public string nextLink { get; set; }
    }
}
