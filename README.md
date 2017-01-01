# FritzWebAccess
This repository contains a little C# console application which demonstates the web access to a **AVM Fritz!Box** running **FRITZ!OS 5.50 or higher**.


## Usage
The console application has to be executed with the fritz box password as command line argument.

  `FritzWebAccess.exe MyPassword`

On the console the session id an auth status together with todays calls are displayed:

```
SessionId: 08fdedbd3e31c9ef
IsAuthenticated: True

<Call><Id>2941</Id><Type>1</Type><Caller>03012345</Caller><Called>SIP: 12345</Called><CalledNumber>12345</CalledNumber><Name>Mustermann, Max</Name><Numbertype>sip</Numbertype><Device>Office-Phone</Device><Port>13</Port><Date>01.01.17 13:33</Date><Duration>0:11</Duration><Count></Count><Path /></Call>

<Call><Id>2940</Id><Type>1</Type><Caller>03056789</Caller><Called>SIP: 12345</Called><CalledNumber>12345</CalledNumber><Name>Musterfrau, Monika</Name><Numbertype>sip</Numbertype><Device>Office-Phone</Device><Port>13</Port><Date>01.01.17 11:56</Date><Duration>0:03</Duration><Count></Count><Path /></Call>

(Return)
```

## API
The API to do this is very simple.

```
var f = new FritzWebAccess();

// Login via password only or via username/password
f.LogIn("myPassword");
// f.LogIn("myUserName", "myPassword");

Console.WriteLine("SessionId: {0}", f.SessionId);
Console.WriteLine("IsAuthenticated: {0}", f.IsAuthenticated);

var callerInfo = f.GetCallHistory(1);
```

## License
This code is based on the
[AVM development documentation](https://avm.de/service/schnittstellen/).
HTTP Session API and program samples can be found
[here](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/AVM_Technical_Note_-_Session_ID.pdf).

If not stated otherwise, it is released under the following
[MIT License](https://opensource.org/licenses/MIT):

Copyright 2017 Michael Hoser

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
