#!/bin/bash

sudo yum update -y
sudo yum install git -y
sudo yum install -y dotnet-sdk-8.0
sudo yum install nginx -y
sudo rm -rf /var/incidenciaApi/
sudo rm -rf /var/incidencia/
cd Marketing.Mvc
sudo dotnet publish -c Release -r linux-x64 ./Marketing.Mvc.csproj -o /var/incidencia/
sudo chown -R ec2-user /var/incidencia
sudo chmod 770 /var/incidencia/*
cd ..
sudo systemctl stop nginx
sudo systemctl stop incidencia.service
sudo systemctl stop incidenciaApi.service
sudo mkdir /etc/nginx/sites-avaliable
sudo mkdir /etc/nginx/sites-enabled
sudo chown -R ec2-user /etc/nginx
sudo chmod 770 /etc/nginx/*
sudo cp -f incidencia.service /etc/systemd/system/incidencia.service
sudo systemctl start nginx
sudo systemctl enable nginx
sudo systemctl restart nginx
sudo systemctl start incidencia.service
sudo systemctl enable incidencia.service
sudo systemctl restart incidencia.service
chmod -R 700 /var/incidencia
cd /
