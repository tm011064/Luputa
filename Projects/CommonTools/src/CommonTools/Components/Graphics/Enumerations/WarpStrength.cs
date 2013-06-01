using System;

namespace CommonTools.Components.Graphics
{
    /// <summary>
    /// Identifies the strength of the Catcha image.
    /// </summary>
    public enum WarpStrength
    {
        /// <summary>
        /// Light noise. Recommended for low and medium traffic sites.
        /// </summary>
        Light,
        /// <summary>
        /// Medium noise.
        /// </summary>
        Normal,

        /// <summary>
        /// High noise.
        /// </summary>
        Strong
    }
}
