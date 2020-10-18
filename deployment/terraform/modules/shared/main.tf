#######################################################
# Variables & Locals
#######################################################
variable location {}
variable projectname {}

locals {
  # see example of how to add additional tags per resource later: https://www.terraform.io/docs/configuration-0-11/locals.html#examples
  resource_tags = {
    "Project"     = var.projectname
    "Environment" = "shared"
    "Scope"       = "shared"
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
# Resource Groups
#######################################################
resource "azurerm_resource_group" "rg" {
  name     = "${var.projectname}-shared"
  location = var.location
  tags     = local.resource_tags
}

#######################################################
# Container registry
#######################################################
resource "azurerm_container_registry" "container-registry" {
  name                = "${var.projectname}containers"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  tags                = local.resource_tags

  sku                 = "Basic"
  admin_enabled       = true
}

#######################################################
# Outputs
#######################################################
output containerregistry_url {
  value = azurerm_container_registry.container-registry.login_server
}
output containerregistry_admin_username {
  value = azurerm_container_registry.container-registry.admin_username
}
output containerregistry_admin_password {
  value = azurerm_container_registry.container-registry.admin_password
  sensitive = true
}
output shared_rg {
  value = azurerm_resource_group.rg.name
}