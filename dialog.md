# AdvancedConferenceDialog

## Refactoring
* Benennen Sie die bestehende Klasse ```RootDialog``` in ```AdvancedConferenceDialog``` um (Hinweis: Visual Studio 2017 bietet mit Strg+. einen Quick fix zur Umbenennung des Dateinamens an)
* Unser Dialog liefert ein ```IDialog<ScheduleQuery>``` statt ```IDialog<object>```

```cs
public class AdvancedConferenceDialog : IDialog<ScheduleQuery>
```

Achten Sie im folgenden Code-Abschnitt auf folgende Details:
* Ein *Dialog* liefert immer ein Ergebnis. In unserem Fall vom Typ ```ScheduleQuery```, im SDK sind aber für Standard-Datentypen bereits Dialoge vordefiniert (zB ```PromptString```).
* ```context.Call()``` führt zum Senden an den Benutzer, sobald eine Antwort eingegeben wird, wird beim angegebenen Callback fortgeführt.
* Innerhalb eines Callbacks können auch mehrere Nachrichten an den Benutzer gesandt werden (siehe ```ResumeAfterTalkAsync```)

```cs
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
        responseMessage.Text = $"Der Vortrag {query.Talk} findet um 10:30 statt.";

        responseMessage.Attachments.Add(card.ToAttachment());

        await context.PostAsync(responseMessage);

        context.Done(this.query);
    }
}
```

Rufen Sie den ```AdvancedConferenceDialog``` nun auch im ```MessagesController``` auf.

```cs
await Conversation.SendAsync(activity, () => new Dialogs.AdvancedConferenceDialog());
```



## Hero Cards
Anstatt einer Textnachricht könnte am Schluss auch eine *HeroCard* angezeigt werden. Weitere Attachments finden Sie unter [Add rich card attachments to messages](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-add-rich-card-attachments)

```cs
var card = new HeroCard()
{
    Title = "Microsoft Bot Framework",
    Subtitle = "Roman Schacherl",
    Text = $"In Raum {query.Room} findet ein Lab über das Bot Framework statt.",
    Images = new List<CardImage>()
    {
        new CardImage()
        {
            Url = "https://api-summit.de/wp-content/uploads/2017/03/API_Summit-3914.jpg",
            Tap = new CardAction(ActionTypes.OpenUrl, "Open", null, "https://www.api-summit.de")
        }
    }
};

responseMessage.Attachments.Add(card.ToAttachment());
```

![HeroCard](images/herocard.png)

Die einzelnen Darstellungen auf den verschiedenen Channels können Sie auch mit dem [Channel Inspector](https://docs.botframework.com/en-us/channel-inspector/channels/Facebook?f=Channeldata&e=example1) simulieren.