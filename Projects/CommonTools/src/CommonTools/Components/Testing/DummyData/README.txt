Author: Roman Majewski

Description:

CommonTools.Components.Testing -> DummyData provides a simple and easy inheritable manager class which can be used to create 
random user profile dummy data (usernames, emails, addresses, phone numbers...). This can be usefull when you have to fill 
a database with millions of dummy users while testing your software.

The CommonTools.Components.Testing.DummyDataManager class relies on an xml file containing dummy values for names, streets, cities. The
schema for this xml file can be found at DummyData.xsd, a sample xml file is located at this directory.

The DummyDataManager class was designed to be easily inherited of with all random data creation logic being overridable virtual methods.

How to use:

	1) Copy the DummyData.xml file which can be found at the \\Testing\DummyData folder of this assembly to your project or hard drive.
	
	2) The following code produces a list of 100 dummy users, each with unique profile data:
	
		CommonTools.Components.Testing.DummyDataManager d = new CommonTools.Components.Testing.DummyDataManager(Server.MapPath("DummyData.xml"));
        List<CommonTools.Components.Testing.DummyUser> list = new List<CommonTools.Components.Testing.DummyUser>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(d.GetDummy());
        }