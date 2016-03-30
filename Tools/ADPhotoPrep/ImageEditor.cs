using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADPhotoPrep
{
	public partial class ImageEditor : UserControl
	{
		public ImageEditor()
		{
			InitializeComponent();
			ViewScale = trackBar1.Value / 100f;
			numericUpDown1.Value = 1;
			numericUpDown2.Value = 1;
			numericUpDown3.Value = 1;

			(panel1 as Control).KeyDown += Editor_KeyDown;
		}

		#region Modifiers and state
		protected float _viewscale = 1.0f;
		public float ViewScale
		{
			get
			{
				return _viewscale;
			}
			set
			{
				var v = Math.Max(0.0f, Math.Min(1.0f, value));
				if (Math.Abs(v - _viewscale) > 0.0001f)
				{
					_viewscale = v;
					_scaledbitmap = null;
					UpdateControl();
					UpdatePreview();
				}
			}
		}
		#endregion

		#region Image members
		private string _filename;

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public string Filename
		{
			get
			{
				if (_sourcebitmap == null)
					return null;
				return string.IsNullOrEmpty(_filename) ? "Untitled.jpg" : _filename;
			}
			private set { _filename = value; }
		}

		private float _imgrotate = 0f;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public float ImageRotation
		{
			get { return _imgrotate; }
			set
			{
				var newval = value % 360;
				if (newval != _imgrotate)
				{
					_imgrotate = newval;
					_sourcebitmap = GetRotatedSource();
					_scaledbitmap = null;
					UpdateControl();
                }
			}
		}

		private Bitmap _truesource = null;
		private Bitmap GetRotatedSource()
		{
			if (_imgrotate == 0 || _truesource == null)
				return _truesource;

            if (((int)Math.Abs(_imgrotate) % 90) == 0)
            {
                int a = (int)_imgrotate;
                while (a < 0)
                    a += 360;
                while (a >= 360)
                    a -= 360;

                int w = a == 180 ? _truesource.Width : _truesource.Height;
                int h = a == 180 ? _truesource.Height : _truesource.Width;

                Bitmap res = _truesource.Clone(new Rectangle(0, 0, _truesource.Width, _truesource.Height), _truesource.PixelFormat);
                var rft = a == 90 ? RotateFlipType.Rotate90FlipNone : a == 180 ? RotateFlipType.Rotate180FlipNone : RotateFlipType.Rotate270FlipNone;
                res.RotateFlip(rft);
                return res;
            }
            else
            {

                Bitmap workbmp = new Bitmap(_truesource.Width * 5, _truesource.Height * 5);
                int dx = (workbmp.Width * 2) / 5;
                int dy = (workbmp.Height * 2) / 5;

                using (var g = Graphics.FromImage(workbmp))
                {
                    float hw = workbmp.Width / 2f;
                    float hh = workbmp.Height / 2f;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    g.TranslateTransform(hw, hh);
                    g.RotateTransform(_imgrotate % 360);
                    g.TranslateTransform(-hw, -hh);
                    g.DrawImage(_truesource, dx, dy);
                }

                workbmp.Save(@"C:\temp\workbmp.bmp");

                int iw = _truesource.Width, ih = _truesource.Height;

                PointF[] rpoints = RotatePoints(
                        new Point[4] { new Point(0, 0), new Point(iw, 0), new Point(iw, ih), new Point(0, ih) },
                        new PointF(iw / 2f, ih / 2f),
                        _imgrotate
                    );
                int nw = (int)(1 + rpoints.Max(p => p.X) - rpoints.Min(p => p.X));
                int nh = (int)(1 + rpoints.Max(p => p.Y) - rpoints.Min(p => p.Y));

                var res = new Bitmap(nw, nh);
                using (var g = Graphics.FromImage(res))
                {
                    float xo = (workbmp.Width - nw) / 2f, yo = (workbmp.Height - nh) / 2f;
                    g.DrawImage(workbmp, -xo, -yo);
                }
                return res;
            }
		}

		private PointF[] RotatePoints(Point[] points, PointF center, float angle)
		{
			PointF[] res = new PointF[points.Length];

			var rads = (angle * Math.PI) / 180d;
			var sin = Math.Sin(rads);
			var cos = Math.Cos(rads);

			for (int pt = 0; pt < points.Length; ++pt)
			{
				var src = points[pt];
				var translated = new PointF(src.X - center.X, src.Y - center.Y);
				var rotated = new PointF(
					(float)(translated.X * cos - translated.Y * sin),
					(float)(translated.X * sin + translated.Y * cos) 
				);
				res[pt] = new PointF(rotated.X + center.X, rotated.Y + center.Y);
			}

			return res;
		}

		private Bitmap _sourcebitmap = null;
		public Bitmap SourceBitmap
		{
			get { return _sourcebitmap; }
			set
			{
				_truesource = value;
				_sourcebitmap = GetRotatedSource();
				_scaledbitmap = null;
				UpdateControl();
			}
		}

		protected Bitmap _scaledbitmap = null;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Bitmap ScaledBitmap
		{
			get
			{
				if (SourceBitmap == null)
					return null;

				if (_scaledbitmap == null)
				{
					Size scaledsize = ScaledImageSize;
					Bitmap res = new Bitmap(scaledsize.Width, scaledsize.Height);
					using (var g = Graphics.FromImage(res))
						g.DrawImage(SourceBitmap, new Rectangle(0, 0, scaledsize.Width, scaledsize.Height));
					_scaledbitmap = res;
				}
				return _scaledbitmap;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Always)]
		public SizeF ScaledImageSizeF
		{
			get
			{
				if (SourceBitmap == null)
					return new SizeF();

				return new SizeF(SourceBitmap.Width * _viewscale, SourceBitmap.Height * _viewscale);
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Size ScaledImageSize
		{
			get
			{
				if (SourceBitmap == null)
					return picturebox.MinimumSize;

				var s = ScaledImageSizeF;
				return new Size((int)(s.Width + 0.5), (int)(s.Height + 0.5));
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Rectangle ScaledImageRect
		{
			get
			{
				if (SourceBitmap == null)
					return new Rectangle();

				return new Rectangle(new Point(), ScaledImageSize);
			}
		}


		#endregion

		#region Selection members
		protected RectangleF _selection = new RectangleF(0, 0, 160, 200);

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public RectangleF Selection
		{
			get { return _selection; }
			set { _selection = value; UpdateControl(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Rectangle ScreenSelection { get { return _selection.ToRectangle(); } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Rectangle SourceSelection
		{
			get
			{
				return new Rectangle(
					(int)(_selection.Left / _viewscale + 0.5),
					(int)(_selection.Top / _viewscale + 0.5),
					(int)(_selection.Width / _viewscale + 0.5),
					(int)(_selection.Height / _viewscale + 0.5));
			}
		}

		protected double _aspectratio = 5 / 4f;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public double AspectRatio
		{
			get { return _aspectratio; }
			set
			{
				if (Math.Abs(value - _aspectratio) < 0.001)
					return;

				if (value < 0.5 || value > 3)
					throw new ArgumentOutOfRangeException("value", string.Format("AspectRatio value must be between 0.5 and 3.0, attempted to set value: {0}", value));

				_aspectratio = value;

				// resize selection around center point
				var center = _selection.Middle();
				var halfh = _selection.Height / 2f;
				var halfw = (float)(halfh / _aspectratio);
				_selection = new RectangleF(center.X - halfw, center.Y - halfh, halfw * 2, halfh * 2);				


				Invalidate();
				picturebox.Invalidate();
				UpdatePreview();
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Size OutputSize
		{
			get
			{
				RectangleF srcrect = SourceSelection;
				int outheight = Math.Min(Properties.Settings.Default.MaxOutputHeight, (int)(srcrect.Height + 0.5));
				return new Size((int)(outheight / AspectRatio + 0.5), outheight);
			}
		}
		#endregion

		#region Drag members
		bool dragging = false;
		int dragmode = 0;
		RectangleF drag_select;
		PointF drag_origin;
		PointF drag_cent;

		static Cursor[] imgcurs = new Cursor[]
		{
			Cursors.Default,
			Cursors.SizeAll,
			Cursors.SizeNWSE
		};

		#endregion

		#region Events & actions
		public event CancelEventHandler Close;

		protected bool RaiseClose()
		{
			if (Close != null)
			{
				var args = new CancelEventArgs();
				Close(this, args);
				if (args.Cancel)
					return false;
			}
			return true;
		}
		#endregion

		static Keys[] modifiers = new Keys[]
			{
				Keys.LShiftKey, Keys.RShiftKey, Keys.ShiftKey,
				Keys.LControlKey, Keys.RControlKey, Keys.ControlKey,
			};

		#region Operations
		public void LoadImage(string filename)
		{
			using (var img = Image.FromFile(filename))
				SetImage(img, filename);
		}

        public void SetImage(Image source, string filename = null)
        {
            SourceBitmap = new Bitmap(source);
            _filename = filename;

            // auto-set initial Zoom to fit height in window
            float zh = Math.Min(1f, Math.Max(0.2f, ((int)((100f * (panel1.ClientRectangle.Height - 6)) / SourceBitmap.Height - 0.5)) / 100f));
            float zw = Math.Min(1f, Math.Max(0.2f, ((int)((100f * (panel1.ClientRectangle.Width - 6)) / SourceBitmap.Width - 0.5)) / 100f));
            float z = Math.Min(zh, zw);
            ViewScale = z;
            trackBar1.Value = (int)(z * 100);

            UpdateControl();
            UpdatePreview();
        }

        public struct SaveResult
		{
			public bool Success;
			public string Filename;
			public Size Dimensions;
			public int Quality;
			public int FileSize;
		}

		public Tuple<byte[], SaveResult> GenerateJPEGData()
		{
			var bmp = GetSelectedImage();
			var img = new BitMiracle.LibJpeg.JpegImage(bmp);
			var parms = new BitMiracle.LibJpeg.CompressionParameters();
			byte[] data = null;

			int q = 100;
            int maxSize = Properties.Settings.Default.MaxFileSize;
			for (; q >= 75 && data == null; q--)
			{
				parms.Quality = q;
				using (var strm = new MemoryStream())
				{
					img.WriteJpeg(strm, parms);
					if (strm.Length < maxSize)
					{
						data = strm.ToArray();
						break;
					}
				}
			}

			if (data == null)
				return Tuple.Create(new byte[0], new SaveResult { Success = false });

			return Tuple.Create(
					data,
					new SaveResult
					{
						Success = true,
						Dimensions = bmp.Size,
						Quality = q,
						FileSize = data.Length
					}
				);
		}

		public byte[] GenerateJPEGBytes()
		{
			var res = GenerateJPEGData();
			if (res.Item2.Success)
				return res.Item1;
			return null;
		}

		public SaveResult SaveImage(string filename = null)
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				var dir = Path.GetDirectoryName(Filename);
				if (string.IsNullOrWhiteSpace(dir))
					dir = @"C:\Temp";
                filename = Path.Combine(dir, Path.GetFileNameWithoutExtension(Filename) + "_Resized.jpg");
            }

			var res = GenerateJPEGData();
			byte[] data = res.Item1;
			SaveResult sr = res.Item2;
			sr.Filename = filename;
			if (sr.Success)
			{
				using (var strm = File.Create(filename))
					strm.Write(data, 0, data.Length);
			}
			return sr;
		}

        public void RotateImage(bool right)
        {

        }

		public void UpdateControl()
		{
			Size s = picturebox.MinimumSize;
			if (SourceBitmap != null)
				s = ScaledImageSize;

			if (picturebox.Image == null || picturebox.Image.Size != s)
				picturebox.Image = new Bitmap(s.Width, s.Height);
			if (picturebox.Size != s)
				picturebox.Size = s;
		}

		public void UpdatePreview()
		{
			previewTimer.Enabled = false;
			previewTimer.Enabled = true;
		}

		protected void internalUpdatePreview()
		{ 
			var img = GetSelectedImage();
			if (previewPB.Width != img.Width)
			{
				panel3.Width = img.Width + panel3.Width - previewPB.Width;
				panel1.Width = ClientSize.Width - panel3.Width;
			}
			if (previewPB.Height != img.Height)
				previewPB.Height = img.Height;

			previewPB.Image = img;
			previewL.Text = string.Format("Size: [{0}, {1}]\nPos:  [{2}, {3}]", img.Size.Width, img.Size.Height, Selection.Left, Selection.Top);
			//previewL.Text = string.Format("Size: [{0}, {1}]\nPos:  [{2}, {3}]\n({4}, {5}, {6})", img.Size.Width, img.Size.Height, Selection.Left, Selection.Top, adjustments.Brightness, adjustments.Contrast, adjustments.Gamma);
		}

		ImageAdjustment adjustments = new ImageAdjustment();
		Bitmap AdjustedSourceImage()
		{
			return adjustments.Adjust(SourceBitmap);
		}

		public Bitmap GetSelectedImage()
		{
			if (SourceBitmap == null)
				return new Bitmap(1, 1);
			var sourceRect = SourceSelection;
			var outputSize = OutputSize;
			var destRect = new Rectangle(new Point(0, 0), outputSize);

			Bitmap res = new Bitmap(outputSize.Width, outputSize.Height);
			using (var g = Graphics.FromImage(res))
			{
				//g.DrawImage(SourceBitmap, destRect, sourceRect, GraphicsUnit.Pixel);
				g.DrawImage(AdjustedSourceImage(), destRect, sourceRect, GraphicsUnit.Pixel);
			}
			return res;
		}

        public void UpdateADThumbnail()
        {
            byte[] data = GenerateJPEGBytes();
            if (data == null)
                return;

            // get A/D name...
            string adname = Path.GetFileNameWithoutExtension(_filename);
            var user = AD.FindUser(adname) ?? AD.FindUser(adname.Replace(".", " "));
            if (user == null && AD.InputBox("Enter Name", "Enter A/D Username to add photo to:", ref adname) == DialogResult.OK)
                user = AD.FindUser(adname);

            if (user == null)
            {
                MessageBox.Show("Unabled to locate user: " + adname);
                return;
            }

#if true
            if (Properties.Settings.Default.SaveBackups)
            {
                string backupFilename = (Properties.Settings.Default.BackupTemplate ?? @"C:\Temp\BAK-{User}_{DateTime}.jpg")
                        .Replace("{User}", adname)
                        .Replace("{DateTime}", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
                try
                {
                    byte[] olddata = (byte[])(user.Properties["ThumbnailPhoto"].Value);
                    if (olddata != null)
                        using (var of = File.Create(backupFilename))
                            of.Write(olddata, 0, olddata.Length);
                }
                catch { }
            }
#endif
            try
            {
                user.Properties["ThumbnailPhoto"].Clear();
                user.Properties["ThumbnailPhoto"].Add(data);
                user.CommitChanges();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
#endregion

        int imageZone(int x, int y)
		{
			Rectangle sel = ScreenSelection;
			int l = sel.Left, t = sel.Top, r = sel.Right, b = sel.Bottom;
			Point p = new Point(x, y);

			Rectangle grab = new Rectangle(sel.Right - 5, sel.Bottom - 5, 11, 11);

			if (grab.Contains(p))
				return 2;
			if (sel.Contains(p))
				return 1;
			return 0;
		}


#region event handlers
		private void Editor_KeyDown(object sender, KeyEventArgs e)
		{
			if (modifiers.Contains(e.KeyCode))
				return;
			//label1.Text = string.Format("{0:X}  {1:X}", e.KeyCode, Keys.ShiftKey | Keys.ControlKey);

			int distance = e.Control ? 10 : 1;
			bool selchanged = false;
			if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract)
			{
				float height = Selection.Height / 2f;
				var c = Selection.Middle();
				if (e.KeyCode == Keys.Subtract)
				{
					height = Math.Max(20, height - distance);
					selchanged = true;
				}
				else if (e.KeyCode == Keys.Add)
				{
					height += distance;
					selchanged = true;
				}
				float width = (float)(height / AspectRatio);
				var s = new SizeF(width * 2f, height * 2f);
				var l = new PointF(c.X - width, c.Y - height);
				Selection = new RectangleF(l, s);
			}
			else if (e.KeyCode == Keys.Left)
			{
				Selection = new RectangleF(new PointF(Selection.Left - distance, Selection.Top), Selection.Size);
				selchanged = true;
			}
			else if (e.KeyCode == Keys.Right)
			{
				Selection = new RectangleF(new PointF(Selection.Left + distance, Selection.Top), Selection.Size);
				selchanged = true;
			}
			else if (e.KeyCode == Keys.Up)
			{
				Selection = new RectangleF(new PointF(Selection.Left, Selection.Top - distance), Selection.Size);
				selchanged = true;
			}
			else if (e.KeyCode == Keys.Down)
			{
				Selection = new RectangleF(new PointF(Selection.Left, Selection.Top + distance), Selection.Size);
				selchanged = true;
			}
            else if (e.KeyCode == Keys.Oemcomma)
            {
                ImageRotation -= 90;
                selchanged = true;
            }
            else if (e.KeyCode == Keys.OemPeriod)
            {
                ImageRotation += 90;
                selchanged = true;
            }

            if (selchanged)
			{
				e.Handled = true;
				picturebox.Invalidate();
				UpdatePreview();
			}
		}

		private void ImageEditor_Resize(object sender, EventArgs e)
		{
			int w = ClientSize.Width - panel3.Width;
			int h = ClientSize.Height - panel2.Height;

			panel1.Size = new Size(w, h);
			picturebox.Size = (SourceBitmap == null) ? new Size(100, 100) : ScaledImageSize;
		}

		private void picturebox_Paint(object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			if (SourceBitmap == null)
			{
				var im = g.InterpolationMode;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
				var sm = g.SmoothingMode;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.DrawLine(Pens.Red, 0, 0, picturebox.Width, picturebox.Height);
				g.DrawLine(Pens.Red, picturebox.Width, 0, 0, picturebox.Height);
				g.InterpolationMode = im;
				g.SmoothingMode = sm;
				return;
			}

			var sel = ScreenSelection;
			var pc = _selection.Location.Add(new PointF(_selection.Size.Width / 2, _selection.Size.Height / 2)).ToPoint();
			Point hl = new Point(5, 0), vl = new Point(0, 5);

			float a = 0f;

			if (a != 0)
			{
				var bmp = ScaledBitmap;
				float hw = bmp.Width / 2f;
				float hh = bmp.Height / 2f;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

				g.TranslateTransform(hw, hh);
				g.RotateTransform(a);
				g.TranslateTransform(-hw, -hh);
				g.DrawImage(bmp, 0, 0);

				g.ResetTransform();
			}

			// start with image
			else
				g.DrawImage(ScaledBitmap, 0, 0);

			// draw selection box 
			g.DrawXORRectangle(Pens.Black, sel);
			
			// draw drag handle
			ControlPaint.DrawGrabHandle(g, new Rectangle(sel.Right - 3, sel.Bottom - 3, 7, 7), false, true);

			// draw centre crosshair
			g.DrawXORLine(Pens.Black, pc.Subtract(hl), pc.Add(hl));
			g.DrawXORLine(Pens.Black, pc.Subtract(vl), pc.Add(vl));
		}

		private void picturebox_MouseDown(object sender, MouseEventArgs e)
		{
			panel1.Focus();

			if (e.Button != MouseButtons.Left || dragging)
				return;

			int z = imageZone(e.X, e.Y);
			if (z == 0)
				return;

			dragging = true;
			dragmode = z;
			drag_select = _selection;
			drag_origin = e.Location;
			drag_cent = _selection.Middle();
		}

		private void picturebox_MouseMove(object sender, MouseEventArgs e)
		{
			if (dragging)
			{
				PointF delta = e.Location.Subtract(drag_origin);
				if (dragmode == 1)
				{
					_selection = drag_select.Add(delta);
				}
				else if (dragmode == 2)
				{
					float w = drag_select.Width / 2f, h = drag_select.Height / 2f;
					if (delta.X > delta.Y)
					{
						w = Math.Max(20f, w + delta.X);
						h = w * (float)AspectRatio;
					}
					else
					{
						h = Math.Max(20 * (float)AspectRatio, h + delta.Y);
						w = h / (float)AspectRatio;
					}

					h = (float)Math.Round(h);
					w = (float)Math.Round(w);

					_selection = new RectangleF(drag_cent.X - w, drag_cent.Y - h, w * 2, h * 2);
				}
				picturebox.Invalidate();
			}
			else
				picturebox.Cursor = imgcurs[imageZone(e.X, e.Y)];
		}

		private void picturebox_MouseUp(object sender, MouseEventArgs e)
		{
			if (!dragging || e.Button != MouseButtons.Left)
				return;

			dragging = false;
			UpdatePreview();
        }

		private void trackBar1_ValueChanged(object sender, EventArgs e)
		{
			ViewScale = trackBar1.Value / 100f;
		}

		private void AspectRatio_CheckChanged(object sender, EventArgs e)
		{
			var ctrl = sender as RadioButton;
			if (ctrl.Checked)
			{
				if (ctrl == aspect8_7)
					AspectRatio = 8 / 7f;
				else if (ctrl == aspect5_4)
					AspectRatio = 5 / 4f;
				else if (ctrl == aspect4_3)
					AspectRatio = 4 / 3f;
				else if (ctrl == aspect3_2)
					AspectRatio = 3 / 2f;
			}
		}

		private void pbctxSave_Click(object sender, EventArgs e)
		{
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var res = SaveImage();
			sw.Stop();

			var sb = new StringBuilder();
			sb.AppendFormat("Save {0}", res.Success ? " completed" : "failed");
			sb.AppendLine();
			sb.AppendFormat("File: {0}", res.Filename);
			if (res.Success)
			{
				sb.AppendLine();
				sb.AppendFormat("Size: {0:#,##0} byte{1}", res.FileSize, res.FileSize == 1 ? "" : "s");
				sb.AppendLine();
				sb.AppendFormat("JPEG Quality: {0}", res.Quality);
				sb.AppendLine();
				sb.AppendFormat("Dimensions: [{0}, {1}]", res.Dimensions.Width, res.Dimensions.Height);
				sb.AppendLine();
				sb.AppendFormat("Time: {0:0.0}ms", sw.ElapsedMilliseconds);
			}

			MessageBox.Show(sb.ToString(), "Save Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void pbctxUpdateAD_Click(object sender, EventArgs e)
		{
            UpdateADThumbnail();
		}

		private void pbctxReset_Click(object sender, EventArgs e)
		{
			var bmp = _sourcebitmap;
			_sourcebitmap = null;

			aspect5_4.Checked = true;
			trackBar1.Value = 50;
			_selection = new RectangleF(0, 0, 160, 200);
			numericUpDown1.Value = numericUpDown2.Value = numericUpDown3.Value = 1;

			_sourcebitmap = bmp;
			picturebox.Invalidate();
			UpdateControl();
			UpdatePreview();
		}

		private void pbctxClose_Click(object sender, EventArgs e)
		{
			RaiseClose();
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			adjustments.Brightness = (float)numericUpDown1.Value;
			UpdatePreview();
		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			adjustments.Contrast = (float)numericUpDown2.Value;
			UpdatePreview();
		}

		private void numericUpDown3_ValueChanged(object sender, EventArgs e)
		{
			adjustments.Gamma = (float)numericUpDown3.Value;
			UpdatePreview();
		}

		private void panel1_ClientSizeChanged(object sender, EventArgs e)
		{
			int margin = 6;
			picturebox.MinimumSize = new Size(panel1.ClientSize.Width - margin, panel1.ClientSize.Height - margin);
			picturebox.Size = (SourceBitmap == null) ? new Size(100, 100) : ScaledImageSize;
		}

		private void previewTimer_Tick(object sender, EventArgs e)
		{
			previewTimer.Enabled = false;
			internalUpdatePreview();
		}

        private void btnUpdateAD_Click(object sender, EventArgs e)
        {
            UpdateADThumbnail();
        }
#endregion
    }
}
