using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmailUs : ContentPage
	{
		public EmailUs ()
		{
			InitializeComponent ();
            Title = "Email";
		}

        private void btnSend_Clicked(object sender, EventArgs e)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("naiveplus2@gmail.com");
                mail.To.Add(txtTo.Text);
                mail.Subject = txtSubject.Text;
                mail.Body = txtBody.Text;

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("naiveplus2@gmail.com", "naive123");

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                DisplayAlert("Failed", ex.Message, "OK");
            }
        }
    }
}