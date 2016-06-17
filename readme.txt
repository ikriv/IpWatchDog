This is an IP watchdog service.

Its job is to monitor local computer's external IP address
(obtained via different public IP provider) and send e-mail notification
when it changes.

If you try to run the service, please copy the files to the 
Program Files folder first, because it runs under Network Service account,
and Network Service account doesn't have rights to open or execute files
from other users' home folder.
