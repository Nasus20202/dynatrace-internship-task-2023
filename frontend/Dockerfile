FROM node:16.15.0-alpine AS development
EXPOSE 3000

WORKDIR /app
COPY ./package.json /app
RUN npm install

COPY . .

CMD npm start