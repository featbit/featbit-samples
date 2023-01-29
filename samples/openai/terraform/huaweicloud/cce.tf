

# Create a VPC
resource "huaweicloud_vpc" "featbit_vpc" {
  name = "featbit_vpc"
  cidr = "192.168.0.0/16"
}

resource "huaweicloud_vpc_subnet" "featbit_vpc_subnet_0" {
  name          = "featbit_vpc_subnet_0"
  cidr          = "192.168.0.0/16"
  gateway_ip    = "192.168.0.1"

  //dns is required for cce node installing
  primary_dns   = "100.125.1.250"
  secondary_dns = "100.125.21.250"
  vpc_id        = huaweicloud_vpc.featbit_vpc.id
}


resource "huaweicloud_vpc_eip" "featbit_eip" {
  publicip {
    type = "5_bgp"
  }
  bandwidth {
    name        = "featbit_eip_bandwidth"
    size        = 3
    share_type  = "PER"
    charge_mode = "traffic"
  }
}

resource "huaweicloud_cce_cluster" "featbit_cce" {
  name                   = "featbit_cce"
  flavor_id              = "cce.s1.small"
  vpc_id                 = huaweicloud_vpc.featbit_vpc.id
  subnet_id              = huaweicloud_vpc_subnet.featbit_vpc_subnet_0.id
  container_network_type = "overlay_l2"
  eip                    = huaweicloud_vpc_eip.featbit_eip.address // If you choose not to use EIP, skip this line.
}

