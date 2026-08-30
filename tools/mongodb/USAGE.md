# Create a script file in the backups directory (accessible from host at ./scripts)
# Example: ./scripts/add-property.js


## Running mongo-tools

# Start the mongo-tools container
docker run -d --name mongo-tools -v "$(pwd)/scripts:/scripts" mongo:latest

# Run a script from within the mongo-tools container
docker exec -it mongo-tools mongosh "mongodb://mongo:27017/MovieStore?directConnection=true" /scripts/add-property.js


