using ConferenceBot.Models;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ConferenceBot.Dialogs
{
    public class SimpleConferenceDialog
    {
        public static IForm<ScheduleQuery> BuildForm()
        {
            return new FormBuilder<ScheduleQuery>()
                .Message("Hi, I need some information from you!")
                .Field(nameof(ScheduleQuery.Time))
                .Field(nameof(ScheduleQuery.Room))
                .OnCompletion(async (context, query) =>
                {
                    var typingMessage = context.MakeMessage();
                    typingMessage.Type = ActivityTypes.Typing;

                    await context.PostAsync(typingMessage);

                    await Task.Delay(3000);

                    var responseMessage = context.MakeMessage();
                    //responseMessage.Text = $"In Raum {query.Room} findet ein Lab über das Bot Framework statt.";

                    var card = new HeroCard()
                    {
                        Title = "Microsoft Bot Framework",
                        Subtitle = "Roman Schacherl",
                        Text = $"In Raum {query.Room} findet ein Lab über das Bot Framework statt.",
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
                })
                .Build();
        }
    }
}