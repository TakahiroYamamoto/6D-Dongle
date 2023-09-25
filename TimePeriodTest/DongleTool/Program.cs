using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongleTool
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            string name = ci.Name;
            if (name != "ja-JP")
                name = "en-US";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(name, false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DongleToolForm());
        }
    }
}
