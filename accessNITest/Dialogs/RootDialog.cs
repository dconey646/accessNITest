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
            await context.PostAsync($"Bout ye! I see that you are on the NI Direct website!");

            PromptDialog.Text(context, this.FirstQuestionResponse, "How can I help you today?");
        }

        private async Task FirstQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            await context.PostAsync($"Got it. You are looking for help with '{description}'.");
            await context.PostAsync($"I can help you with that!");

            PromptDialog.Text(context, this.SecondQuestionResponse, $"Before we get started can I ask for name?");

        }

        private async Task SecondQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            await context.PostAsync($"Great {description}, Let's start!");

            PromptDialog.Text(context, this.ThirdQuestionResponse, $"To save you time you will need the following information at hand:" +
                $"National Insuranace Number, Drivers Licence, Passport, Addresses that you have lived at for the past 5 years(and corresponding dates)" +
                $"and a valid debit or credit card. DO you have all this information to hand?");

        }

        private async Task ThirdQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;

            await context.PostAsync($"Now I will gather some of the personal information required to complete the application.");
            PromptDialog.Text(context, this.FourthQuestionResponse, $"Please can you enter your Date Of Birth in the following format DD/MM/YYYY:");

        }

        private async Task FourthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            PromptDialog.Text(context, this.FifthQuestionResponse, $"Please State Your Gender.");
            await context.PostAsync($"Male");
            await context.PostAsync($"Female");
            await context.PostAsync($"Other");


        }

        private async Task FifthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            PromptDialog.Text(context, this.FifthQuestionResponse, $"Please Select Your Gender.");


        }




    }
}