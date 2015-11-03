using System;

namespace SplendidCRM
{
	class KeySortDropDownList : System.Web.UI.WebControls.DropDownList
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.Attributes.Add("onkeypress", "return KeySortDropDownList_onkeypress(this, false)");
			this.Attributes.Add("onkeydown" , "if (window.event.keyCode == 13||window.event.keyCode == 9||window.event.keyCode == 27){this.fireEvent('onChange');onchangefired=true;}");
			this.Attributes.Add("onclick"   , "if (this.selectedIndex!=" + this.SelectedIndex + " && onchangefired==false) {this.fireEvent('onChange');onchangefired=true;}");
			this.Attributes.Add("onblur"    , "if (this.selectedIndex!=" + this.SelectedIndex + " && onchangefired==false) {this.fireEvent('onChange')}");
		}
	}
}

