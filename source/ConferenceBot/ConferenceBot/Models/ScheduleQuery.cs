﻿using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceBot.Models
{
    [Serializable]
    public class ScheduleQuery
    {
        public string Speaker { get; set; }

        public Room? Room { get; set; }

        public string Talk { get; set; }

        [Prompt("Please enter the begin time of the talk")]
        public string Time { get; set; }
    }
}