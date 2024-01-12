# ModernWMS - Warehouse Management System

<div align="center">
  <img src="logo.png" alt="ModernWMS logo" width="200" height="auto" />
  <h1>ModernWMS</h1>
  <p>A simple, complete and open source warehouse management system</p>

<!-- Badges -->
[![License: Apache2.0](https://img.shields.io/badge/license-Apache2.0-orange.svg)](https://opensource.org/license/apache-2-0/)
![Release Version (latest Version)](https://img.shields.io/github/v/release/fjykTec/ModernWMS?color=orange&include_prereleases)
![QR Code Support](https://img.shields.io/badge/QR--Code-Support-orange.svg)
![Docker Support](https://img.shields.io/badge/Docker-Support-orange.svg)
![i18n Support](https://img.shields.io/badge/i18n-Support-orange.svg)
[![MySQL8](https://img.shields.io/badge/MySQL8.0%2B-Support-orange)](https://www.mysql.com/downloads/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server2017%2B-Support-orange)](https://www.mysql.com/downloads/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL12-Support-orange)](https://www.mysql.com/downloads/)

![repo size](https://img.shields.io/github/repo-size/fjykTec/ModernWMS)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/fjykTec/ModernWMS)
<!--![Contributors](https://img.shields.io/github/contributors/fjykTec/ModernWMS?color=blue) -->

![GitHub Org's stars](https://img.shields.io/github/stars/fjykTec/ModernWMS?style=social)
![GitHub Follows](https://img.shields.io/github/followers/ModernWMS?style=social)
![GitHub Forks](https://img.shields.io/github/forks/fjykTec/ModernWMS?style=social)
![GitHub Watch](https://img.shields.io/github/watchers/fjykTec/ModernWMS?style=social)
![Gitee Stars](https://gitee.com/modernwms/ModernWMS/badge/star.svg?theme=social)
![Gitee Forks](https://gitee.com/modernwms/ModernWMS/badge/fork.svg?theme=social)

![.NET](https://img.shields.io/badge/.NET-7.0.0-green)
![Vuetify Cli](https://img.shields.io/badge/Vuetify/cli-3.0.4-green)
![Vue](https://img.shields.io/badge/Vue-3.2.45-green)
![TypeScript](https://img.shields.io/badge/TypeScript-4.1.2-green)
![VXE Table](https://img.shields.io/badge/VXETable-4.3.7-green)
![Vite](https://img.shields.io/badge/Vite-4.0.0-green)
![NodeJS](https://img.shields.io/badge/NodeJS-16.13.1-green)
</div>
<div align="center">
  <h3>
  <a href="https://gitee.com/modernwms/ModernWMS/blob/master/README.zh_CN.md">中文文档</a>
  </h3>
  <h3>
  <a href="https://modernwms.ikeyly.com">Home Page</a>
  </h3>
</div>

# Contents

- [ModernWMS - Warehouse Management System](#modernwms---warehouse-management-system)
- [Contents](#contents)
  - [Introduction](#introduction)
  - [Requirements](#requirements)
    - [Linux OS](#linux-os)
    - [Windows OS](#windows-os)
  - [Installation](#installation)
    - [Linux](#linux)
    - [Windows](#windows)
    - [Docker(Optional)](#dockeroptional)
  - [Usage](#usage)
  - [Contact](#contact)
  - [License](#license)
  - [Donate](#donate)

## Introduction 

  The inventory management system is a set of small logistics warehousing supply chain processes that we have summarized from years of ERP system research and development. In the process of work, many of our small and medium-sized enterprises, due to limited IT budget, cannot use the right system for them, but there are real needs in warehouse management, that's how we started the project. To help some people who need it.

## Requirements

### Linux OS

+ Ubuntu 18.04(LTS),20.04(LTS),22.04(LTS)
+ CentOS Stream 8,9
+ RHEL 8(8.7),9(9.1)
+ Debian 10,11
+ openSUSE 15

### Windows OS

+ Windows 10(1607+),11(21H2+)
+ Windows Server 2012+

## Installation

### Linux

+ download the source code and compile
  + Step 1, download the source code

  ```bash
  cd /tmp/ && wget https://gitee.com/modernwms/ModernWMS/repository/archive/master.zip
  ```  

  + Step 2, Install .NET SDK and NodeJS

  ```bash
  wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
  curl -fsSL https://deb.nodesource.com/setup_16.x | sudo -E bash -
  sudo apt install -y nodejs
  sudo apt-get install gcc g++ make
  sudo npm install -g yarn
  ```  

  + Step 3, compile frontend and backend

  ```bash
  sudo apt install unzip
  cd /tmp/ && unzip master.zip && cd ./ModernWMS-master
  mkdir -p /ModernWMS/frontend/ /ModernWMS/backend/
  cd /tmp/ModernWMS-master/frontend/ 
  sed -i 's#http://127.0.0.1#http://IP address#g' ./.env.production
  yarn && yarn build && cp -rf /tmp/ModernWMS-master/frontend/dist/* /ModernWMS/frontend/
  cd /tmp/ModernWMS-master/backend/ && sudo dotnet publish && cp -rf /tmp/ModernWMS-master/backend/ModernWMS/bin/Debug/net7.0/publish/* /ModernWMS/backend/
  cp -rf /tmp/ModernWMS-master/backend/ModernWMS/wms.db /ModernWMS/backend/
  ```  

  + Step 4, Install Nginx

  ```bash
  cd /tmp/ && wget http://nginx.org/download/nginx-1.18.0.tar.gz 
  tar -zxvf nginx-1.18.0.tar.gz && cd nginx-1.18.0
  ./configure --prefix=/etc/nginx --with-http_secure_link_module --with-http_stub_status_module --with-http_realip_module --without-http_rewrite_module --without-http_gzip_module
  make && make install
  cp -rf /ModernWMS/frontend/* /etc/nginx/html/
  nohup /etc/nginx/sbin/nginx -g 'daemon off;' &
  cd /ModernWMS/backend/ && dotnet ModernWMS.dll --urls http://0.0.0.0:20011
  ```  
  
### Windows

+ download the source code and compile
  + Step 1, download the source code
  ```PowerShell
  cd C:\
  wget -Uri https://gitee.com/modernwms/ModernWMS/repository/archive/master.zip  -OutFile master.zip
  Expand-Archive -Path C:\master.zip -DestinationPath C:\
  ```
  + Step 2, Install .NET SDK and NodeJS
  ```CMD
  wget -Uri https://download.visualstudio.microsoft.com/download/pr/35660869-0942-4c5d-8692-6e0d4040137a/4921a36b578d8358dac4c27598519832/dotnet-sdk-7.0.101-win-x64.exe  -OutFile dotnet-sdk-7.0.101-win-x64.exe
  .\dotnet-sdk-7.0.101-win-x64.exe /install /quiet /norestart
  wget -Uri https://nodejs.org/dist/v16.13.1/node-v16.13.1-x64.msi  -OutFile node-v16.13.1-x64.msi
  msiexec /i .\node-v16.13.1-x64.msi /passive /norestart
  npm install -g yarn
  ```
  + Step 3, compile frontend and backend
  ```
  md C:\ModernWMS\frontend\
  md C:\ModernWMS\backend\
  cd C:\ModernWMS-master\backend
  dotnet publish 
  copy-item -path "C:\ModernWMS-master\backend\ModernWMS\bin\Debug\net7.0\publish\*" -destination "C:\ModernWMS\backend\" -recurse
  copy-Item "C:\ModernWMS-master\backend\ModernWMS\wms.db" -Destination "C:\ModernWMS\backend\"
  cd C:\ModernWMS-master\frontend  
  yarn
  yarn build 
  copy-item -path "C:\ModernWMS-master\frontend\dist\*" -destination "C:\ModernWMS\frontend\" -recurse
  ```
  + Step 4, Install Nginx
  ```
  cd C:\
  wget -Uri http://nginx.org/download/nginx-1.16.1.zip -OutFile nginx-1.16.1.zip
  Expand-Archive -Path C:\nginx-1.16.1.zip -DestinationPath C:\
  copy-item -path "C:\ModernWMS\frontend\*" -destination "C:\nginx-1.16.1\html\" -recurse
  cd C:\nginx-1.16.1\
  start nginx.exe
  cd C:\ModernWMS\backend\
  dotnet ModernWMS.dll --urls http://0.0.0.0:20011
  ```

### Docker(Optional)

+ Approach 1, download the image from docker hub

  + Step 1, install docker and download the image

  ```bash
  sudo apt install docker.io
  sudo docker pull modernwms/modernwms:1.0
  ```  

  + Step 2，deploy
  
  ```bash
  sudo docker run -d -p 20011:20011 -p 80:80  modernwms/modernwms:1.0 ./run.sh
  sudo docker ps -a | awk 'NR>1 && $2=="modernwms/modernwms:1.0" {print $1}'
  sudo docker exec -it <CONTAINER ID> /bin/bash
  ```

  After entering the Docker container, execute the following command in the container.

  ```bash
  grep -rl "http://127.0.0.1:20011" /frontend | xargs sed -i 's#http://127.0.0.1:20011#http://IP address:20011#g'
  exit
  ```

  restart container

  ```bash
  sudo docker restart <CONTAINER ID>
  ```

+ Approach 2, Build your own image
  + Step 1, download the source code

  ```bash
  cd /tmp/ && wget https://gitee.com/modernwms/ModernWMS/repository/archive/master.zip
  ```  

  + Step 2，Install .NET SDK and NodeJS

  ```bash
  wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
  curl -fsSL https://deb.nodesource.com/setup_16.x | sudo -E bash -
  sudo apt install -y nodejs
  sudo apt-get install gcc g++ make
  sudo npm install -g yarn
  ```  

  + Step 3, compile frontend and backend
  
  ```bash
  sudo apt install unzip
  cd /tmp/ && unzip master.zip && cd ./ModernWMS-master
  cd /tmp/ModernWMS-master/frontend/ && sed -i 's#http://127.0.0.1#http://IP address#g' ./.env.production
  yarn && yarn build && cp -rf /tmp/ModernWMS-master/frontend/dist/* /tmp/ModernWMS-master/docker/frontend/
  cd /tmp/ModernWMS-master/backend/ && sudo dotnet publish && cp -rf /tmp/ModernWMS-master/backend/ModernWMS/bin/Debug/net7.0/publish/* /tmp/ModernWMS-master/docker/backend/
  cp -rf /tmp/ModernWMS-master/backend/ModernWMS/wms.db /tmp/ModernWMS-master/docker/backend/
  ``` 

  + Step 4, deploy

  ```bash
  sudo apt install docker.io
  cd /tmp/ModernWMS-master/docker/
  docker build -t modernwms:1.0 .
  docker run -d -p 20011:20011 -p 80:80  modernwms:1.0 ./run.sh
  ```

## Usage

  ```shell
  Accessing ip address (http://127.0.0.1 or http://the IP address you depolyed) via web browser 
  
  Account: admin 
  Password: 1
  ```

  <h4>
    <a href="https://wmsonline.ikeyly.com">Demo</a>
  </h4> 

  <img src="image2.png" alt="image2" height="auto" />

  <img src="image0.png" alt="image0" height="auto" />

  <img src="image1.png" alt="image1" height="auto" />
  
## Contact

<h4>
  <a href="https://gitee.com/modernwms/ModernWMS/issues/new?issue%5Bassignee_id%5D=0&issue%5Bmilestone_id%5D=0">Report a BUG</a>
</h4>
<h4>
  <a href="https://gitee.com/leucoon/vue-element-plus-admin/issues/new?issue%5Bassignee_id%5D=0&issue%5Bmilestone_id%5D=0">Submit a suggestion</a>
</h4>

## License

Distributed under the [MIT](https://opensource.org/licenses/MIT/) License. See [LICENSE.txt](https://gitee.com/modernwms/ModernWMS/blob/master/LICENSE) for more information.This must be observed.

## Donate

If it's helpful to you, you can donate us by alipay,by wechat. Your support will encourage us to continue creating

<img src="alipay.jpg" alt="image3" height="auto" />
<img src="wechat.jpg" alt="image4" height="auto" />
