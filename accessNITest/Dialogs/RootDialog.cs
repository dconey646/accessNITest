using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace accessNITest.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private AccessNIInformation currentChatter;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            currentChatter = new AccessNIInformation();

            var activity = await result as Activity;

            // return our reply to the user
            await context.PostAsync($"Hi! I see that you are on the NI Direct website!");

            PromptDialog.Text(context, this.FirstQuestionResponse, "How can I help you today?");
        }

        private async Task FirstQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            await context.PostAsync($"Got it. You are looking for help with '{description}'.");
            await context.PostAsync($"I can help you with that!");

            context.Done<object>(null);

        }
    }
}