provider "azurerm" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider being used
  version         = "=2.32.0"
  # we need this, as in customer subscription only certain resource providers were registered...
  # without it, any terraform plan/apply would fail with a 403 on trying to access e.g. Media Services
  # see https://github.com/hashicorp/terraform/issues/18180#issuecomment-394369502
  skip_provider_registration = false
  features {}
}

#######################################################
# Module + Config
#######################################################
module "terraform" {
  source = "../../modules/terraform"
  
  projectname = "simonsvosstest"
  location = "West Europe"
}

#######################################################
# Outputs
#######################################################
output "vnet_id" {
  value = module.terraform.storage_accesskey
}