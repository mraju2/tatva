# Stage 1: Build the application
FROM node:18 AS builder

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application
COPY . .

# Build the Next.js application
RUN npm run build

# Stage 2: Run the application
FROM node:18 AS runner

# Set the working directory
WORKDIR /app

# Copy the build output and necessary files
COPY --from=builder /app/package.json /app/package-lock.json ./
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/public ./public
COPY --from=builder /app/node_modules ./node_modules

# Expose the Next.js default port
EXPOSE 3000

# Start the application
CMD ["npm", "run", "start"]
