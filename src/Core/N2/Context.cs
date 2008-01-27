#region License
/* Copyright (C) 2007 Cristian Libardo
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 */
#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;

namespace N2
{
    /// <summary>
	/// Provides access to the singleton instance of the N2 CMS engine.
	/// </summary>
    public class Context
    {
		#region Private Fields
		private static Engine.IEngine instance = null;
		#endregion

		#region Initialization Methods
    	/// <summary>Initializes a static instance of the N2 factory.</summary>
		/// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void Initialize(bool forceRecreate)
		{
			if (instance == null || forceRecreate)
			{
				instance = CreateEngineInstance();
				instance.InitializePlugIns();
				Debug.WriteLine("Factory.Initialize: Created factory instance.");
			}
			else if (instance != null)
				Trace.TraceInformation("Factory.Initialize: Instance already created");
		}

		/// <summary>Sets the static factory instance to the supplied factory. Use this method to supply your own factory implementation.</summary>
		/// <param name="engine">The factory to use.</param>
		/// <remarks>Only use this method if you know what you're doing.</remarks>
		public static void Initialize(Engine.IEngine engine)
		{
			instance = engine;
		}

		/// <summary>Creates a factory instance and adds a http application injecting facility.</summary>
		/// <returns>A new factory.</returns>
		public static Engine.IEngine CreateEngineInstance()
		{
			return new Engine.CmsEngine();
		}

		#endregion

		#region Properties: Persister, Definitions, Integrity, UrlParser, CurrentPage

		/// <summary>Gets the factory intsance used to hold together N2.</summary>
		public static Engine.IEngine Instance
		{
			get
			{
				if (instance == null)
				{
					Initialize(false);
					instance.Attach(HttpContext.Current.ApplicationInstance);
				}
				return instance;
			}
		}

		/// <summary>Gets N2 persistence manager used for database persistence of content.</summary>
		public static Persistence.IPersister Persister
		{
			get { return Instance.Persister; }
		}

		/// <summary>Gets N2 definition manager</summary>
		public static Definitions.IDefinitionManager Definitions
		{
			get { return Instance.Definitions; }
		}
        
		/// <summary>Gets N2 integrity manager </summary>
		public static Integrity.IIntegrityManager IntegrityManager
        {
			get { return Instance.IntegrityManager; }
        }

		/// <summary>Gets N2 security manager responsible of item access checks.</summary>
		public static Security.ISecurityManager SecurityManager
		{
			get { return Instance.SecurityManager; }
		}

		/// <summary>Gets the url parser responsible of mapping urls to items and back again.</summary>
		public static Web.IUrlParser UrlParser
		{
			get { return Instance.UrlParser; }
		}

        /// <summary>Gets the current page. This is retrieved by the page querystring.</summary>
        public static ContentItem CurrentPage
        {
            get
            {
				HttpContext context = HttpContext.Current;
				if (context != null)
				{
					ContentItem item = context.Items["N2.Factory.CurrentPage"] as ContentItem;

					if (item == null)
					{
						item = GetPageOrStartPage(context);
						context.Items["N2.Factory.CurrentPage"] = item;
					}

					return item;
				}
				return null;
            }
        }

		private static ContentItem GetPageOrStartPage(HttpContext context)
		{
			string page = context.Request["page"];
			if (!string.IsNullOrEmpty(page))
			{
				return Persister.Get(int.Parse(page)) ?? UrlParser.StartPage;
			}
			else
			{
				return UrlParser.StartPage;
			}
		}
        #endregion
	}
}