
using System;
using UIKit;

namespace Simon.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            foreach (var font in UIFont.FamilyNames)
            {
                Console.WriteLine("Font Name for : " + font);
                foreach (var fontName in UIFont.FontNamesForFamilyName(font))
                {
                    System.Diagnostics.Debug.WriteLine(UIFont.FamilyNames);
                    System.Diagnostics.Debug.WriteLine(fontName);
                }
            }
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
