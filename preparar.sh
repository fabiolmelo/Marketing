#!/bin/bash

sudo yum update -y
sudo yum install git -y
sudo yum install -y dotnet-sdk-8.0
sudo yum install nginx -y
sudo rm -rf ./Marketing/
sudo rm -rf /var/incidenciaApi/
sudo rm -rf /var/incidencia/
cd Marketing/Marketing.Mvc
sudo dotnet publish -c Release -r linux-x64 ./Marketing.Mvc.csproj -o /var/incidencia/
sudo chown -R ec2-user /var/incidencia
sudo chmod 770 /var/incidencia/*
cd ../Marketing.WebApi
sudo dotnet publish -c Release -r linux-x64 ./Marketing.WebApi.csproj -o /var/incidenciaApi/
sudo chown -R ec2-user /var/incidenciaApi
sudo chmod 770 /var/incidenciaApi/*

sudo cp incidencia.service /etc/systemd/system/
sudo cp incidenciaApi.service /etc/systemd/system/
sudo cp nano.conf /etc/nginx
sudo systemctl start nginx
sudo systemctl enable nginx
sudo systemctl restart nginx
sudo systemctl start incidencia.service
sudo systemctl enable incidencia.service
sudo systemctl restart incidencia.service
sudo systemctl start incidenciaApi.service
sudo systemctl enable incidenciaApi.service
sudo systemctl restart incidenciaApi.service

