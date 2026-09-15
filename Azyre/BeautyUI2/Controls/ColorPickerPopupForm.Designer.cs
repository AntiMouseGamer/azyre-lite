namespace BeautyUI2.Controls
{
	// Token: 0x02000013 RID: 19
	internal partial class ColorPickerPopupForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00006778 File Offset: 0x00004978
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._fadeTimer != null)
			{
				this._fadeTimer.Stop();
				this._fadeTimer.Tick -= new global::System.EventHandler(this.FadeTimer_Tick);
				this._fadeTimer.Dispose();
				this._fadeTimer = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000092 RID: 146
		private global::System.Windows.Forms.Timer _fadeTimer;
	}
}
