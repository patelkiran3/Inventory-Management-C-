﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.BLL
{
    class categoriesBLL
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime added_date { get; set; }
        public int added_by { get; set; }
    }
}
