How to Register/Unregister 
=======================

This template will prepare a Post-Build event to copy the .addin to the
Inventor folder ($(ALLUSERSPROFILE)\Autodesk\Inventor Addins\). This .addin file is deleted from
Inventor folder when a Clean is performed. You can remove/edit this option
at the project configuration. If you prefer do it manually, follow the steps:

## Setup/Usage Instructions

1) Build Project

2) Copy add-in dll file to one of following locations: 

  a) Anywhere, then *.addin file <Assembly> setting should be updated to the full path including the dll name
  b) Inventor <InstallPath>\bin\ folder, then *.addin file <Assembly> setting should be the dll name only: <AddInName>.dll
  c) Inventor <InstallPath>\bin\XX folder, then *.addin file <Assembly> setting shoule be a relative path: XX\<AddInName>.dll

3) Copy.addin manifest file to one of following locations:

  a) Inventor Version Dependent
     Windows XP:
	C:\Documents and Settings\All Users\Application Data\Autodesk\Inventor 2013\Addins\
     Windows7/Vista:
	C:\ProgramData\Autodesk\Inventor Addins\

  b) Inventor Version Independent
     Windows XP:
	C:\Documents and Settings\All Users\Application Data\Autodesk\Inventor Addins\
     Windows7/Vista:
	C:\ProgramData\Autodesk\Inventor Addins\

  c) Per User Override
     Windows XP:
	C:\Documents and Settings\<user>\Application Data\Autodesk\Inventor 2013\Addins\
     Windows7/Vista:
	C:\Users\<user>\AppData\Roaming\Autodesk\Inventor 2013\Addins\

4) Startup Inventor, the AddIn should be loaded

To unregister the AddIn, remove the Autodesk.<AddInName>.Inventor.addin from above mentioned .addin manifest file locations directly.

## License

That samples are licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.

## Written by 

Philippe Leefsma
