using System.Net.Mail;
using System.Net;

namespace TatBlog.WebApi.Extensions;

public static class SendMailExtensions
{
	private static IWebHostEnvironment _env;

	public static void Initialize(IWebHostEnvironment env)
	{
		_env = env;
	}

	public static string SetTemplateEmail(string path)
	{
		string wwwrootPath = _env.WebRootPath;
		string filePath = Path.Combine(wwwrootPath, path);
		string fileContents = System.IO.File.ReadAllText(filePath);
		return fileContents;
	}

	public static void SendEmail(string recipient, string subject, string body)
	{
		try
		{
			string fromEmail = "noreply.email.dluconfession@gmail.com";
			string fromPassword = "upngspyyxjhrpjqd";
			string smtpServer = "smtp.gmail.com";
			int smtpPort = 587;

			MailMessage message = new MailMessage(fromEmail, recipient, subject, body);
			message.IsBodyHtml = true;
			SmtpClient client = new SmtpClient(smtpServer, smtpPort);

			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(fromEmail, fromPassword);
			client.EnableSsl = true;


			client.Send(message);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error sending email: " + ex.Message);
		}
	}

}