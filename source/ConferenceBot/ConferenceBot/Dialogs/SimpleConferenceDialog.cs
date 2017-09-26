using ConferenceBot.Models;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceBot.Dialogs
{
    public class SimpleConferenceDialog
    {
        public static IForm<ScheduleQuery> BuildForm()
        {
            return new FormBuilder<ScheduleQuery>()
                .Build();
        }
    }
}