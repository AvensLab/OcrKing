namespace AvensLab.ServiceDemoGui
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbNetFile = new System.Windows.Forms.GroupBox();
            this.btNetFile = new System.Windows.Forms.Button();
            this.lbNetFile = new System.Windows.Forms.Label();
            this.tbNetFile = new System.Windows.Forms.TextBox();
            this.gbUpload = new System.Windows.Forms.GroupBox();
            this.btLocalFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLocalFile = new System.Windows.Forms.TextBox();
            this.gbImage = new System.Windows.Forms.GroupBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.pbDesFile = new System.Windows.Forms.PictureBox();
            this.pbSrcFile = new System.Windows.Forms.PictureBox();
            this.gbNetFile.SuspendLayout();
            this.gbUpload.SuspendLayout();
            this.gbImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDesFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSrcFile)).BeginInit();
            this.SuspendLayout();
            // 
            // gbNetFile
            // 
            this.gbNetFile.Controls.Add(this.btNetFile);
            this.gbNetFile.Controls.Add(this.lbNetFile);
            this.gbNetFile.Controls.Add(this.tbNetFile);
            this.gbNetFile.Location = new System.Drawing.Point(13, 13);
            this.gbNetFile.Name = "gbNetFile";
            this.gbNetFile.Size = new System.Drawing.Size(635, 82);
            this.gbNetFile.TabIndex = 0;
            this.gbNetFile.TabStop = false;
            this.gbNetFile.Text = "网络文件识别";
            // 
            // btNetFile
            // 
            this.btNetFile.Location = new System.Drawing.Point(527, 30);
            this.btNetFile.Name = "btNetFile";
            this.btNetFile.Size = new System.Drawing.Size(75, 23);
            this.btNetFile.TabIndex = 2;
            this.btNetFile.Text = "开始识别";
            this.btNetFile.UseVisualStyleBackColor = true;
            this.btNetFile.Click += new System.EventHandler(this.btNetFile_Click);
            // 
            // lbNetFile
            // 
            this.lbNetFile.AutoSize = true;
            this.lbNetFile.Location = new System.Drawing.Point(16, 35);
            this.lbNetFile.Name = "lbNetFile";
            this.lbNetFile.Size = new System.Drawing.Size(77, 12);
            this.lbNetFile.TabIndex = 1;
            this.lbNetFile.Text = "验证码地址：";
            // 
            // tbNetFile
            // 
            this.tbNetFile.Location = new System.Drawing.Point(99, 32);
            this.tbNetFile.Name = "tbNetFile";
            this.tbNetFile.Size = new System.Drawing.Size(408, 21);
            this.tbNetFile.TabIndex = 0;
            this.tbNetFile.Text = "https://www.bestpay.com.cn/api/captcha/getCode?1408294248050";
            // 
            // gbUpload
            // 
            this.gbUpload.Controls.Add(this.btLocalFile);
            this.gbUpload.Controls.Add(this.label1);
            this.gbUpload.Controls.Add(this.tbLocalFile);
            this.gbUpload.Location = new System.Drawing.Point(13, 109);
            this.gbUpload.Name = "gbUpload";
            this.gbUpload.Size = new System.Drawing.Size(635, 82);
            this.gbUpload.TabIndex = 1;
            this.gbUpload.TabStop = false;
            this.gbUpload.Text = "上传文件识别";
            // 
            // btLocalFile
            // 
            this.btLocalFile.Location = new System.Drawing.Point(527, 30);
            this.btLocalFile.Name = "btLocalFile";
            this.btLocalFile.Size = new System.Drawing.Size(75, 23);
            this.btLocalFile.TabIndex = 2;
            this.btLocalFile.Text = "选择并识别";
            this.btLocalFile.UseVisualStyleBackColor = true;
            this.btLocalFile.Click += new System.EventHandler(this.btLocalFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "文件路径：";
            // 
            // tbLocalFile
            // 
            this.tbLocalFile.Location = new System.Drawing.Point(99, 32);
            this.tbLocalFile.Name = "tbLocalFile";
            this.tbLocalFile.Size = new System.Drawing.Size(408, 21);
            this.tbLocalFile.TabIndex = 0;
            // 
            // gbImage
            // 
            this.gbImage.Controls.Add(this.tbResult);
            this.gbImage.Controls.Add(this.pbDesFile);
            this.gbImage.Controls.Add(this.pbSrcFile);
            this.gbImage.Location = new System.Drawing.Point(13, 204);
            this.gbImage.Name = "gbImage";
            this.gbImage.Size = new System.Drawing.Size(635, 84);
            this.gbImage.TabIndex = 2;
            this.gbImage.TabStop = false;
            this.gbImage.Text = "图片显示";
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(261, 35);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(100, 21);
            this.tbResult.TabIndex = 2;
            // 
            // pbDesFile
            // 
            this.pbDesFile.Location = new System.Drawing.Point(402, 20);
            this.pbDesFile.Name = "pbDesFile";
            this.pbDesFile.Size = new System.Drawing.Size(200, 50);
            this.pbDesFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDesFile.TabIndex = 1;
            this.pbDesFile.TabStop = false;
            // 
            // pbSrcFile
            // 
            this.pbSrcFile.Location = new System.Drawing.Point(18, 20);
            this.pbSrcFile.Name = "pbSrcFile";
            this.pbSrcFile.Size = new System.Drawing.Size(200, 50);
            this.pbSrcFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSrcFile.TabIndex = 0;
            this.pbSrcFile.TabStop = false;
            this.pbSrcFile.Click += new System.EventHandler(this.pbSrcFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 300);
            this.Controls.Add(this.gbImage);
            this.Controls.Add(this.gbUpload);
            this.Controls.Add(this.gbNetFile);
            this.Name = "MainForm";
            this.Text = "OcrKing使用Demo";
            this.gbNetFile.ResumeLayout(false);
            this.gbNetFile.PerformLayout();
            this.gbUpload.ResumeLayout(false);
            this.gbUpload.PerformLayout();
            this.gbImage.ResumeLayout(false);
            this.gbImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDesFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSrcFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbNetFile;
        private System.Windows.Forms.Label lbNetFile;
        private System.Windows.Forms.TextBox tbNetFile;
        private System.Windows.Forms.Button btNetFile;
        private System.Windows.Forms.GroupBox gbUpload;
        private System.Windows.Forms.Button btLocalFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLocalFile;
        private System.Windows.Forms.GroupBox gbImage;
        private System.Windows.Forms.PictureBox pbDesFile;
        private System.Windows.Forms.PictureBox pbSrcFile;
        private System.Windows.Forms.TextBox tbResult;
    }
}

