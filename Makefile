# Variables
DOCKER_COMPOSE = docker-compose
DOCKER_COMPOSE_FILE = docker-compose.yml
RESOURCE_GROUP = FungEye

# Include deployment environment variables
-include .env.deploy

# Colors for output
GREEN = \033[0;32m
RED = \033[0;31m
YELLOW = \033[0;33m
PINK = \033[38;5;206m
NC = \033[0m # No Color

# Default target
.DEFAULT_GOAL := help

# Help target
help:
	@echo -e "$(PINK)LOCAL DEVELOPMENT:$(NC)"
	@echo -e "  $(GREEN)make start$(NC)     - Start the FungEye project"
	@echo -e "  $(GREEN)make stop$(NC)      - Stop the FungEye project"
	@echo -e "  $(GREEN)make restart$(NC)   - Restart the FungEye project"
	@echo -e "  $(GREEN)make build$(NC)     - Rebuild all services"
	@echo -e "$(PINK)AZURE DEVELOPMENT:$(NC)"
	@echo -e "  $(GREEN)make build-api$(NC) - Build and push API image"
	@echo -e "  $(GREEN)make build-app$(NC) - Build and push frontend image"
	@echo -e "  $(GREEN)make build-ai$(NC)  - Build and push TF Serving image"
	@echo -e "  $(GREEN)make deploy-api$(NC)- Deploy API to ACI"
	@echo -e "  $(GREEN)make deploy-app$(NC)- Deploy frontend to ACI"
	@echo -e "  $(GREEN)make deploy-ai$(NC) - Deploy TF Serving to ACI"
	@echo -e "  $(GREEN)make deploy-all$(NC)- Deploy all services to ACI"

# Check for deployment environment variables
check-env:
ifndef ACR_SERVER
	$(error ACR_SERVER is not set. Please create .env.deploy file)
endif
ifndef ACR_USERNAME
	$(error ACR_USERNAME is not set. Please create .env.deploy file)
endif
ifndef ACR_PASSWORD
	$(error ACR_PASSWORD is not set. Please create .env.deploy file)
endif

# Generate deployment files
deploy-fungeye-%.yml: deploy-fungeye-%.yml.template check-env
	@echo -e "$(YELLOW)Generating deployment file for $*...$(NC)"
	@sed "s|\$${ACR_SERVER}|$(ACR_SERVER)|g; s|\$${ACR_USERNAME}|$(ACR_USERNAME)|g; s|\$${ACR_PASSWORD}|$(ACR_PASSWORD)|g" $< > $@

# Start the project
start:
	@echo -e "$(GREEN)Starting FungEye project...$(NC)"
	$(DOCKER_COMPOSE) -f $(DOCKER_COMPOSE_FILE) up -d
	@echo -e "$(GREEN)FungEye project is up and running!$(NC)"

# Stop the project
stop:
	@echo -e "$(RED)Stopping FungEye project...$(NC)"
	$(DOCKER_COMPOSE) -f $(DOCKER_COMPOSE_FILE) down
	@echo -e "$(RED)FungEye project has been stopped.$(NC)"

# Restart the project
restart: stop start

# Rebuild all services
build:
	@echo -e "$(YELLOW)Rebuilding all services...$(NC)"
	$(DOCKER_COMPOSE) -f $(DOCKER_COMPOSE_FILE) build
	@echo -e "$(GREEN)Rebuild complete.$(NC)"

# Build and push API image
build-api: check-env
	@echo -e "$(YELLOW)Building and pushing API image...$(NC)"
	cd FungEyeApi/FungEyeApi && \
	docker build -t $(ACR_SERVER)/fungeye-api:latest -f Dockerfile . && \
	docker push $(ACR_SERVER)/fungeye-api:latest
	@echo -e "$(GREEN)API image built and pushed successfully.$(NC)"

# Build and push frontend image
build-app: check-env
	@echo -e "$(YELLOW)Building and pushing frontend image...$(NC)"
	cd FungEyeApp && \
	docker build -t $(ACR_SERVER)/fungeye-app:latest -f Dockerfile . && \
	docker push $(ACR_SERVER)/fungeye-app:latest
	@echo -e "$(GREEN)Frontend image built and pushed successfully.$(NC)"

# Build and push TF Serving image
build-ai: check-env
	@echo -e "$(YELLOW)Building and pushing TF Serving image...$(NC)"
	docker build -t $(ACR_SERVER)/fungeye-tfserving:latest -f Dockerfile.tfserving . && \
	docker push $(ACR_SERVER)/fungeye-tfserving:latest
	@echo -e "$(GREEN)TF Serving image built and pushed successfully.$(NC)"

# Deploy API to ACI
deploy-api: deploy-fungeye-api.yml
	@echo -e "$(YELLOW)Deploying API to ACI...$(NC)"
	az container create --resource-group $(RESOURCE_GROUP) --file $<
	@echo -e "$(GREEN)API deployed successfully.$(NC)"
	@rm $<

# Deploy frontend to ACI
deploy-app: deploy-fungeye-app.yml
	@echo -e "$(YELLOW)Deploying frontend to ACI...$(NC)"
	az container create --resource-group $(RESOURCE_GROUP) --file $<
	@echo -e "$(GREEN)Frontend deployed successfully.$(NC)"
	@rm $<

# Deploy TF Serving to ACI
deploy-ai: deploy-fungeye-tfserving.yml
	@echo -e "$(YELLOW)Deploying TF Serving to ACI...$(NC)"
	@cat $<
	@echo -e "$(GREEN)TF Serving deployed successfully.$(NC)"
	@rm $<

# Deploy all services to ACI
deploy-all: deploy-api deploy-app deploy-ai

# Build and deploy everything
build-and-deploy-all: build-api build-app build-ai deploy-all

.PHONY: help start stop restart build build-api build-app build-ai deploy-api deploy-app deploy-ai deploy-all build-and-deploy-all check-env