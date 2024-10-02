# Create react app in docker

Create docker hub repository - publish
```
docker build -t facebook-api . 
docker run -it --rm -p 5085:8080 --name facebook_container facebook-api
docker run -d --restart=always --name facebook_container -p 5085:8080 facebook-api
docker run -d --restart=always -v d:/volumes/api/images:/app/images -v d:/volumes/api/EmailTemplates:/app/EmailTemplates --name facebook_container -p 5085:8080 facebook-api
docker run -d --restart=always -v /volumes/api/images:/app/images -v /volumes/api/EmailTemplates:/app/EmailTemplates --name facebook_container -p 5085:8080 facebook-api
docker ps -a
docker stop facebook_container
docker rm facebook_container

docker images --all
docker rmi facebook-api

docker login
docker tag facebook-api:latest novakvova/facebook-api:latest
docker push novakvova/facebook-api:latest

docker pull novakvova/facebook-api:latest
docker ps -a
docker run -d --restart=always --name facebook_container -p 5085:8080 novakvova/facebook-api


docker pull novakvova/facebook-api:latest
docker images --all
docker ps -a
docker stop facebook_container
docker rm facebook_container
docker run -d --restart=always --name facebook_container -p 5085:8080 novakvova/facebook-api
```

```nginx options /etc/nginx/sites-available/default
server {
    server_name   api-qubix.itstep.click *.api-qubix.itstep.click;
    location / {
       proxy_pass         http://localhost:5085;
       proxy_http_version 1.1;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_cache_bypass $http_upgrade;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
    }
}

server {
		server_name   qubix.itstep.click *.qubix.itstep.click;
		root /var/dist;
		index index.html;

		location / {
			try_files $uri /index.html;
			#try_files $uri $uri/ =404;
		}
}

server {
		server_name   admin-qubix.itstep.click *.admin-qubix.itstep.click;
		root /var/admin-qubix.itstep.click;
		index index.html;

		location / {
			try_files $uri /index.html;
			#try_files $uri $uri/ =404;
		}
}

sudo systemctl restart nginx
certbot
```

/var/api-qubix.itstep.click/





