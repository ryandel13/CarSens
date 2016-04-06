#########################################################
#														#
#				mkEngineering CarSens 1.2.1				#
#					  Release Notes						#
#														#
#########################################################

CarSens is a application to display various sensors. 
Target Environment is a CarPC running Windows 7 or higher.

Current Features
*	Supported Sensors: Temperature Sensor (DS18B20 running on ATMega)
*	Displaying Current Temperature and average temperature
*	Notifies on Reconnect, Disconnect and Failure
*	User Feeback for sensors which exceeds minimum or maximum values
*	Change Background
*	Developed for Touch input (Huge Controls)

System Requirements
*	Display with mininmum Resolution 1024x600 (hey, this is made for a 7" Display)
*	Windows 7, windows 8.1 recommended
*	.Net Framework 4.5
*	A SensorTestBoard to connect DS18B20 Temperature Sensors
*	A GPS Board to have Speed and Location

Version 1.0

Known Issues
*	Some ugly fonts still left at some points
*	You have to clean the SensorList to edit the name or max/min Values of a Sensor
*	Display issues with more than four sensors
*	UI flickering
*	Please feel free to give feedback



Version 1.1

Fixed Issues
*	Fixed some small issues with fullscreen
*	Corrected Calculation of Average Values	

Added Features
*	Implemented GPS support

Version 1.1.1

Fixed Issues
*	Displaying issues with more than 4 sensors
*	Sensor Name missing on loading

Version 1.2

Fixed Issues
*	Calculation of Average Value for GPS fixed
*	UI Problems with more than 4 Sensors fixed (Adding Wizard)
*	UI Problem with Fullscreen (Dialogue Windows will now be centered).

Added Features
*	Voltmeter Added (works with 4Channel Voltmeter)
*	Highlighting of Sensors in AddingWizard

Version 1.2.1

Fixed Issues
*	Icon for Voltmeter fixed
*	Disconnect of Voltmeter can now be recognized
*	Fixed Statusupdate for not Connected Sensors
*	Internal: Usage of MOCK Sensors stabilized
*	Only GPS Sensors at COM Port will be detected as GPS
*	Rows of Sensors depending on App-Width (Three in default dimension).


Roadmap for the Next Versions (There is no priority in this list)
*	Improved Support for GPS-Board (Location)
*	Maybe support for Amperemeters
*	Maybe support for pressure
*	Some UI improvements
*	Edit Sensor functionality
*	List, Remove, Add, Edit should be shown in one List
*	Better FullScreen Support
*	Start on Last state (fullscreen)
*	Improve ListLoading
*	SensorServer, it will run in the background. All Sensors can be accessed via SOAP or equal interface
*	Windows 8.1 TileApp, this will require the SensorServer. Each Tile can be set up as a single Sensor.
*	Android App (will work together with the SensorServer in the same Network)