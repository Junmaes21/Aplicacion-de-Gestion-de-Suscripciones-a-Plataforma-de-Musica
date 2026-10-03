terraform {
  required_version = ">= 1.6.0"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

variable "location" {
  default = "West Europe"
}

variable "resource_group_name" {
  default = "rg-sonora"
}

variable "container_image" {
  default = "ghcr.io/junmaes21/sonora-api:latest"
}

resource "azurerm_resource_group" "main" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_container_group" "api" {
  name                = "sonora-api"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  os_type             = "Linux"
  ip_address_type     = "Public"
  dns_name_label      = "sonora-music-api"

  container {
    name   = "api"
    image  = var.container_image
    cpu    = "1"
    memory = "1.5"

    ports {
      port     = 8080
      protocol = "TCP"
    }
  }
}

output "api_url" {
  value = "http://${azurerm_container_group.api.fqdn}:8080"
}
