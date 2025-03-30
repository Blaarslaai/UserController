# Use Node.js to build Angular app
FROM node:20 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --configuration=production

# Use Nginx to serve the built Angular app
FROM nginx:alpine
COPY --from=build /app/dist/webapp /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
