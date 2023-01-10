FROM ubuntu:22.04

RUN apt-get update && apt-get install -y wget curl
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y aspnetcore-runtime-7.0 nginx \
    && curl -fsSL https://deb.nodesource.com/setup_16.x | /bin/bash - \
    && apt install -y nodejs \
    && mkdir -p /app/  /frontend/
WORKDIR /app  
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY ./frontend /frontend/ 
COPY ./backend /app/
COPY ./run.sh /app/run.sh
RUN chmod u+x /app/run.sh
EXPOSE 80
ENTRYPOINT ["/bin/bash"]