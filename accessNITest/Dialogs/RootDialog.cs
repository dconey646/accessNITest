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
        private Validator validator;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            currentChatter = new AccessNIInformation();
            validator = new Validator();

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

            PromptDialog.Text(context, this.SecondQuestionResponse, $"Before we get started can I ask for your name?");

        }

        private async Task SecondQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            currentChatter.FirstName = await result;
            await context.PostAsync($"Great {currentChatter.FirstName}, Let's start!");

            PromptDialog.Text(context, this.ThirdQuestionResponse, $"To save you time you will need the following information at hand:" +
                $"National Insuranace Number, Drivers Licence, Passport, Addresses that you have lived at for the past 5 years(and corresponding dates)" +
                $"and a valid debit or credit card. DO you have all this information to hand?");

        }

        private async Task ThirdQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;

            if (description == "false")
            {
                context.Done<object>(null);
            }

            await context.PostAsync($"Now I will gather some of the personal information required to complete the application.");
            PromptDialog.Text(context, this.FourthQuestionResponse, $"Please can you enter your NINO (National Insurance Number):");

        }

        private async Task FourthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            currentChatter.NINumber = await result;
            validator.CheckNINumber(currentChatter.NINumber);

            await context.PostAsync($"{currentChatter.FirstName}, Thank you for that!");
            await context.PostAsync($"Date of Birth: 28/10/1993");
            await context.PostAsync($"Gender: Male");
            PromptDialog.Text(context, this.FifthQuestionResponse, $"Is this information correct?");
        }

        private async Task FifthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            await context.PostAsync($"Thank you for confirming that {currentChatter.FirstName}.");
            PromptDialog.Text(context, this.SixthQuestionResponse, $"Now, do you have any previous forenames? Yes or No will do :)");
        }

        private async Task SixthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            if (description.Equals("No"))
            {
                PromptDialog.Text(context, this.SeventhQuestionResponse, $"Okay, can you tell me your current surname please?");
            }
        }

        private async Task SeventhQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            currentChatter.LastName = await result;
            await context.PostAsync($"Thank you {currentChatter.FirstName} {currentChatter.LastName} for the information so far, nearly done :)");
            PromptDialog.Text(context, this.EighthQuestionResponse, "Do you hold a valid driving license?");
        }

        private async Task EighthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            PromptDialog.Text(context, this.NinthQuestionResponse, "Okay, please enter your driving license number. 8 digits for Northern Ireland, 16 digits for Mainland UK.");
        }

        private async Task NinthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            bool valid = validator.CheckDriverLicenseNumber(description);
            if (valid)
            {
                currentChatter.DriverLicenseNumber = Convert.ToInt32(description);
                await context.PostAsync("Thank you for that! So, we have worked out the following information:");
                await context.PostAsync("Place of Birth: Newtownards");
                await context.PostAsync("Country of Birth: Northern Ireland");
                PromptDialog.Text(context, this.TenthQuestionResponse, "Is this information is correct?");
            }
        }

        private async Task TenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            if (answer.Equals("Yes"))
            {
                await context.PostAsync("Thanks for confirming!");
                PromptDialog.Text(context, this.EleventhQuestionResponse, "Do you hold a passport?");
            }
        }

        private async Task EleventhQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            if (answer.Equals("Yes"))
            {
                PromptDialog.Text(context, this.TwelfthQuestionResponse, "Okay, what is your passport number?");
            }
        }

        private async Task TwelfthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            bool valid = validator.CheckPassportNumber(answer);
            if (valid)
            {
                await context.PostAsync("Thanks for your passport number. We have found the following information:");
                await context.PostAsync("Nationality: Irish");
                await context.PostAsync("Country of Issue: GB");
                PromptDialog.Text(context, this.ThirteenthQuestionResponse, "Is this information correct?");
            }
        }

        private async Task ThirteenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            if (description.Equals("Yes"))
            {
                await context.PostAsync("Awesome! Thank you for confirming. Almost there!");
                PromptDialog.Text(context, this.FourteenthQuestionResponse, "Can you enter your postcode and house number like so: E.G BT127AB 71");
            }
        }

        private async Task FourteenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String description = await result;
            String[] houseInfo = description.Split(' ');
            bool decision = validator.CheckAddress(houseInfo[0], Convert.ToInt32(houseInfo[1]));
            if(decision)
            {
                await context.PostAsync("We have got the following address:");
                await context.PostAsync("71 East Mount,");
                await context.PostAsync("Newtownards");
                await context.PostAsync("Co, Down");
                await context.PostAsync("BT23 8SE");
                PromptDialog.Text(context, this.FifteenthQuestionResponse, "Is this address correct?");
            }
        }

        private async Task FifteenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            if(answer.Equals("Yes"))
            {
                PromptDialog.Text(context, this.SixteenthQuestionResponse, "Awesome! How many years have you lived at this address?");
            }
        }

        private async Task SixteenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            if(Convert.ToInt32(answer) > 5)
            {
                await context.PostAsync($"So you have lived at that address for {answer} years. Thank you! Now for the last two questions :)");
                PromptDialog.Text(context, this.SeventeenthQuestionResponse, "Can you give me your email address please? We will use this to email your certificate once the application is completed!");
            }
        }

        private async Task SeventeenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            currentChatter.EmailAddress = answer;
            await context.PostAsync("Thanks for your email address");
            PromptDialog.Text(context, this.EighteenthQuestionResponse, "Can we have your contact telephone number?");
        }

        private async Task EighteenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String answer = await result;
            currentChatter.ContactNumber = Convert.ToInt64(answer);
            await context.PostAsync("Thank you for that information!");
            await context.PostAsync("Now, we need to decide on a payment method for the check.");
            PromptDialog.Text(context, this.NineteenthQuestionResponse, "Would you like to pay by 'Debit Card' or Postal Order?");
        }

        private async Task NineteenthQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String decision = await result;
            if(decision.Equals("Debit Card"))
            {
                await context.PostAsync("Thank you for choosing Debit Card.");
                await context.PostAsync("We will prompt you to go another web page to complete your payment.");
                await context.PostAsync("Once your payment has been received, your application will be submitted.");
                PromptDialog.Text(context, this.TwentiethQuestionResponse, "By typing 'submit' you will be prompted to " +
                    "enter your debit card details. This also means you are agreeing to the declaration and for your information to be sent to access ni.");
            }
        }

        private async Task TwentiethQuestionResponse(IDialogContext context, IAwaitable<string> result)
        {
            String decision = await result;
            if(decision.Equals("submit"))
            {
                await context.PostAsync($"Thank you for submitting your application! Your reference number is: AB12345667. Your certificate will be emailed to you at {currentChatter.EmailAddress}, once processing has finished.");
            }
        }
    }
}