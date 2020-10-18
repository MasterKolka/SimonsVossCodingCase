#######################################################
# Terraform remote state
#######################################################
terraform {
  # we use azure blob storage as backing store for terraform state
  backend "azurerm" {
    resource_group_name  = "simonsvosstest-terraform"
    storage_account_name = "simonsvosstesttfstate"
    container_name       = "terraform"
    # make sure you adjust this per new variant, we can't use a variable here...
    # first string part should match this filename
    key = "shared.terraform.tfstate"
    # no access key here.. follow https://docs.microsoft.com/en-us/azure/terraform/terraform-backend#configure-state-backend
    # set ARM_ACCESS_KEY env variable to storage account key via `setx ARM_ACCESS_KEY <storage account key>`
  }
}


#######################################################
# Module + Config
#######################################################
module "global" {
  source = "../../modules/shared"

  projectname = "simonsvosstest"
  location = "West Europe"
}


#######################################################
# Outputs
#######################################################
output "containerregistry_url" {
  value = module.global.containerregistry_url
}
output "containerregistry_admin_username" {
  value = module.global.containerregistry_admin_username
  sensitive = true
}
output "containerregistry_admin_password" {
  value = module.global.containerregistry_admin_password
  sensitive = true
}
output shared_rg {
  value = module.global.shared_rg
}