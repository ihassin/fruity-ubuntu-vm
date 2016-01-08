# NRF51 & NRF52 FruityMesh Ubuntu development VM provisioned with Vagrant, VirtualBox and Parallels

Running this Ansible script in conjunction with Vagrant will set up a VM that contains all you need for [NRF51](https://www.nordicsemi.com/eng/Products/Bluetooth-Smart-Bluetooth-low-energy/nRF51822) and [NRF52](https://www.nordicsemi.com/Products/nRF52-Series-SoC) programming using the [FruityMesh BLE framework](https://github.com/mwaylabs/fruitymesh).

Mac OS X users that want to develop from their Mac natively - please see [here](https://github.com/ihassin/fruitymesh-mac-osx).

![ansible](https://cloud.githubusercontent.com/assets/19006/10564465/5c29b77e-7583-11e5-91b8-de254a5d2992.gif)

# General steps
- Clone this repo
- Customise Vagrant file, /etc/hosts and ssh keys
- Run Vagrant up
- ssh to vm
- Start developing

# Pre-requisites

* Install VirtualBox from [here](https://www.virtualbox.org/wiki/Downloads).
* Install Vagrant from [here](https://docs.vagrantup.com/v2/installation).
* Install Ansible from [here](http://docs.ansible.com/ansible/intro_installation.html#getting-ansible).
* If you are using Parallels, download it from [here](http://trial.parallels.com/?lang=en&terr=en).
* And install the Vagrant-Parallels plugin from [here](https://github.com/Parallels/vagrant-parallels).

# Cloning this repo

```
git clone git@github.com:ihassin/fruitymesh-ubuntu-vm.git
```
or

```
git clone https://github.com/ihassin/fruitymesh-ubuntu-vm.git
```

# Provisioning the VM

The infrastructure as code for these VMs is in the infra sub-directory, so:

```
cd infra
```

## VirtualBox:

```
cp Vagrantfile.vb Vagrantfile
cp inventory.ini.vb inventory.ini
```
Vagrantfile assumes a base box named 'ubuntu/trusty64'.

## Parallels:

```
cp Vagrantfile.pvm Vagrantfile
cp inventory.ini.pvm inventory.ini
```

Vagrantfile assumes a base box named 'parallels/ubuntu-14.04'.

## Modify Vagrant file and /etc/hosts to contain the desired host name and IP address of your VM

If you want to change the VM's IP address, or networking in general, please edit Vagrantfile to suit your needs.

## Changing the ip address

The inventory file is set to load a DNS entry named 'fruity-vb' (for VirtualBox) and/or 'fruity-pvm' (For Parallels) . Make sure your /etc/hosts contains an entry for it. As an example:

```
33.33.33.55	fruity-vb		# VirtualBox version
```

(Parallels will give you its box's IP as it's loading)

The IP must match the entry in the VirtualBox Vagrantfile:

```
fruity.vm.network "private_network", ip: "33.33.33.55" # VirtualBox version
```

## Providing your own SSH public key to access the VM

You can place your public key in infra/roles/common/templates/ita.pub. The keys there will be added to the 'deploy' user that the script creates.
Placing your public key there will allow you to connect using ssh without the need to log in manually.

## Bringing up the VM

### Install the plugins:

```
vagrant plugin install vagrant-parallels
# or
vagrant plugin install virtualbox
```

You can then bring up the box for configuring by issuing the following command:

```
cd infra
vagrant up --provider virtualbox
# or
vagrant up --provider parallels
```

It will take about 15 minutes when it installs for the first time.

Once you have done that, you can ```ssh deploy@fruity-pvm``` or ```ssh deploy@fruity-vb```. At this time, deploy will not have a password. Change that quickly once you're on the VM!

If you want to access the VM using your own ssh key, insert your public key in common/files/ssh_keys.pub, or copy it in manually to ~deploy/.ssh/authorized_keys. If you copy it manually, it will be lost the next time you rebuild the VM, so do it once and for all in the Ansible script.

# Fruity Time!

Once the Ansible script finishes running, the user 'deploy' you will be logging in with has a blank password. To set a new password:

```
vagrant ssh
sudo passwd deploy
exit
```

Now that your password is set, you can SSH in as deploy user:

```
ssh deploy@fruity-vb # or deploy@fruity-pvm
```
and attempt to build the [FruityMesh image](https://github.com/mwaylabs/fruitymesh) for the NRF51 by issuing:

```
cd nrf/projects/fruitymesh
make clean
make
```

And the flash the device with the resulting image built in _build/FruityMesh.hex using JLink.
JLink can be found in ~/nrf/tools.

## Flashing using VirtualBox
It's hard to get proper USB support for VirtualBox images, so a quick and dirty way is to copy the resulting hex file to the shared directory on your host machine.
Do this by copying to /vagrant

## Flashing using Parallels
Plug in the device while the Parallels UI is foremost, and it will ask you where to attach the USB port to. Select the VM and you'll have access to run JLink.

# Testing - ServerSpec and Cucumber

## ServerSpec

Some ServerSpec tests accompany this VM, just for fun.
Execute them using:

```
rake
```

## Cucumber

Run cucumber and see this run:

```
Feature: As a ninja developer
  I want to have a FruityMesh development environment ready
  So that I can write the next mesh killer-app

  Scenario Outline: Having a VirtualBox Ubuntu VM to develop FruityMesh apps
    Given I use <provider> to create a vm at <host>
    When log on as "deploy"
    Then I can build the "fruitymesh" image
    And I see the result

    Examples:
    | provider      | host          |
    | "VirtualBox"  | "fruity-vb"   |
    | "Parallels"   | "fruity-pvm"  |
```

# Communicating with the device

## Minicom

This VM comes with Minicom to allow you to communicate with the device over serial port.
Establish communication using:

```
sudo minicom --device /dev/ttyACM0 --b 38400
```

## screen

This VM comes with screen to allow you to communicate with the device over serial port.
Establish communication using:

```
sudo screen /dev/ttyACM0 38400
```

# Experimenting with FruityMesh

Check out my [modified ping example](https://github.com/ihassin/fruitymesh-ping) that programs an RGB LED using GPIO pins to show signal strength status of connected devices on the mesh.

# Contributing

1. Fork it (https://github.com/ihassin/fruity-ubuntu-vm/fork)
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create a new Pull Request

# Code of conduct

Our code of conduct is [here](https://github.com/ihassin/fruitymesh-ubuntu-vm/blob/master/CODE_OF_CONDUCT.md).

# License

MIT

