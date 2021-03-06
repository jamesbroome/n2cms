﻿using System.Diagnostics;
using System.Linq;
using N2.Linq;
using NUnit.Framework;

namespace N2.Extensions.Tests.Linq
{
	/// <summary>
	/// these tests asserts nhibernate.linq's built-in capabilities
	/// </summary>
	[TestFixture]
//	[Ignore]
	public class VanillaLinqingTests : LinqTestsBase
	{
		[Test]
		public void CanSelect_Count()
		{
			var query = from ci in engine.QueryItems<LinqItem>()
						select ci;

			int count = query.Count();

			Assert.That(count, Is.EqualTo(2));
		}

		[Test]
		public void CanSelect_AllItems()
		{
			var query = from ci in engine.QueryItems<LinqItem>()
						select ci;

			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(2));
			Assert.That(items.Contains(root));
			Assert.That(items.Contains(item));
		}

		[Test]
		public void CanSelect_Where_Name_Equals()
		{
			var query = from ci in engine.QueryItems<LinqItem>()
						where ci.Name == "root"
						select ci;

			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items[0], Is.EqualTo(root));
		}

		//Expr: value(NHibernate.Linq.Query`1[N2.ContentItem]).Where(ci => ci.Details.Values.OfType().Any(sd => ((sd.Name = "StringProperty2") && (sd.StringValue = "another string"))))
		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByNameAndValue()
		{
			var query = engine.QueryItems()
				.Where(ci => ci.Details
					.Any(sd => sd.Name == "StringProperty2" && sd.StringValue == "another string"));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}

		//Expr: value(NHibernate.Linq.NhQueryable`1[N2.ContentItem]).Where(i => (i.VersionOf == null)).Where(ci => ci.Details.Any(sd => ((sd.Name == "IntProperty") AndAlso (sd.IntValue == Convert(123)))))
		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByName_AndNullableValue()
		{
			var query = engine.QueryItems()
				.Where(ci => ci.Details
					.Any(sd => sd.Name == "IntProperty" && sd.IntValue == 123));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}

		//Expr: value(NHibernate.Linq.Query`1[N2.ContentItem]).Where(ci => ci.Details.Values.OfType().Any(sd => ((sd.Name = "StringProperty2") && sd.StringValue.StartsWith("another"))))
		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByName_And_StartsWithValue()
		{
			var query = engine.QueryItems()
				.Where(ci => ci.Details
					.Any(sd => sd.Name == "StringProperty2" && sd.StringValue.StartsWith("another")));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}

		//Expr: value(NHibernate.Linq.Query`1[N2.Extensions.Tests.Linq.LinqItem]).Where(ci => ci.Details.Values.OfType().Any(cd => ((cd.Name = "StringProperty2") && (cd.StringValue = "another string"))))
		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByNameAndValue_StronglyTypedQuery()
		{
			var query = engine.QueryItems<LinqItem>()
				.Where(ci => ci.Details
					.Any(cd => cd.Name == "StringProperty2" && cd.StringValue == "another string"));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}

		//Expr: value(NHibernate.Linq.Query`1[N2.Extensions.Tests.Linq.LinqItem]).Where(ci => ci.Details.Values.OfType().Any(cd => ((cd.Name = "StringProperty2") && cd.StringValue.StartsWith("another"))))
		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByNameAndValue_StartsWith()
		{
			var query = engine.QueryItems<LinqItem>()
				.Where(ci => ci.Details
					.Any(cd => cd.Name == "StringProperty2" && cd.StringValue.StartsWith("another")));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}


		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByNameAndValue_OnTwoDetails()
		{
			var query = engine.QueryItems<LinqItem>()
				.Where(ci => ci.Details
					.Any(cd => cd.Name == "StringProperty" && cd.StringValue == "a string"))
				.Where(ci => ci.Details
					.Any(cd => cd.Name == "StringProperty2" && cd.StringValue == "another string"));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}

		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByNameAndValue_And_NameAndValue()
		{
			var query = engine.QueryItems<LinqItem>()
				.Where(ci => ci.Details.Any(cd => cd.Name == "StringProperty" && cd.StringValue == "a string")
					&& ci.Details.Any(cd => cd.Name == "StringProperty2" && cd.StringValue == "another string"));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(1));
			Assert.That(items.Contains(root));
			Assert.That(!items.Contains(item));
		}

		[Test]
		public void CanSelect_SingleItem_BySubselectingDetail_ByNameAndValue_Or_NameAndValue()
		{
			var query = engine.QueryItems<LinqItem>()
				.Where(ci => ci.Details.Any(cd => cd.Name == "StringProperty2" && cd.StringValue == "another string")
					|| ci.Details.Any(cd => cd.Name == "StringProperty2" && cd.StringValue == "yet another string"));

			Debug.WriteLine(query.Expression);
			var items = query.ToList();

			Assert.That(items.Count, Is.EqualTo(2));
			Assert.That(items.Contains(root));
			Assert.That(items.Contains(item));
		}

		[Test]
		public void CanSkipAndTake()
		{
			var query = engine.QueryItems<LinqItem>().OrderBy(ci => ci.ID).Skip(1).Take(1);

			var items = query.ToList();

			Assert.That(items.Single(), Is.EqualTo(item));
		}
	}
}
