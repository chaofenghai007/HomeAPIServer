%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe SCServicesMonitoring.exe
Net Start SCServicesMonitoring
sc config SCServicesMonitoring start= auto 
pause