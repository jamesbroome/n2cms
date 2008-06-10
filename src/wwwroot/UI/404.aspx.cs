﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace N2.Templates.UI
{
	public class _404 : Page
	{
		protected override void OnInit(EventArgs args)
		{
			Response.Status = "404 Not Found";

			try
			{
				N2.ContentItem page = N2.Templates.Find.StartPage.NotFoundPage;
				if (page != null)
				{
					Server.Execute(page.RewrittenUrl);
					return;
				}
			}
			catch
			{
			}
			Response.Write("<html><body><h1>404 Not Found</h1></body></html>");
		}
	}
}