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
cd ../Marketing.WebApi
sudo dotnet publish -c Release -r linux-x64 ./Marketing.WebApi.csproj -o /var/incidenciaApi/
sudo chown -R ec2-user /var/incidenciaApi
sudo chmod 770 /var/incidenciaApi/*
cd ..
sudo systemctl stop nginx
sudo systemctl stop incidencia.service
sudo systemctl stop incidenciaApi.service
sudo mkdir /etc/nginx/sites-avaliable
sudo mkdir /etc/nginx/sites-enabled
sudo chown -R ec2-user /etc/nginx
sudo chmod 770 /etc/nginx/*
sudo cp -f incidencia.service /etc/systemd/system/incidencia.service
sudo cp -f incidenciaApi.service /etc/systemd/system/incidenciaApi.service
sudo cp -f programadeincidencia.com.br /etc/nginx/sites-available/programadeincidencia.com.br
sudo cp -f api.programadeincidencia.com.br /etc/nginx/sites-available/api.programadeincidencia.com.br
sudo ln -s /etc/nginx/sites-available/programadeincidencia.com.br /etc/nginx/sites-enabled/
sudo ln -s /etc/nginx/sites-available/api.programadeincidencia.com.br /etc/nginx/sites-enabled/
sudo systemctl start nginx
sudo systemctl enable nginx
sudo systemctl restart nginx
sudo systemctl start incidencia.service
sudo systemctl enable incidencia.service
sudo systemctl restart incidencia.service
sudo systemctl start incidenciaApi.service
sudo systemctl enable incidenciaApi.service
sudo systemctl restart incidenciaApi.service
chmod -R 700 /var/incidencia
chmod -R 700 /var/incidenciaApi
cd /
sudo certbot --nginx -d programadeincidencia.com.br -d www.programadeincidencia.com.br -d api.programadeincidencia.com.br
