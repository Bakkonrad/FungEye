# Variables
DOCKER_COMPOSE = docker compose
DOCKER_COMPOSE_FILE = docker-compose.yml
RESOURCE_GROUP = FungEye
VERSION_FILE = .version
CHANGELOG_FILE = CHANGELOG.md

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
	@echo -e "  $(GREEN)make start-all$(NC) - Start all ACI services"
	@echo -e "  $(GREEN)make start-app$(NC) - Start fungeye-app ACI"
	@echo -e "  $(GREEN)make start-api$(NC) - Start fungeye-api ACI"
	@echo -e "  $(GREEN)make start-ai$(NC)  - Start fungeye-tfserving ACI"
	@echo -e ""
	@echo -e "  $(GREEN)make stop-all$(NC)  - Stop all ACI services"
	@echo -e "  $(GREEN)make stop-app$(NC)  - Stop fungeye-app ACI"
	@echo -e "  $(GREEN)make stop-api$(NC)  - Stop fungeye-api ACI"
	@echo -e "  $(GREEN)make stop-ai$(NC)   - Stop fungeye-tfserving ACI"
	@echo -e ""
	@echo -e "  $(GREEN)make build-api$(NC) - Build and push API image"
	@echo -e "  $(GREEN)make build-app$(NC) - Build and push frontend image"
	@echo -e "  $(GREEN)make build-ai$(NC)  - Build and push TF Serving image"
	@echo -e ""
	@echo -e "  $(GREEN)make deploy-api$(NC)- Deploy API to ACI"
	@echo -e "  $(GREEN)make deploy-app$(NC)- Deploy frontend to ACI"
	@echo -e "  $(GREEN)make deploy-ai$(NC) - Deploy TF Serving to ACI"
	@echo -e "  $(GREEN)make deploy-all$(NC)- Deploy all services to ACI"

# Bump version
bump-version:
	@current=$$(cat $(VERSION_FILE)); \
	next=$$(echo "$$current + 0.1" | bc -l | xargs printf "%.1f"); \
	echo $$next > $(VERSION_FILE); \
	echo -e "$(YELLOW)Version bumped: $$current -> $$next$(NC)"; \
	printf '\n## [%s] - %s\n- New version\n- Updated: %s\n' "$$next" "$$(date +'%d-%m-%Y')" "$(COMPONENT)" | cat - $(CHANGELOG_FILE) > temp && mv temp $(CHANGELOG_FILE)

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
	@export COMPONENT="API" && make bump-version && \
	VERSION=$$(cat $(VERSION_FILE)); \
	echo -e "$(YELLOW)Building and pushing $$COMPONENT image v$$VERSION...$(NC)"; \
	cd FungEyeApi/FungEyeApi && \
	docker build -t $(ACR_SERVER)/fungeye-api:v$$VERSION -f Dockerfile . && \
	docker push $(ACR_SERVER)/fungeye-api:v$$VERSION && \
	docker tag $(ACR_SERVER)/fungeye-api:v$$VERSION $(ACR_SERVER)/fungeye-api:latest && \
	docker push $(ACR_SERVER)/fungeye-api:latest && \
	echo -e "$(GREEN)$$COMPONENT image v$$(cat $(VERSION_FILE)) built and pushed successfully.$(NC)"

# Build and push frontend image
build-app: check-env
	@export COMPONENT="APP" && make bump-version && \
	VERSION=$$(cat $(VERSION_FILE)); \
	echo -e "$(YELLOW)Building and pushing $$COMPONENT image v$$VERSION...$(NC)"; \
	cd FungEyeApp && \
	docker build -t $(ACR_SERVER)/fungeye-app:v$$VERSION -f Dockerfile . && \
	docker push $(ACR_SERVER)/fungeye-app:v$$VERSION && \
	docker tag $(ACR_SERVER)/fungeye-app:v$$VERSION $(ACR_SERVER)/fungeye-app:latest && \
	docker push $(ACR_SERVER)/fungeye-app:latest && \
	echo -e "$(GREEN)$$COMPONENT image v$$(cat $(VERSION_FILE)) built and pushed successfully.$(NC)"

# Build and push TF Serving image
build-ai: check-env
	@export COMPONENT="Tf Serving" && make bump-version && \
	VERSION=$$(cat $(VERSION_FILE)); \
	echo -e "$(YELLOW)Building and pushing $$COMPONENT image v$$VERSION...$(NC)"; \
	docker build -t $(ACR_SERVER)/fungeye-tfserving:v$$VERSION -f Dockerfile.tfserving . && \
	docker push $(ACR_SERVER)/fungeye-tfserving:v$$VERSION && \
	docker tag $(ACR_SERVER)/fungeye-tfserving:v$$VERSION $(ACR_SERVER)/fungeye-tfserving:latest && \
	docker push $(ACR_SERVER)/fungeye-tfserving:latest && \
	echo -e "$(GREEN)$$COMPONENT image v$$(cat $(VERSION_FILE)) built and pushed successfully.$(NC)"

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
	@az container create --resource-group $(RESOURCE_GROUP) --file $<
	@echo -e "$(GREEN)TF Serving deployed successfully.$(NC)"
	@rm $<

# Deploy all services to ACI
deploy-all: deploy-api deploy-app deploy-ai

# Start fungeye-tfserving ACI
start-ai:
	@echo -e "$(YELLOW)Starting fungeye-tfserving ACI...$(NC)"
	@az container start --name fungeye-tfserving --resource-group $(RESOURCE_GROUP)
	@echo -e "$(GREEN)fungeye-tfserving ACI started.$(NC)"

# Start fungeye-app ACI
start-app:
	@echo -e "$(YELLOW)Starting fungeye-app ACI...$(NC)"
	@az container start --name fungeye-app --resource-group $(RESOURCE_GROUP)
	@echo -e "$(GREEN)fungeye-app ACI started.$(NC)"

# Start fungeye-api ACI
start-api:
	@echo -e "$(YELLOW)Starting fungeye-api ACI...$(NC)"
	@az container start --name fungeye-api --resource-group $(RESOURCE_GROUP)
	@echo -e "$(GREEN)fungeye-api ACI started.$(NC)"

# Start all ACI services
start-all: start-api start-app start-ai

# Stop fungeye-tfserving ACI
stop-ai:
	@echo -e "$(YELLOW)Stopping fungeye-tfserving ACI...$(NC)"
	@az container stop --name fungeye-tfserving --resource-group $(RESOURCE_GROUP)
	@echo -e "$(RED)fungeye-tfserving ACI stopped.$(NC)"

# Stop fungeye-app ACI
stop-app:
	@echo -e "$(YELLOW)Stopping fungeye-app ACI...$(NC)"
	@az container stop --name fungeye-app --resource-group $(RESOURCE_GROUP)
	@echo -e "$(RED)fungeye-app ACI stopped.$(NC)"

# Stop fungeye-api ACI
stop-api:
	@echo -e "$(YELLOW)Stopping fungeye-api ACI...$(NC)"
	@az container stop --name fungeye-api --resource-group $(RESOURCE_GROUP)
	@echo -e "$(RED)fungeye-api ACI stopped.$(NC)"

# Stop all ACI services
stop-all: stop-api stop-app stop-ai

.PHONY: help start stop restart build build-api build-app build-ai deploy-api deploy-app deploy-ai deploy-all build-and-deploy-all check-env
