namespace ArucoTest
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cameraButton = new System.Windows.Forms.Button();
            this.cameraImageBox = new Emgu.CV.UI.ImageBox();
            this.lb_result = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cameraImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cameraButton
            // 
            this.cameraButton.Location = new System.Drawing.Point(155, 8);
            this.cameraButton.Margin = new System.Windows.Forms.Padding(2);
            this.cameraButton.Name = "cameraButton";
            this.cameraButton.Size = new System.Drawing.Size(204, 27);
            this.cameraButton.TabIndex = 1;
            this.cameraButton.Text = "Open Camera";
            this.cameraButton.UseVisualStyleBackColor = true;
            this.cameraButton.Click += new System.EventHandler(this.cameraButton_Click);
            // 
            // cameraImageBox
            // 
            this.cameraImageBox.Location = new System.Drawing.Point(11, 39);
            this.cameraImageBox.Margin = new System.Windows.Forms.Padding(2);
            this.cameraImageBox.Name = "cameraImageBox";
            this.cameraImageBox.Size = new System.Drawing.Size(868, 500);
            this.cameraImageBox.TabIndex = 2;
            this.cameraImageBox.TabStop = false;
            // 
            // lb_result
            // 
            this.lb_result.AutoSize = true;
            this.lb_result.Location = new System.Drawing.Point(421, 13);
            this.lb_result.Name = "lb_result";
            this.lb_result.Size = new System.Drawing.Size(35, 13);
            this.lb_result.TabIndex = 3;
            this.lb_result.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 573);
            this.Controls.Add(this.lb_result);
            this.Controls.Add(this.cameraImageBox);
            this.Controls.Add(this.cameraButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Aruco demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cameraButton;
        private Emgu.CV.UI.ImageBox cameraImageBox;
        private System.Windows.Forms.Label lb_result;
    }
}

