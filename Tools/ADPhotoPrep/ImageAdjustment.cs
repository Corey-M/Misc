using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ADPhotoPrep
{
	public class ImageAdjustment
	{
		protected float _brightness = 1f;
		protected float _contrast = 1f;
		protected float _gamma = 1f;
		protected ImageAttributes _attribs = null;

		public float Brightness { get { return _brightness; } set { _brightness = value; _attribs = null; } }
		public float Contrast { get { return _contrast; } set { _contrast = value; _attribs = null; } }
		public float Gamma { get { return _gamma; } set { _gamma = value; _attribs = null; } }
		public ImageAttributes Attributes
		{
			get
			{
				if (_attribs == null)
				{
					// Code adapted from: http://stackoverflow.com/questions/15408607/adjust-brightness-contrast-and-gamma-of-an-image
					float adjustedBrightness = _brightness - 1.0f;
					// create matrix that will brighten and contrast the image
					float[][] ptsArray ={
						new float[] {_contrast, 0, 0, 0, 0}, // scale red
						new float[] {0, _contrast, 0, 0, 0}, // scale green
						new float[] {0, 0, _contrast, 0, 0}, // scale blue
						new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
						new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

					_attribs = new ImageAttributes();
					_attribs.ClearColorMatrix();
					_attribs.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
					_attribs.SetGamma(_gamma, ColorAdjustType.Bitmap);
				}
				return _attribs;
			}
		}

		public Bitmap Adjust(Bitmap source)
		{
			if (_brightness == 1f && _contrast == 1f && _gamma == 1f)
				return source;

			var adjustedImage = new Bitmap(source.Width, source.Height);
			using (Graphics g = Graphics.FromImage(adjustedImage))
				g.DrawImage(source, new Rectangle(new Point(0, 0), source.Size), 0, 0, source.Width, source.Height,
					GraphicsUnit.Pixel, Attributes);
			return adjustedImage;
		}
	}
}
