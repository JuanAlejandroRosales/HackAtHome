﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HackAtHome.Entities
{
    public class LabItem
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Lab { get; set; }
        public string DeviceId { get; set; }
    }
}