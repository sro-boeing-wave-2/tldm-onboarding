echo Executing Deployment Script
COMPOSE_HTTP_TIMEOUT = 200 docker-compose -H ${DEPLOYMENT_SERVER}:2375 up --build -d --remove-orphans