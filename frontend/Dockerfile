FROM node:16.15.0-alpine AS build

WORKDIR /app
COPY ./package*.json /app
RUN npm ci
COPY . /app
RUN npm run build

FROM nginx:alpine
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]