using System;
using System.Drawing;

namespace ADPhotoPrep
{
	public static class Extensions
	{
		public static Point Middle(this Rectangle a)
		{
			return new Point(a.Left + (a.Width >> 1), a.Top + (a.Height >> 1));
		}

		public static PointF Middle(this RectangleF r)
		{
			return new PointF(r.Left + r.Width / 2f, r.Top + r.Height / 2f);
		}


		public static Point ToPoint(this PointF p)
		{
			return new Point((int)(p.X + 0.5f), (int)(p.Y + 0.5f));
		}


		public static Point Add(this Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static PointF Add(this PointF a, PointF b)
		{
			return new PointF(a.X + b.X, a.Y + b.Y);
		}

		public static PointF Add(this PointF a, Point b)
		{
			return new PointF(a.X + b.X, a.Y + b.Y);
		}

		public static Rectangle Add(this Rectangle a, Point b)
		{
			return new Rectangle(a.Location.Add(b), a.Size);
		}

		public static RectangleF Add(this RectangleF a, PointF b)
		{
			return new RectangleF(a.Location.Add(b), a.Size);
        }

		public static Point Subtract(this Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}

		public static PointF Subtract(this Point a, PointF b)
		{
			return new PointF(a.X - b.X, a.Y - b.Y);
		}

		public static PointF Subtract(this PointF a, PointF b)
		{
			return new PointF(a.X - b.X, a.Y - b.Y);
		}

		public static Rectangle Subtract(this Rectangle a, Point b)
		{
			return new Rectangle(a.Location.Subtract(b), a.Size);
		}

		public static Size Add(this Size a, Point b)
		{
			return new Size(a.Width + b.X, a.Height + b.Y);
		}

		public static Size Add(this Size a, Size b)
		{
			return new Size(a.Width + b.Width, a.Height + b.Height);
		}

		public static Size Subtract(this Size a, Point b)
		{
			return new Size(a.Width - b.X, a.Height - b.Y);
		}

		public static Size Subtract(this Size a, Size b)
		{
			return new Size(a.Width - b.Width, a.Height - b.Height);
		}

		public static Rectangle ToRectangle(this RectangleF r)
		{
			return new Rectangle((int)(r.Left + 0.5), (int)(r.Top + 0.5), (int)(r.Width + 0.5), (int)(r.Height + 0.5));
		}
	}
}
