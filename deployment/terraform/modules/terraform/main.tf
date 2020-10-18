#######################################################
# Variables & Locals
#######################################################
variable location {}
variable projectname {}

locals {
  storageName = "${var.projectname}tfstate"
  storageContainer = "terraform"
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.projectname}-terraform"
  location = var.location
}

resource "azurerm_storage_account" "terraform" {
  name                     = local.storageName
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "BlobStorage"
  access_tier              = "Cool"

  tags = {
    environment = "terraform"
  }
}

resource "azurerm_storage_container" "container" {
  name                  = local.storageContainer
  storage_account_name  = azurerm_storage_account.terraform.name
  container_access_type = "private"
}

#######################################################
# Outputs
#######################################################
output "storage_accesskey" {
  value = azurerm_storage_account.terraform.primary_access_key
}