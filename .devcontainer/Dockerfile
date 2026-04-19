FROM mcr.microsoft.com/devcontainers/dotnet:1-10-bullseye

# Install Node.js (latest LTS)
RUN curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g npm@latest

# Install Angular CLI
RUN npm install -g @angular/cli

# Install additional development tools
RUN apt-get update && apt-get install -y \
    mongodb-tools \
    postgresql-client \
    git-flow \
    && rm -rf /var/lib/apt/lists/*

# Install .NET tools
RUN dotnet tool install -g dotnet-ef \
    && dotnet tool install -g dotnet-format \
    && dotnet tool install -g dotnet-reportgenerator-globaltool

# Set environment variables
ENV PATH="${PATH}:/root/.dotnet/tools"
ENV NODE_PATH="/usr/lib/node_modules"

# Create workspace directory
WORKDIR /workspace

# Set git to be safe for workspace folder
RUN git config --global --add safe.directory /workspace

# Expose ports
EXPOSE 5000 5001 4200 27017 9090

# Default shell
CMD ["/bin/bash"]
