This is the IP watchdog service.

Its job is to monitor local computer's external IP address, i.e. the one
visible from the Internet, and send e-mail notification when it changes.

This is useful for computers that host externally accessible services
on networks with semi-static IP addresses that only change several times
a year. 

See also: http://www.ikriv.com/dev/dotnet/IpWatchdog/index.shtml

INSTALLATION NOTE: The service runs under 'Network Service' account, which
by default does not have access to other user's home folder. Before you install
the service, either copy it to the Program Files folder, or give 'Network Service'
account read and execute rights to the folder where the server binaries are located.


GMAIL SECURITY NOTE: If you use gmail.com as your SMTP server, make sure
to enable 'account access for less secure apps'. Google considers anything
that uses only user name and password for authentication to be 'less secure',
even if the authentication is done via SSL/TLS.
https://support.google.com/accounts/answer/6010255?hl=en

ACKNOWLEDGEMENTS: Thanks to Meister1977 (Ambrózy Lorinc) for his contribution. 
