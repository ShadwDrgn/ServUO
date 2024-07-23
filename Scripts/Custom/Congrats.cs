using System;
using Server;
using Server.Mobiles;
using System.Net.Http;
using System.Text;

namespace Server.Misc
{
    public class Congrats
    {
        private static HttpClient client = new HttpClient();
        private static string url = "https://discord.com/api/webhooks/1265008799182360697/sgAjQks_Ef3hX8zWgWZeRTj5pRFXBebMFBPFr2cVCsGUjQCLarIoRPKo52oso7lCl20d";
        public static void Initialize()
        {
            // Register our event handler
            EventSink.SkillGain += new SkillGainEventHandler(EventSink_SkillGain);
        }
        private static void EventSink_SkillGain(SkillGainEventArgs e)
        {
	    string content = "";
            if (e.From is PlayerMobile)
            {
                double oldSkill = e.Skill.Base - ((double)e.Gained/10);
                if (e.Skill.Base >= 100 && oldSkill < 100)
                {
                    content = String.Format("{0} has become a GrandMaster {1}", e.From.Name, e.Skill.Info.Title);
		    var json = "{\"content\": \"" + content + "\"}";
		    var msg = new StringContent(json, Encoding.UTF8, "application/json");
		    var response = client.PostAsync(url, msg);
                    Console.WriteLine(json);

                }
                if (e.Skill.Base >= 120 && oldSkill < 120)
                {
                    content = String.Format("{0} has become a Legendary {1}", e.From.Name, e.Skill.Info.Title);
		    var json = "{\"content\": \"" + content + "\"}";
		    var msg = new StringContent(json, Encoding.UTF8, "application/json");
		    var response = client.PostAsync(url, msg);
                }
            }
        }
    }
}

