#!/bin/bash
echo "start init shell"

sudo yum update -y

# git
sudo yum install git -y
git clone https://github.com/taehyun-h/eva_server.git

# dotnet
rpm -Uvh https://packages.microsoft.com/config/rhel/7/packages-microsoft-prod.rpm
sudo yum install dotnet-sdk-2.2 -y
ln -sf /usr/share/dotnet/dotnet /usr/local/bin/dotnet

# tmux
sudo yum -y install  https://centos7.iuscommunity.org/ius-release.rpm
sudo yum install -y tmux
tmux new -s mywindow

# run server
cd eva_server
dotnet publish
sudo dotnet eva_server/bin/Debug/netcoreapp2.2/eva_server.dll
