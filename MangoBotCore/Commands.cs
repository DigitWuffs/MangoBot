﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MangoBotStartup;
using Mono.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using unirest_net.http;

namespace MangoBotCommandsNamespace
{
    public class PPSize
    {
        public Dictionary<ulong, int> ppsize { get; set; } = new Dictionary<ulong, int>();
    }

    public class Commands : ModuleBase<SocketCommandContext>
    {
        private BotConfig config;

        [Command("ping")]
        private async Task Ping(params string[] args)
        {
            await ReplyAsync("Pong! 🏓 **" + Program._client.Latency + "ms**");
        }
        [Command("status")]
        [RequireOwner]
        private async Task status([Remainder] string args)
        {
             await Program._client.SetGameAsync($"{args}");
        }
        [Command("help")]
        private async Task help()
        {
            ulong authorid = Context.Message.Author.Id;
            config = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("config.json"));
            string prefix = config.prefix;
            string CommandsList = $"**Commands:**\n" +
    $"***Current Prefix is {prefix}***\n" +
    $"----------------------------\n" +
    $"**help:** *Displays this command.*\n" +
    $"**about:** *Displays some information about the bot!*\n" +
    $"**todo:** *Lists any upcoming commands and/or things that need to be fixed/changed.*\n" +
    $"**penis:** *Generates a penis size for the mentioned user.*\n" +
    $"**8ball:** *Read the future with this 8ball command!*\n" +
    $"**ping:** *Sends the ping of the discord bot.*\n" +
    $"**slap @user:** *Slaps specified user.*\n" +
    $"**joke:** *Tells a dad joke!*\n" +
    $"**avatar:** *Sends the avatar of the person mentioned, or yourself if nobody is mentioned.*\n" +
    $"**defaultavatar:** *Sends the default avatar of the person mentioned, or yourself if nobody is mentioned.*\n";
            if (!Context.IsPrivate && Context.Guild.GetUser(authorid).GuildPermissions.ManageMessages == true)
            {
                CommandsList = (CommandsList + $"\n**Moderator Commands:**\n" +
                    $"----------------------------\n" +
                    $"**purge:** *Purges amount of messages specified (Requires Manage Messages)*\n" +
                    $"**ban:** *Bans mentioned user with reason specified. Ex. `^ban @lXxMangoxXl Not working on MangoBot`. (Requires Ban Members)*\n");
            }
            if (!Context.IsPrivate && Context.Guild.Id == 687875961995132973)
            {
                CommandsList = (CommandsList + "**\nUnlimited SCP Commands**:\n" +
                    "----------------------------\n" +
                    "**minecraft:** *Sends the current IP of the minecraft server*\n" +
                    "**appeal:** *Sends a invite link to the appeal discord*");
            }
            await ReplyAsync(CommandsList);
        }
        [Command("penis")]
        private async Task penis(params string[] args)
        {
            //State config.disabledpenis
            bool runpeniscommand;
            config = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("config.json"));
            //string disabledpenis = config.disabledpenis;

            //Checks to see if penis command is disabled
            if (config.disabledpenis == "1")
            {
                //Checks if it's Unlimited
                if (!Context.IsPrivate && Context.Guild.Id == 687875961995132973)
                {
                    await ReplyAsync("Penis commands have been disabled, sorry!");
                    runpeniscommand = false;
                }
                //If it isn't unlimited, it'll let you through
                else
                {
                    runpeniscommand = true;
                }
            }
            else //If off in configs, send it through.
            {
                runpeniscommand = true;
            }

            //If the checks went through, and 
            if (runpeniscommand == true)
            {
                /*string[] penisquotes = { "8=D", "8==D", "8===D", "8====D", "8=====D", "8======D", "8=======D", "8========D", "8=========D", "8=========D"};
                Random rand = new Random();
                int index = rand.Next(penisquotes.Length);
                await ReplyAsync($"{args}'s penis length is {penisquotes[index]}");*/

                var pp = JsonConvert.DeserializeObject<PPSize>(File.ReadAllText("ppsize.json"));

                //just generate random number and set that num to ppnum
                string[] penisquotes = { "8D", "8=D", "8==D", "8===D", "8====D", "8=====D", "8======D", "8=======D", "8========D", "8=========D", "8=========D", "8==========D" }; //All the different penises that it can send.
                Random rand = new Random(); //Creates a random veriable
                int RandomID = rand.Next(1, 11); //Select one of the two options that it can pick from
                int ppnum = RandomID; //Saves the RandomID to ppnum.
                if (args.Length == 0) // If there isn't any mentioned users.
                {
                    ulong authorid = Context.User.Id;
                    if (pp.ppsize.ContainsKey(authorid))
                    {
                        ppnum = pp.ppsize[authorid];
                    }
                    else
                    {
                        pp.ppsize.Add(authorid, ppnum);
                        File.WriteAllText("ppsize.json", JsonConvert.SerializeObject(pp));
                    }
                    await ReplyAsync($"<@{authorid}>'s penis size is {penisquotes[ppnum]}");
                }
                if (args.Length == 1) //If theres one mentioned user.
                {
                    if (args[0].Contains("@everyone") | args[0].Contains("@here")) //Checking for any everyone or here pings.
                    {
                        await ReplyAsync("Tsk Tsk");
                    }
                    else
                    {
                        ulong UserID = MentionUtils.ParseUser(args[0]); //Takes the UserID from mention and saves it to UserID
                        if (pp.ppsize.ContainsKey(UserID))
                        {
                            ppnum = pp.ppsize[UserID];
                        }
                        else
                        {
                            pp.ppsize.Add(UserID, ppnum);
                            File.WriteAllText("ppsize.json", JsonConvert.SerializeObject(pp));
                        }
                        await ReplyAsync($"{Program._client.GetUser(UserID).Username}'s penis size is {penisquotes[ppnum]}");
                    }
                }
                if (args.Length > 2 | args.Length == 2) //If theres more than one argument
                {
                    await ReplyAsync($"Only have one input argument, you currently have {args.Length}, you're only supposed to have 1!");
                }
            }
        }


        [Command("8ball")]
        private async Task eightball(params string[] args)
        {
            string[] eightballquotes = { "As I see it, yes.", "Ask again later.", "Better not tell you now.", "Cannot predict now.", "Concentrate and ask again.", "Don’t count on it.", "Don’t count on it.", "It is certain.",
                "It is decidedly so.", "Most likely.", "My reply is no.", "My sources say no.", "Outlook not so good.", "Outlook good.", "Reply hazy, try again.", "Signs point to yes", "Very doubtful", "Without a doubt.",
            "Yes.", "Yes - definitely.", "You may rely on it"};
            Random rand = new Random();
            int index = rand.Next(eightballquotes.Length);
            if (args.Length == 0)
            {
                await ReplyAsync("You have to say something in order to recieve a prediction!");
            }
            else
            {
                await ReplyAsync($"{eightballquotes[index]}");
            }
        }
        [Command("slap")]
        private async Task slap(params string[] args)
        {
            if (args.Length > 2 | args.Length == 2)
            {
                await ReplyAsync("Only mention one user!");
            }
            if (args.Length == 0)
            {
                await ReplyAsync($"<@{Context.User.Id}>... Slapped themself?");
            }
            if (args.Length == 1)
            {
                if (args[0].Contains("@everyone") | args[0].Contains("@here"))
                {
                    await ReplyAsync("Tsk Tsk");
                }
                else
                {
                    if (args[0].Contains("@"))
                    {
                        ulong CLIENTID = MentionUtils.ParseUser(args[0]);
                        string[] listofslaps = { $"<@{Context.User.Id}> just slapped {Program._client.GetUser(CLIENTID).Username}!", $"<@{Context.User.Id}> slaps {Program._client.GetUser(CLIENTID).Username} around with a large trout!" };
                        Random rand = new Random();
                        int index = rand.Next(listofslaps.Length);
                        await ReplyAsync($"{listofslaps[index]}");
                    }
                    else
                    {
                        await ReplyAsync("You need to mention someone to slap them!");
                    }
                }
            }
        }
        [Command("joke")]
        [Alias("dadjoke")]
        [Summary("Tells a dad joke!")]
        private async Task joke(params string[] args)
        {
            HttpResponse<string> response = Unirest.get("https://icanhazdadjoke.com/")
            .header("User-Agent", "MangoBot https://github.com/lXxMangoxXl/MangoBot")
            .header("Accept", "text/plain")
            .asString();
            await ReplyAsync(response.Body.ToString());
        }
        [Command("todo")]
        private async Task todo(params string[] args)
        {
            await ReplyAsync("**1.** Edit command handler for better error messages. **FIXED**\n" +
                "**2.** Add ban command. *Work in progress.*\n" +
                "**3.** Add ^modhelp or the sort for listing staff commands\n" +
                "**4.** Rework penis command code.");
        }
        [Command("avatar")]
        [Alias("pfp")]
        private async Task avatar(params string[] args)
        {
            if (args.Length == 0)
            {
                string avatarurl = Context.User.GetAvatarUrl();
                await ReplyAsync($"Heres your avatar! {avatarurl}");
            }
            if (args.Length == 1)
            {
                if (args[0].Contains("@everyone") | args[0].Contains("@here"))
                {
                    await ReplyAsync("Tsk Tsk");
                }
                else
                {
                    if (args[0].Contains("@"))
                    {
                        ulong userid = MentionUtils.ParseUser(args[0]);
                        SocketUser user = Program._client.GetUser(userid);
                        if (user.GetAvatarUrl() == null)
                        {
                            await ReplyAsync($"{Program._client.GetUser(userid).Username}'s avatar is {user.GetDefaultAvatarUrl()}");
                        }
                        else
                        {
                            await ReplyAsync($"{Program._client.GetUser(userid).Username}'s avatar is {user.GetAvatarUrl()}");
                        }
                    }
                    else
                    {
                        await ReplyAsync("You have to mention someone to get their avatar!");
                    }
                }
            }
            if (args.Length == 2 | args.Length > 2)
            {
                await ReplyAsync("Mention only one user!");
            }
        }
        [Command("defaultavatar")]
        [Alias("defaultpfp")]
        private async Task defaultavatar(params string[] args)
        {
            if (args.Length == 0)
            {
                await ReplyAsync($"Heres your default avatar! {Context.User.GetDefaultAvatarUrl()}");
            }
            if (args.Length == 1)
            {
                if (args[0].Contains("@everyone") | args[0].Contains("@here"))
                {
                    await ReplyAsync("Tsk Tsk");
                }
                else
                {
                    if (args[0].Contains("@"))
                    {
                        ulong userid = MentionUtils.ParseUser(args[0]);
                        SocketUser user = Program._client.GetUser(userid);
                        await ReplyAsync($"{Program._client.GetUser(userid).Username}'s avatar is {user.GetDefaultAvatarUrl()}");
                    }
                    else
                    {
                        await ReplyAsync("You have to mention someone to get their avatar!");
                    }
                }
            }
            if (args.Length == 2 | args.Length > 2)
            {
                await ReplyAsync("Mention only one user!");
            }
        }
        [Command("say")]
        [RequireOwner]
        private async Task say([Remainder] string args)
        {
            if (!Context.IsPrivate && Context.Guild.CurrentUser.GuildPermissions.ManageMessages == true)
            {
                await Context.Message.DeleteAsync();
            }
            await ReplyAsync(args);
        }

        [Command("about")]
        private async Task about()
        {
            await ReplyAsync("Made by lXxMangoxXl#8878\n" +
                "https://github.com/lXxMangoxXl/MangoBot/ \n" +
                "Made with Discord.Net, C#, and lots of love!");
        }
        [Command("minecraft")]
        private async Task minecraft()
        {
            //If you're in Unlimited, send the game address, if you aren't, spit out a generic error message.
            if (!Context.IsPrivate && Context.Guild.Id == 687875961995132973)
            {
                await ReplyAsync("The server IP is `mc.unlimitedscp.com`");
            }
        }
        [Command("appeal")]
        [Alias("appeals")]
        private async Task appeal()
        {
            //If you're in Unlimited, send the invite.
            if (!Context.IsPrivate && Context.Guild.Id == 687875961995132973)
            {
                await ReplyAsync($"The appeal URL is http://appeal.unlimitedscp.com");
            }
        }
        [Command("disablepenis")]
        private async Task disablepenis()
        {
            //label the config as a string.
            string text = File.ReadAllText("config.json");

            //Prepare the config.disabledpenis
            config = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("config.json"));
            string disabledpenisstringg = config.disabledpenis;

            //Checks to see if your userid is either Mango's or River's
            if (Context.User.Id == 287778194977980416 | Context.User.Id == 706468176707190845)
            {
                //If it's currently disabled
                if (config.disabledpenis == "1")
                {
                    //replace the found text with the other text
                    text = text.Replace("\"disabledpenis\": \"1\"", "\"disabledpenis\": \"2\"");
                    //write it
                    File.WriteAllText("config.json", text);
                    //reply with a basic response
                    await ReplyAsync("Penis commands are now *enabled*");
                }
                //if it's not currently disabled
                else
                {
                    //replace the found text with the other text
                    text = text.Replace("\"disabledpenis\": \"2\"", "\"disabledpenis\": \"1\"");
                    //write it
                    File.WriteAllText("config.json", text);
                    //reply with a basic response
                    await ReplyAsync("Penis commands are now *disabled*");
                }
            }
            //If you aren't Mango or River
            else
            {
                await ReplyAsync("You have to be either River or Mango to execute this command!");
            }
        }
        [Command("brazil")]
        [Summary("Send someone to brazil.")]
        private async Task brazil()
        {
            await ReplyAsync("https://media1.tenor.com/images/d632412aaffe388de314b7abff9c408e/tenor.gif?itemid=17781004");
        }
        [Command("purge")]
        [Summary("purges X messages")]
        [RequireUserPermission(GuildPermission.ManageMessages, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        private async Task purge(int args)
        {
            if(!Context.IsPrivate && Context.Guild.CurrentUser.GuildPermissions.ManageMessages == true)
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(args + 1).FlattenAsync();
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
                const int delay = 3000;
                IUserMessage m = await ReplyAsync($"I have deleted {args} messages for ya. :)");
                await Task.Delay(delay);
                await m.DeleteAsync();
            }
            else
            {
                await ReplyAsync("You either, need to give me Manage Messages in this server before I can run this command, or you are in a DM!");
            }
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, Group = "Permissions")]
        [RequireOwner(Group = "Permissions")]
        private async Task ban(SocketGuildUser usertobehammered, [Remainder]string banre)
        {
            if(!Context.IsPrivate && Context.Guild.CurrentUser.GuildPermissions.BanMembers == true)
            {
                var serverid = Context.Guild.Id;
                string servername = Context.Guild.Name;
                var bannedfool = usertobehammered;

                if (!Context.IsPrivate && Context.Guild.Id == 687875961995132973)
                {
                    var rUser = Context.User as SocketGuildUser;

                    await ReplyAsync($"User {usertobehammered.Mention} has been banned.");
                    await bannedfool.SendMessageAsync($"You've been banned from {Context.Guild.Name}. Please visit http://appeal.unlimitedscp.com to appeal your ban.");
                    await Context.Guild.AddBanAsync(usertobehammered, 0, banre);
                }
                else
                {
                    var rUser = Context.User as SocketGuildUser;

                    await ReplyAsync($"User {usertobehammered.Mention} has been banned.");
                    await bannedfool.SendMessageAsync($"You've been banned from {Context.Guild.Name}.");
                    await Context.Guild.AddBanAsync(usertobehammered, 0, banre);
                }
            }
            else
            {
                if(!Context.IsPrivate && Context.Guild.CurrentUser.GuildPermissions.ManageMessages == true)
                {
                    await Context.Message.DeleteAsync();
                }
                await ReplyAsync("You need to give me Ban Members in this server before I can run this command!");
            }
        }
        [Command("bann")]
        private async Task bann(SocketGuildUser usertobehammered, [Remainder] string banre)
        {
            var serverid = Context.Guild.Id;
            string servername = Context.Guild.Name;
            var bannedfool = usertobehammered;

            if(String.IsNullOrEmpty(banre))
            {
                await ReplyAsync($"Banned {bannedfool.Nickname}!");
            }
            else
            {
                await ReplyAsync($"Banned {bannedfool.Nickname} for {banre}!");
            }
        }
    }
}
