#!/bin/bash
# start 1
/usr/sbin/nginx -g 'daemon off;' > /var/log/start1.log 2>&1 &
# start 2
dotnet /app/ModernWMS.dll --urls http://0.0.0.0:21011 > /var/log/start2.log 2>&1 &
# just keep this script running
while [[ true ]]; do
    sleep 1
done