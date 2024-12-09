# Build stage
FROM --platform=linux/amd64 node:18 AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm install -D vite@4.5.0 esbuild@0.20.2
RUN npm run build

FROM --platform=linux/amd64 nginx:stable-alpine AS production
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]