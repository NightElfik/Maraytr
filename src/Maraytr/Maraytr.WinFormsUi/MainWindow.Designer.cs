namespace Maraytr.WinFormsUi {
	partial class frmMainWindow {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainWindow));
			this.btnRender = new System.Windows.Forms.Button();
			this.pbImage = new System.Windows.Forms.PictureBox();
			this.pnlScroll = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
			this.pnlScroll.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnRender
			// 
			this.btnRender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRender.Location = new System.Drawing.Point(12, 12);
			this.btnRender.Name = "btnRender";
			this.btnRender.Size = new System.Drawing.Size(559, 23);
			this.btnRender.TabIndex = 0;
			this.btnRender.Text = "Render";
			this.btnRender.UseVisualStyleBackColor = true;
			this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
			// 
			// pbImage
			// 
			this.pbImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbImage.BackgroundImage")));
			this.pbImage.Location = new System.Drawing.Point(0, 0);
			this.pbImage.Name = "pbImage";
			this.pbImage.Size = new System.Drawing.Size(640, 480);
			this.pbImage.TabIndex = 1;
			this.pbImage.TabStop = false;
			this.pbImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseClick);
			this.pbImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbImage_MouseMove);
			// 
			// pnlScroll
			// 
			this.pnlScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlScroll.AutoScroll = true;
			this.pnlScroll.Controls.Add(this.pbImage);
			this.pnlScroll.Location = new System.Drawing.Point(12, 41);
			this.pnlScroll.Name = "pnlScroll";
			this.pnlScroll.Size = new System.Drawing.Size(640, 480);
			this.pnlScroll.TabIndex = 2;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(577, 12);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Save ...";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "png";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 524);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(664, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tsslStatus
			// 
			this.tsslStatus.Name = "tsslStatus";
			this.tsslStatus.Size = new System.Drawing.Size(61, 17);
			this.tsslStatus.Text = "Status text";
			// 
			// frmMainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(664, 546);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.pnlScroll);
			this.Controls.Add(this.btnRender);
			this.Name = "frmMainWindow";
			this.Text = "Maraytr – Marek\'s ray traycer";
			((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
			this.pnlScroll.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRender;
		private System.Windows.Forms.PictureBox pbImage;
		private System.Windows.Forms.Panel pnlScroll;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
	}
}

