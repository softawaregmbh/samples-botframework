using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ConferenceBot.Models;
using System.Collections.Generic;

namespace ConferenceBot.Dialogs
{
    [Serializable]
    public class AdvancedConferenceDialog : IDialog<ScheduleQuery>
    {
        private ScheduleQuery query;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var message = activity.Text?.ToLower() ?? string.Empty;

            this.query = new ScheduleQuery();

            if (message.Contains("wann"))
            {
                var talkQuestion = new PromptDialog.PromptString("Welcher Vortrag?", "Entschuldigung. Welchen Vortrag meinen Sie?", 3);
                context.Call(talkQuestion, ResumeAfterTalkAsync);
            }
        }

        private async Task ResumeAfterTalkAsync(IDialogContext context, IAwaitable<string> result)
        {
            this.query.Talk = await result;

            var typingMessage = context.MakeMessage();
            typingMessage.Type = ActivityTypes.Typing;

            await context.PostAsync(typingMessage);

            await Task.Delay(3000);

            var responseMessage = context.MakeMessage();

            var card = new HeroCard()
            {
                Title = "Microsoft Bot Framework",
                Subtitle = "Roman Schacherl",
                Text = $"Der Vortrag {query.Talk} findet um 10:30 statt.",
                Images = new List<CardImage>()
                        {
                            new CardImage()
                            {
                                Url = "https://basta.net/wp-content/uploads/2016/03/160920-091311-basta2016-107-1500px.jpg",
                                Tap = new CardAction(ActionTypes.OpenUrl, "Open", null, "https://www.basta.net")
                            }
                        }
            };

            responseMessage.Attachments.Add(card.ToAttachment());

            await context.PostAsync(responseMessage);

            context.Done(this.query);
        }
    }
}