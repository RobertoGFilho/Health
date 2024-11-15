using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Health.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            string dbPath = GetDatabasePath(); // Add this line to get the database path
            LoadApplication(new App(dbPath)); // Pass the dbPath to the App constructor

            return base.FinishedLaunching(app, options);
        }

        private string GetDatabasePath()
        {
            // Implement this method to return the correct database path
            // For example:
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3");
        }
    }
}
