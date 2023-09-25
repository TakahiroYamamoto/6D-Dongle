using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongleManager
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormMain formMain = new DongleManager.FormMain();
            for( int i = 0; i < args.Length; i++ )
            {
                if (args[i] == "-admin")
                    formMain.SetAdmin();
            }
            Application.Run(formMain);
        }
    }
}
