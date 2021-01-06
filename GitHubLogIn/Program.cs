using System;
using System.Collections.Generic;
using System.Linq;


using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;


using System.Net.Http;
using System.Net.Http.Headers;


namespace GitHubLogIn
{
    public class Program
    {

        // HTTP Client
       // static HttpClient client = new HttpClient();



        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

   //         Task.WaitAll(Main1());
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //Task.WaitAll(ExecuteAsync());


        }


       
    }
}
