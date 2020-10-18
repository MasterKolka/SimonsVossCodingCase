#######################################################
# Tenant stage: all the stuff per whitelabel-app that
# is "per environment" (dev/prod/etc.)
#######################################################

#######################################################
# Variables & Locals
#######################################################
variable location {}
variable projectname {}
variable stage {}

variable apiserver_plantier {}
variable apiserver_plansize {}
variable apiserver_plancapacity {}

variable containerregistry_url {}
variable containerregistry_admin_username {}
variable containerregistry_admin_password {}

#######################################################
# Tags
#######################################################
locals {
  # see example of how to add additional tags per resource later: https://www.terraform.io/docs/configuration-0-11/locals.html#examples
  resource_tags = {
    "Project"     = var.projectname
    "Environment" = var.stage
    "Scope"       = "stage"
  }
}



#######################################################
# Resources/Providers
#######################################################
provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version         = "=2.32.0"
  # we need this, as in customer subscription only certain resource providers were registered...
  # without it, any terraform plan/apply would fail with a 403 on trying to access e.g. Media Services
  # see https://github.com/hashicorp/terraform/issues/18180#issuecomment-394369502
  skip_provider_registration = true
  features {}
}

#######################################################
# Resource Group
#######################################################
resource "azurerm_resource_group" "rg" {
  name     = "${var.projectname}-${var.stage}"
  location = var.location
  tags = local.resource_tags
}

########################################################
# Application Insights
#######################################################
# Create application insights (IdentityServer)
resource "azurerm_application_insights" "appinsights" {
  name                = "${var.projectname}-appinsights-${var.stage}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  application_type    = "web"
  tags                = local.resource_tags
}

########################################################
# Api WebApp
#######################################################
# create app service plan (linux!)
resource "azurerm_app_service_plan" "appservice-plan" {
  name                = "${var.projectname}-plan-${var.stage}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  kind                = "Linux"
  # important: if you are going to use docker, the property "reserved" is mendatory to be set to true!
  reserved = true
  tags = local.resource_tags

  sku {
    tier      = var.apiserver_plantier
    size      = var.apiserver_plansize
    capacity  = var.apiserver_plancapacity
  }
}

locals {
  compose = file("../../../compose/docker-compose.yml")
}

resource "azurerm_app_service" "webapp" {
  name                = "${var.projectname}-registration-api-${var.stage}"
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.appservice-plan.id
  location            = var.location
  https_only          = true
  tags = local.resource_tags

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on        = true
    http2_enabled    = true
    # this is important: define which container to deploy, determines tag
    linux_fx_version = "COMPOSE|${base64encode(local.compose)}"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.appinsights.instrumentation_key

    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
    
    DOCKER_REGISTRY_SERVER_URL          = var.containerregistry_url
    DOCKER_REGISTRY_SERVER_USERNAME     = var.containerregistry_admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = var.containerregistry_admin_password
    
    ASPNETCORE_ENVIRONMENT              = "Production"
    DOCKER_ENABLE_CI                    = false
    WEBSITE_HTTPLOGGING_RETENTION_DAYS  = "7"
  }
}