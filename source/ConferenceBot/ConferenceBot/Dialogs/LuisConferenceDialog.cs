using ConferenceBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ConferenceBot.Dialogs
{
    [LuisModel("8ab12548-3dc5-4456-86b1-2b726d3abaf6", "67b3432988354c6d90ab346d4554a104")]
    [Serializable]
    public class LuisConferenceDialog : LuisDialog<ScheduleQuery>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Tut mir leid, das habe ich nicht verstanden: "
                + string.Join(", ", result.Intents.Select(i => i.Intent));

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("ScheduleQuery")]
        public async Task GetSessionInfo(IDialogContext context, LuisResult result)
        {
            var room = result.Entities.FirstOrDefault(p => p.Type == "Raum");
            var time = result.Entities.FirstOrDefault(p => p.Type == "Uhrzeit");

            await context.PostAsync($"Ich sehe nach, was im **{room.Entity}** um **{time.Entity}** läuft...");

            await Task.Delay(1000);

            await context.PostAsync($"Da ist die Keynote!");

            context.Wait(MessageReceived);
        }
    }
}