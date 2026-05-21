FROM node:20 AS client
# RUN apt-get update && apt-get upgrade -y
EXPOSE 4200
USER root

RUN mkdir /home/node/.npm-global
ENV PATH=/home/node/.npm-global/bin:$PATH
ENV NPM_CONFIG_PREFIX=/home/node/.npm-global

RUN npm install -g @angular/cli@21.1.4

WORKDIR /app

COPY ./ClientApp/package.json ./
COPY ./ClientApp/package-lock.json ./
COPY ./ClientApp/. ./

RUN npm install

CMD ["ng", "serve", "--port", "4200", "--host", "0.0.0.0", "--poll", "2000"]
