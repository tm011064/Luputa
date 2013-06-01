using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Components.Flash
{
	/// <summary>
	/// A user control that determines the version of Flash installed on a user's system.
	/// </summary>
    public static class FlashDetection
    {
        /// <summary>
        /// Detects via VB and Javascript whether the client has Flash installed. This script instances
        /// two global variables, 'flashinstalled' and 'flashversion'. After the detect, the variable flashinstalled can have three values:
        /// 2: Flash installed, 1: Flash not installed, 0: Unknown if Flash is installed.
        /// </summary>
        /// <param name="pLatestVersion">define the latest version of flash available. This integer is used in the vbscript part to loop through a try/catch block
        /// to determine which version is currently installed.</param>
        /// <returns>A string including the Javascript code for detecting flash (Script Tags included).</returns>
        public static string GetDetectVersionScript(int pLatestVersion)
        {
            return
@"
<SCRIPT LANGUAGE=""Javascript"">
<!--
//After the detect, the variable flashinstalled can have three values:
//2: Flash installed 
//1: Flash not installed 
//0: Unknown if Flash is installed 
var flashinstalled = 0;

var flashversion = 0;

MSDetect = ""false"";

if (navigator.plugins && navigator.plugins.length)
{
	x = navigator.plugins[""Shockwave Flash""];
	if (x)
	{
		flashinstalled = 2;
		if (x.description)
		{
			y = x.description;
			flashversion = y.charAt(y.indexOf('.')-1);
		}
	}
	else
		flashinstalled = 1;
	if (navigator.plugins[""Shockwave Flash 2.0""])
	{
		flashinstalled = 2;
		flashversion = 2;
	}
}
else if (navigator.mimeTypes && navigator.mimeTypes.length)
{
	x = navigator.mimeTypes['application/x-shockwave-flash'];
	if (x && x.enabledPlugin)
		flashinstalled = 2;
	else
		flashinstalled = 1;
}
else
	MSDetect = ""true"";

// -->
</SCRIPT>

<SCRIPT LANGUAGE=""VBScript"">

on error resume next

If MSDetect = ""true"" Then
	For i = 2 to " + pLatestVersion.ToString() + @"
		If Not(IsObject(CreateObject(""ShockwaveFlash.ShockwaveFlash."" & i))) Then

		Else
			flashinstalled = 2
			flashversion = i
		End If
	Next
End If

If flashinstalled = 0 Then
	flashinstalled = 1
End If

</SCRIPT>
";
        }

    }
}
