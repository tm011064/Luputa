using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace CommonTools.Components.Graphics
{
	/// <summary>
	/// Summary description for CaptchaImage.
	/// </summary>
	/// <example>
	/// <![CDATA[
	/// // Insert the following code into the codebehind of an aspx page:      
	///
	/// protected void Page_Load(object sender, System.EventArgs e)
	/// {
	/// 	// Get the text to render from the querystring
	/// 	string text = Request.QueryString["v"];
	/// 
	/// 	if (!string.IsNullOrEmpty(text))
	/// 	{
	/// 		CaptchaImage ci = new CaptchaImage(
	/// 			text
	/// 			, 160, 40
	/// 			, "Verdana"
	/// 			, WarpStrength.Strong
	/// 			, Color.BlueViolet, Color.Beige, Color.White, Color.Turquoise);
	///      
	/// 		this.Response.Clear();
	/// 		this.Response.ContentType = "image/jpeg";
	/// 
	/// 		// Render the page's output as the captcha image
	/// 		ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);
	/// 
	/// 		ci.Dispose();
	/// 	}
	/// }
	/// ]]>
	/// </example>
	public class CaptchaImage : IDisposable
	{
		#region Public properties
		/// <summary>
		/// Gets the text containing the answer of the control.
		/// </summary>
		public string Text
		{
			get { return this.text; }
		}
		/// <summary>
		/// Gets the image bitmap of the Captcha image.
		/// </summary>
		public Bitmap Image
		{
			get { return this.image; }
		}
		/// <summary>
		/// Gets the image width.
		/// </summary>
		public int Width
		{
			get { return this.width; }
		}
		/// <summary>
		/// Gets the image height.
		/// </summary>
		public int Height
		{
			get { return this.height; }
		}
		#endregion

		#region Internal properties
		// Internal properties.
		private WarpStrength _WarpStrength = WarpStrength.Normal;
		private Color _BGColor = Color.White;
		private Color _DotColor = Color.Gray;
		private Color _FontColor = Color.Black;
		private Color _TextDistortioncolor = Color.White;

		private string text;
		private int width;
		private int height;
		private string familyName;
		private Bitmap image;

		// For generating random numbers.
		private Random random = new Random();
		#endregion

		/// <summary>
		/// Initializes a new instance of the CaptchaImage class
		/// </summary>
		/// <param name="s">The text for the captcha class.</param>
		/// <param name="width">The width of the control.</param>
		/// <param name="height">The height of the control.</param>
		public CaptchaImage(string s, int width, int height)
		{
			this.text = s;
			this.SetDimensions(width, height);
			this.GenerateImage();
		}

		/// <summary>
		/// Overloaded. Initializes a new instance of the CaptchaImage class using the
		/// specified text, width, height and font family.
		/// </summary>
		/// <param name="s">The text of the catcha control.</param>
		/// <param name="width">The width of the control.</param>
		/// <param name="height">The height of the control.</param>
		/// <param name="familyName">The Font to use</param>
		public CaptchaImage(string s, int width, int height, string familyName)
		{
			this.text = s;
			this.SetDimensions(width, height);
			this.SetFamilyName(familyName);
			this.GenerateImage();
		}

		/// <summary>
		/// Initializes a new instance of the CaptchaImage class using the specified text,
		/// width, height, and font family.
		/// </summary>
		/// <param name="pText">The text for the captcha control.</param>
		/// <param name="pWidth">The width of the captcha control.</param>
		/// <param name="pHeight">The height of the captcha control.</param>
		/// <param name="pFamilyName">The font family to use.</param>
		/// <param name="pWarpStrength">One of the WarpStrength values that indicate the noise
		/// strength to use.</param>
		/// <param name="pFontColor">The colour for the text.</param>
		/// <param name="pDotColor">The colour of the noise dots.</param>
		/// <param name="pBGColor">The colour of the background.</param>
		/// <param name="pTextDistortioncolor">The secondary colour for the text.</param>
		public CaptchaImage(string pText, int pWidth, int pHeight, string pFamilyName
			, WarpStrength pWarpStrength, Color pFontColor, Color pDotColor, Color pBGColor, Color pTextDistortioncolor)
		{
			this.text = pText;
			this.SetDimensions(pWidth, pHeight);
			this.SetFamilyName(pFamilyName);
			this._BGColor = pBGColor;
			this._DotColor = pDotColor;
			this._FontColor = pFontColor;
			this._WarpStrength = pWarpStrength;
			this._TextDistortioncolor = pTextDistortioncolor;

			this.GenerateImage();
		}

		/// <summary>
		/// Implements object.Finalize()
		/// </summary>
		~CaptchaImage()
		{
			Dispose(false);
		}

		/// <summary>
		/// Implements the IDisposable interface. Releases all resources used by this object.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Custom Dispose method to clean up unmanaged resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose of the bitmap.
				if (this.image != null)
				{
					this.image.Dispose();
					this.image = null;
				}
			}
		}

		/// <summary>
		/// Sets the image width and height.
		/// </summary>
		/// <param name="width">width of the control</param>
		/// <param name="height">The height of the control.</param>
		private void SetDimensions(int width, int height)
		{
			// Check the width and height.
			if (width <= 0)
				throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
			if (height <= 0)
				throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
			this.width = width;
			this.height = height;
		}

		/// <summary>
		/// Sets the font used for the image text.
		/// </summary>
		/// <param name="familyName">The font family name.</param>
		private void SetFamilyName(string familyName)
		{
			// If the named font is not installed, default to a system font.
			try
			{
				using (Font font = new Font(this.familyName, 16F))
				{
					this.familyName = familyName;
				}
			}
			catch
			{
				this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
			}
		}

		// ====================================================================
		// Creates the bitmap image.
		// ====================================================================
		private void GenerateImage()
		{
			// Create a new 32-bit bitmap image.
			Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

			// Create a graphics object for drawing.
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle rect = new Rectangle(0, 0, this.width, this.height);

			// Fill in the background.
			HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, _DotColor, _BGColor);
			g.FillRectangle(hatchBrush, rect);

			// Set up the text font.
			SizeF size;
			float fontSize = rect.Height + 1;
			Font font;
			// Adjust the font size until the text fits within the image.
			do
			{
				fontSize--;
				font = new Font(this.familyName, fontSize, FontStyle.Bold);
				size = g.MeasureString(this.text, font);
			} while (size.Width > rect.Width);

			// Set up the text format.
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			// Create a path using the text and warp it randomly.
			GraphicsPath path = new GraphicsPath();
			path.AddString(this.text, font.FontFamily, (int)font.Style, font.Size, rect, format);

			float v = 0F;
			switch (_WarpStrength)
			{
				case WarpStrength.Light:
					v = 4F;
					break;
				case WarpStrength.Normal:
					v = 3.8F;
					break;
				case WarpStrength.Strong:
					v = 3.7F;
					break;
			}
			PointF[] points =
			{
				new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
			Matrix matrix = new Matrix();
			matrix.Translate(0F, 0F);
			path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

			// Draw the text.
			hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, _TextDistortioncolor, _FontColor);
			g.FillPath(hatchBrush, path);

			// Add some random noise.
			int m = Math.Max(rect.Width, rect.Height);
			for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
			{
				int x = this.random.Next(rect.Width);
				int y = this.random.Next(rect.Height);
				int w = this.random.Next(m / 50);
				int h = this.random.Next(m / 50);
				g.FillEllipse(hatchBrush, x, y, w, h);
			}

			g.DrawRectangle(new Pen(new SolidBrush(Color.Black), 2), 0, 0, width - 1, height - 1);

			// Clean up.
			font.Dispose();
			hatchBrush.Dispose();
			g.Dispose();

			// Set the image.
			this.image = bitmap;
		}
	}
}
