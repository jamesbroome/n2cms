using System.Web.UI;
using System.Web.UI.WebControls;

namespace N2.Details
{
	/// <summary>An editable checkbox attribute. Besides creating a checkbox it also uses the checkbox's text property to display text.</summary>
	public class EditableCheckBox : EditableAttribute
	{
		private string checkBoxText;

		/// <summary>Creates a new instance of the checkbox editable attribute.</summary>
		/// <param name="checkBoxText">The text on the checkbox.</param>
		/// <param name="sortOrder">The order of this editable checkbox.</param>
		public EditableCheckBox(string checkBoxText, int sortOrder)
			: base(string.Empty, typeof (CheckBox), "Checked", sortOrder)
		{
			CheckBoxText = checkBoxText;
		}

		/// <summary>Gets or sets the text on the checkbox. This differs from the title property since the text is after the checkbox.</summary>
		public string CheckBoxText
		{
			get { return checkBoxText; }
			set { checkBoxText = value; }
		}

		/// <summary>Creates a checkbox.</summary>
		/// <param name="container">The container the checkbox will be added to.</param>
		/// <returns>A checkbox.</returns>
		protected override Control CreateEditor(Control container)
		{
			CheckBox cb = new CheckBox();
			cb.Text = GetLocalizedText("CheckBoxText") ?? CheckBoxText;
			return cb;
		}
	}
}