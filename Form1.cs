using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnonUp
{
    public partial class Form1 : Form
    {
        public String ret="";
        private bool done = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                String[] args = Environment.GetCommandLineArgs();
                if(args.Count()>1)
                {
                    String file = args[1];
                    Task.Run(async () => await UploadFile(file));
                    ret = getURL(ret);
                    
                }
            }
            catch (Exception ex)
            { }
        }

        private String getURL(String ret)
        {            
            Regex regex = new Regex("(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?");
            Match match = regex.Match(ret);

            if (match.Success)
            {
                return match.Value;
            }
            return "";
        }

        public async Task<string> UploadFile(string filePath)
        {
            String s = "ERROR";
            try
            {
               
                var form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "file", Path.GetFileName(filePath));

                HttpClient htt = new HttpClient();
                var response = htt.PostAsync("https://api.anonfiles.com/upload", form);

                response.Wait();
                s = await response.Result.Content.ReadAsStringAsync();
                ret = s;
            }
            catch (Exception ex) { };
            try
            {
                textBox1.Text = getURL(ret);
                //Console.WriteLine(getURL(ret));
                // Clipboard.Clear();
                Clipboard.SetText(getURL(ret));
                label1.Text = "URL sent to clipboard.";
                
                //System.Windows.Forms.Clipboard.SetText(getURL(ret));
            } catch (Exception ex1) { };
            done = true;
            return s;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (done)
            {
                Application.Exit();
                this.Close();
            }
        }
    }
}
