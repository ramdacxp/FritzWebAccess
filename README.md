# FritzWebAccess
This repository contains a little C# console application which demonstates the web- and SOAP TR064- access to a **AVM Fritz!Box** running **FRITZ!OS 5.50 or higher**.

## Usage
The console application has to be executed with the fritz box password as command line argument. If the optional second argument is given, the WLAN can be enabled/disabled.

  `FritzWebAccess.exe MyPassword [1|0]`

On the console the following info is displayed:
* **Web Access**
  * session id
  * auth status 
  * list of telephone calls (in and out) of the current day (as XML fragments)
* **SOAP Access**
  * number of registered clients
  * total number of bytes sent and received
  * properties of the fritz box wan access (type, max. rates and status)
  * current DSL info (status, current/max up/down rates, noise margin and attenuation)
  * the external ip address
  * WLAN info (if enabled, status, channel and SSID name)

```
Web Access

SessionId: 08fdedbd3e31c9ef
IsAuthenticated: True

<Call>
  <Id>2941</Id>
  <Type>1</Type>
  <Caller>03012345</Caller>
  <Called>SIP: 12345</Called>
  <CalledNumber>12345</CalledNumber>
  <Name>Mustermann , Max</Name>
  <Numbertype>sip</Numbertype>
  <Device>Office-Phone</Device>
  <Port>13</Port>
  <Date>01.01.17 13:33</Date>
  <Duration>0:11</Duration>
  <Count></Count>
  <Path />
</Call>
<Call>
  <Id>2940</Id>
  ...
</Call>

SOAP Access

HostNumberOfEntries:  13
TotalBytesSent:       140177460
TotalBytesReceived:   1498279811
CommonLinkProperties: AccessType: DSL, MaxUpstreamBitRate: 1065000, MaxDownstreamBitRate: 12088000, Status: Up
DslInterfaceInfo:     Status: Up, Up/Down- RateCurrent: 1061/11263, RateMax: 1065/12088, NoiseMargin: 100/100, Attenuation: 130/260
ExternalIPAddress:    92.194.40.11
WirelessLanInfo:      IsEnabled: True, Status: Up, Channel: 5, SSID: MyWLanNetwork
```

## API
The API to do this is pretty simple.

### Web Access

```
var f = new FritzWebAccess();

// Login via password only or via username/password
f.LogIn("myPassword");
// f.LogIn("myUserName", "myPassword");

Console.WriteLine("SessionId: {0}", f.SessionId);
Console.WriteLine("IsAuthenticated: {0}", f.IsAuthenticated);

var callerInfo = f.GetCallHistory(1);
```

### SOAP / TR064 Access

```
var soap = new FritzSoapAccess
{
    Password = password
};

if ( setWirelessLan )
{
    Console.Write("SetWirelessLan({0}) ...", enableWirelessLan);
    soap.SetWirelessLan(enableWirelessLan);
    Console.WriteLine(" Done.");
}

Console.WriteLine("HostNumberOfEntries:  {0}", soap.GetHostNumberOfEntries());
Console.WriteLine("TotalBytesSent:       {0}", soap.GetTotalBytesSent());
Console.WriteLine("TotalBytesReceived:   {0}", soap.GetTotalBytesReceived());

Console.WriteLine("CommonLinkProperties: {0}", soap.GetCommonLinkProperties());
Console.WriteLine("DslInterfaceInfo:     {0}", soap.GetDslInterfaceInfo());
Console.WriteLine("ExternalIPAddress:    {0}", soap.GetExternalIPAddress());
Console.WriteLine("WirelessLanInfo:      {0}", soap.GetWirelessLanInfo());
```

The more complex types are exposed as own classes. Other data will be returned as `string`.

* `CommonLinkProperties`
* `DslInterfaceInfo`
* `WirelessLanInfo`


## Todo

* Support additional TR064 statements, e.g. complete the hosts interface
* Release as library
* Nuget Packages
* Port to .Net Core
* Better error handling

Feel free to contribute. :)

## License
This code is based on the
[AVM development documentation](https://avm.de/service/schnittstellen/).
HTTP Session API and program samples can be found
[here](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/AVM_Technical_Note_-_Session_ID.pdf).

If not stated otherwise, the code is released under the following
[MIT License](https://opensource.org/licenses/MIT):

Copyright 2017 Michael Hoser

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
