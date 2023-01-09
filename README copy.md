# ModernWMS - 仓库管理系统

<div align="center">
  <img src="/logo.png" alt="ModernWMS logo" width="200" height="auto" />
  <h1>ModernWMS</h1>
  <p>开源的简易完整的仓库管理系统</p>

<!-- Badges -->
[![License: MIT](https://img.shields.io/badge/license-MIT-orange.svg)](https://opensource.org/licenses/MIT/)
![Release Version (latest Version)](https://img.shields.io/github/v/release/fjykTec/ModernWMS?color=orange&include_prereleases)
![QR Code Support](https://img.shields.io/badge/QR--Code-Support-orange.svg)
![Docker Support](https://img.shields.io/badge/Docker-Support-orange.svg)
![i18n Support](https://img.shields.io/badge/i18n-Support-orange.svg)
[![MySQL8](https://img.shields.io/badge/MySQL-8.0%2B-orange)](https://www.mysql.com/downloads/)

![repo size](https://img.shields.io/github/repo-size/fjykTec/ModernWMS)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/fjykTec/ModernWMS)
<!--![Contributors](https://img.shields.io/github/contributors/fjykTec/ModernWMS?color=blue) -->

![GitHub Org's stars](https://img.shields.io/github/stars/ModernWMS?style=social)
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

# Contents - 目录

- [ModernWMS - 仓库管理系统](#modernwms---仓库管理系统)
- [Contents - 目录](#contents---目录)
  - [Introduction - 介绍](#introduction---介绍)
  - [Requirements - 必要条件](#requirements---必要条件)
    - [Linux OS](#linux-os)
    - [Windows OS](#windows-os)
  - [Installation - 安装](#installation---安装)
    - [Linux](#linux)
    - [Windows](#windows)
    - [Docker](#docker)
  - [Usage - 用法](#usage---用法)
  - [Contact - 联系](#contact---联系)
  - [License - 版权信息](#license---版权信息)

## Introduction - 介绍
  该库存管理系统是，我们从多年ERP系统研发中总结出来的一套针对小型物流仓储供应链流程。 在工作过程中我们很多的中小企业，由于IT预算有限，所以无法用上适合他们的系统，却又实实在在存在仓储管理方面的需求，以此我们开始了这个项目。 为了帮助一些有需要的用户。

## Requirements - 必要条件

### Linux OS

+ Ubuntu 18.04(LTS),20.04(LTS),22.04(LTS)
+ CentOS Stream 8,9
+ RHEL 8(8.7),9(9.1)
+ Debian 10,11
+ openSUSE 15

### Windows OS

+ Windows 10 版本 1607 或更高版本
+ Windows Server 2012 或更高版本

## Installation - 安装

### Linux

+ 下载源码后编译
  + 第一步，下载源码

  ```bash
  cd /tmp/ && wget https://github.com/fjykTec/ModernWMS/archive/refs/heads/master.zip
  ```  

  + 第二步，安装.NET SDK 、运行时 和 NodeJS

  ```bash
  wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
  sudo apt-get install -y aspnetcore-runtime-7.0
  curl -fsSL https://deb.nodesource.com/setup_16.x | sudo -E bash -
  sudo apt install -y nodejs
  ```  

  + 第三步，编译前端和后端

  ```bash
  cd /tmp/ && unzip master.zip && cd ./ModernWMS-master
  mkdir -p /ModernWMS/frontend/ /ModernWMS/backend/
  cd ./frontend/ && yarn && yarn build && cp -rf ./frontend/dist/* /ModernWMS/frontend/
  cd ./backend/ && sudo dotnet publish && cp -rf ./backend/ModernWMS/bin/Debug/net7.0/publish/* /ModernWMS/backend/
  ```  

  + 第四步，安装nginx

  ```bash
  cd /tmp/ && wget http://nginx.org/download/nginx-1.18.0.tar.gz 
  tar -zxvf nginx-1.18.0.tar.gz && cd nginx-1.18.0
  ./configure --prefix=/etc/nginx --with-http_secure_link_module --with-http_stub_status_module --with-http_ssl_module --with-http_realip_module
  make && make install
  ```  
### Windows

+ 下载源码后编译部署
  + 第一步，下载源码
  ```PowerShell
  cd c:\
  wget -Uri https://github.com/fjykTec/ModernWMS/archive/refs/heads/master.zip  -OutFile master.zip
  Expand-Archive -Path C:\master.zip -DestinationPath C:\
  ```
  + 第二步，安装.NET SDK 、运行时 和 NodeJS
  ```CMD
  wget -Uri https://download.visualstudio.microsoft.com/download/pr/35660869-0942-4c5d-8692-6e0d4040137a/4921a36b578d8358dac4c27598519832/dotnet-sdk-7.0.101-win-x64.exe  -OutFile dotnet-sdk-7.0.101-win-x64.exe
  dotnet-sdk-7.0.100-win-x64.exe /install /quiet /norestart
  wget -Uri https://nodejs.org/dist/v16.13.1/node-v16.13.1-x64.msi  -OutFile node-v16.13.1-x64.msi
  msiexec /i .\node-v16.13.1-x64.msi /passive /norestart
  ```
  + 第三步，编译前端和后端
  ```
  md C:\ModernWMS\frontend\
  md C:\ModernWMS\backend\
  cd c:\ModernWMS-master\backend
  dotnet publish 
  copy-item -path ".\backend\ModernWMS\bin\Debug\net7.0\publish\" -destination "C:\ModernWMS\backend\" -recurse
  cd c:\ModernWMS-master\frontend  
  yarn && yarn build 
  copy-item -path ".\frontend\dist\" -destination "C:\ModernWMS\frontend\" -recurse
  ```
  + 第四步，安装nginx并启动
  ```
  cd C:\
  wget -Uri http://nginx.org/download/nginx-1.16.1.zip -OutFile nginx-1.16.1.zip
  Expand-Archive -Path C:\nginx-1.16.1.zip -DestinationPath C:\
  start .\nginx-1.16.1\nginx.exe
  cd C:\ModernWMS\backend\
  dotnet ModernWMS.dll --urls http://0.0.0.0:20011
  ```

### Docker


+ 下载源码后编译
  + 第一步，下载源码

  ```bash
  cd /tmp/ && wget https://github.com/fjykTec/ModernWMS/archive/refs/heads/master.zip
  ```  
  
  + 第二步，编译前端和后端

  ```bash
  cd /tmp/ && unzip master.zip && cd ./ModernWMS-master
  cd ./frontend/ && yarn && yarn build && cp -rf ./frontend/dist/* ./docker/frontend/
  cd ./backend/ && sudo dotnet publish && cp -rf ./backend/ModernWMS/bin/Debug/net7.0/publish/* ./docker/backend/
  ```  
  + 第三步，部署

  ```bash
  cd /tmp/ModernWMS-master/docker/
  docker build -t modernwms:1.0 .
  docker run -d -p 80:80  modernwms:1.0 /bin/bash ./run.sh
  ```
## Usage - 用法
  
  ```
  打开浏览器，进入：http://127.0.0.1 或者 http://部署电脑的IP地址
  ```
  <h4>
    <a href="https://wmsonline.ikeyly.com">体验地址入口</a>
  </h4> 

  ![image0.png](https://gitee.com/modernwms/ModernWMS/raw/develop/image0.png)

  ![image1.png](https://gitee.com/modernwms/ModernWMS/raw/develop/image1.png)

  ![image2.png](https://gitee.com/modernwms/ModernWMS/raw/develop/image2.png)

## Contact - 联系

<h4>
  <a href="https://github.com/fjykTec/ModernWMS/issues/new?template=bug_report.md&title=[BUG]">提交一个Bug</a>
</h4>
<h4>
  <a href="https://github.com/fjykTec/ModernWMS/issues/new?template=feature_request.md&title=[FR]">提交一个建议</a>
</h4>
<h4>
  <a href="https://jq.qq.com/?_wv=1027&k=YgVJGWnI">加入QQ群  757128595</a>
</h4>

## License - 版权信息
该项目使用的是 [MIT](https://opensource.org/licenses/MIT/) 协议. 详情查阅[LICENSE.txt](https://github.com/fjykTec/ModernWMS/master/LICENSE).必须遵守此协议。