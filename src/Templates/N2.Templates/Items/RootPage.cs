using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using N2.Details;
using N2.Integrity;
using System.Net.Mail;

namespace N2.Templates.Items
{
	[Definition("Root Page", "RootPage", "A root page used to organize start pages.", "", 0, MayBeRoot = true)]
	[RestrictParents(AllowedTypes.None)]
	[AvailableZone("Left", "Left")]
	[AvailableZone("Center", "Center")]
	[AvailableZone("Right", "Right")]
	[AvailableZone("Above", "Above")]
	[AvailableZone("Below", "Below")]
	[N2.Web.UI.TabPanel("smtp", "Smtp settings", 30)]
	public class RootPage : AbstractContentPage, N2.Web.ISitesProvider
	{
		public override string TemplateUrl
		{
			get { return "~/Secured/Root.aspx"; }
		}

		public override string IconUrl
		{
			get
			{
				return "~/Img/page_gear.png";
			}
		}

		public override string Url
		{
			get
			{
				return RewrittenUrl;
			}
		}

		#region ISitesProvider Members

		public ICollection<N2.Web.Site> GetSites()
		{
			List<N2.Web.Site> sites = new List<N2.Web.Site>();
			foreach (StartPage page in GetChildren<StartPage>())
				sites.Add(new N2.Web.Site(this.ID, page.ID, page.HostName));
			return sites;
		}

		#endregion


		[EditableTextBox("Smtp Host", 100, ContainerName = "smtp")]
		public virtual string SmtpHost
		{
			get { return (string)(GetDetail("SmtpHost") ?? string.Empty); }
			set { SetDetail("SmtpHost", value, string.Empty); }
		}

		[EditableTextBox("Smtp Port", 110, ContainerName = "smtp")]
		public virtual int SmtpPort
		{
			get { return (int)(GetDetail("SmtpPort") ?? 25); }
			set { SetDetail("SmtpPort", value, 25); }
		}

		[EditableTextBox("Smtp User", 100, ContainerName = "smtp")]
		public virtual string SmtpUser
		{
			get { return (string)(GetDetail("SmtpUser") ?? string.Empty); }
			set { SetDetail("SmtpUser", value, string.Empty); }
		}

		[EditableTextBox("Smtp Password", 100, ContainerName = "smtp")]
		public virtual string SmtpPassword
		{
			get { return (string)(GetDetail("SmtpPassword") ?? string.Empty); }
			set { SetDetail("SmtpPassword", value, string.Empty); }
		}

		public virtual SmtpClient GetSmtpClient()
		{
			SmtpClient sc = new SmtpClient(SmtpHost, SmtpPort);
			if (SmtpUser.Length > 0)
			{
				sc.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
			}
			return sc;
		}
	}
}