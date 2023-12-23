﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewKeyValue
    {
        public object Key { get; set; }
        public object Value { get; set; }

        public ViewKeyValue() { }
    }

    public class ViewKeyValueString
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ViewKeyValueString() { }
    }
}
