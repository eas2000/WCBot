using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Reflection;

namespace WCBot 
{
    [LuisModel("c2f71a06-9544-4b5c-86b0-b7abd7beb652", "3d1ebb89be3c4fdbb0ff79d790017a74")]
    [Serializable]
    public class WCBotLUISDialog : LuisDialog<object>
    {
        const string Greeting = @" I am the Alegeus WealthCare Wizard.  I will be your guide in the exciting world of the Consumer Driven Healthcare.";
        const string Good = @"Alegeus";
        static string[] Bad = new string[3] { "Evolution One", "Evil7", "Wexed Evolion 1" };
        static Random rnd = new Random();


        internal static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }


        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand what you said. ";// + string.Join(", ", result.Intents.Select(i => i.Intent));
            message += "\n" + "So far I can only answer basic questions about who has the best CDH solution.";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            //Microsoft.Bot.Builder.Dialogs.Internals.JObjectBotData data = GetInstanceField(typeof(Microsoft.Bot.Builder.Dialogs.Internals.DialogContext), context, "botData") as Microsoft.Bot.Builder.Dialogs.Internals.JObjectBotData;
            Microsoft.Bot.Builder.Dialogs.Internals.AlwaysSendDirect_BotToUser data = GetInstanceField(typeof(Microsoft.Bot.Builder.Dialogs.Internals.DialogContext), context, "botToUser") as Microsoft.Bot.Builder.Dialogs.Internals.AlwaysSendDirect_BotToUser;
            Microsoft.Bot.Connector.Activity originalMessage = GetInstanceField(typeof(Microsoft.Bot.Builder.Dialogs.Internals.AlwaysSendDirect_BotToUser), data, "toBot") as Microsoft.Bot.Connector.Activity;

            

            string userName = originalMessage.From.Name;
            //context.PerUserInConversationData.SetValue<AlegeusUser>("auser", new AlegeusUser());
            string message = "Hello, " + userName + "!" + Greeting;

            try
            {

                //message += " Type=" + originalMessage.Type;
                //message += " From=" + originalMessage.From.ToString();
                //message += " Type=" + originalMessage.Type;
                //message += " From.ChannelId=" + originalMessage.From.ChannelId;
                //message += " From.IsBot=" + originalMessage.From.IsBot;
                //message += " From.Id=" + originalMessage.From.Id;
                //message += " From.Address=" + originalMessage.From.Address;
                //message += " From.Name=" + originalMessage.From.Name;
            }
            catch (Exception e)
            {

                message += " Exception:" + e.Message;
            }

            await context.PostAsync(message);
            //await context.PostAsync("test");
            context.Wait(MessageReceived);
        }

        [LuisIntent("whois")]
        public async Task WhoIsTheBest(IDialogContext context, LuisResult result)
        {

            EntityRecommendation Qualifier;

            if (!result.TryFindEntityEx("qualifier", out Qualifier))
            {
                Qualifier = new EntityRecommendation(type: "qualifier") { Entity = "" };
            }

            if (Qualifier.GetChildType() == "first")
            {
                string r = Good;
                if (result.Query.ToUpper().StartsWith("WHOS"))
                    r += "'s";
                await context.PostAsync(r);
            }
            else if (Qualifier.GetChildType() == "last")
            {
                int n = rnd.Next(0, 2);
                string r = Bad[n];
                if (result.Query.ToUpper().Contains("WHOS"))
                    r += "'s";
                await context.PostAsync(r);
            }
            else
            {
                await context.PostAsync("I don't know what to say.");
            }
            context.Wait(MessageReceived);
        }

    }


}