# Makefile for FungEye project

# Variables
DOCKER_COMPOSE = docker-compose
DOCKER_COMPOSE_FILE = docker-compose.yml

# Colors for output
GREEN = \033[0;32m
RED = \033[0;31m
YELLOW = \033[0;33m
NC = \033[0m # No Color

# Default target
.DEFAULT_GOAL := help

# Help target
help:
	@echo -e "$(YELLOW)Available commands:$(NC)"
	@echo -e "  $(GREEN)make start$(NC)     - Start the FungEye project"
	@echo -e "  $(GREEN)make stop$(NC)      - Stop the FungEye project"
	@echo -e "  $(GREEN)make restart$(NC)   - Restart the FungEye project"
	@echo -e "  $(GREEN)make build$(NC)     - Rebuild all services"

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
restart: down up

# Rebuild all services
build:
	@echo -e "$(YELLOW)Rebuilding all services...$(NC)"
	$(DOCKER_COMPOSE) -f $(DOCKER_COMPOSE_FILE) build
	@echo -e "$(GREEN)Rebuild complete.$(NC)"

.PHONY: help up down restart logs build clean