# FungEye

FungEye is an application designed for mushroom enthusiasts, providing tools and resources to enhance their foraging experience.

## Table of Contents

- [FungEye](#fungeye)
  - [Table of Contents](#table-of-contents)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
    - [1. Clone the Repository](#1-clone-the-repository)
    - [2. Set Up Environment Variables](#2-set-up-environment-variables)
    - [3. Configuration](#3-configuration)
      - [Deployment Templates](#deployment-templates)
  - [Usage](#usage)
    - [Local Development](#local-development)
    - [Azure Deployment](#azure-deployment)
  - [Help](#help)
    - [Key Points:](#key-points)

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **Docker**: Install Docker
- **Azure CLI**: Install Azure CLI
- **Make**: Ensure `make` is installed on your system.
- **Secrets File**: Obtain `secrets.json` and place it in the `FungEyeApi/FungEyeApi` directory.
- **AI Model Directory**: Obtain `inception_v3_mushroomsv1_5_5` and place it in the root of the project.

## Installation

### 1. Clone the Repository

 ```bash
 git clone https://github.com/Bakkonrad/FungEye.git
 cd FungEye
 ```
### 2. Set Up Environment Variables

Copy the example environment file and fill in your details:
```bash
cp .env.deploy.example .env.deploy
```
Edit .env.deploy with your Azure Container Registry (ACR) credentials and other necessary information.

### 3. Configuration

#### Deployment Templates
The deployment templates are located in the root directory:

```
deploy-fungeye-api.yml.template
deploy-fungeye-app.yml.template
deploy-fungeye-tfserving.yml.template
```
These templates use placeholders for sensitive data. Ensure you have configured `.env.deploy` correctly to replace these placeholders during deployment.

## Usage

### Local Development

To start the project locally, use the following commands:


Start the Project
```
make start
```
Stop the Project
```
make stop
```
Restart the Project
```
make restart
```
Rebuild All Services
```
make build
```
### Azure Deployment
For deploying to Azure, ensure you have set up your Azure account (login using Azure CLI and also login to ACR) and configured the necessary credentials in .env.deploy.

Build and Push API Image
```
make build-api
```
Build and Push Frontend Image
```
make build-app
```
Build and Push TF Serving Image
```
make build-ai
```
Deploy API to Azure Container Instances (ACI)
```
make deploy-api
```
Deploy Frontend to ACI
```
make deploy-app
```
Deploy TF Serving to ACI
```
make deploy-ai
```
Deploy All Services
```
make deploy-all
```
## Help

To view all available commands, run:

```
make help
```

### Key Points:
- **Prerequisites**: Lists required software.
- **Installation**: Guides cloning and initial setup.
- **Configuration**: Explains how to configure the project.
- **Usage**: Details commands for local development and Azure deployment.
- **Help**: Provides a command to list all Makefile targets.