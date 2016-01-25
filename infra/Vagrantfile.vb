# -*- mode: ruby -*-
# vi: set ft=ruby :

# Vagrantfile API/syntax version. Don't touch unless you know what you're doing!
VAGRANTFILE_API_VERSION = "2"

# system "vagrant plugin update"
# required_plugins = %w(vagrant-digitalocean)
# required_plugins.each do |plugin|
#   system "NOKOGIRI_USE_SYSTEM_LIBRARIES=1 vagrant plugin install #{plugin}" unless Vagrant.has_plugin? plugin
# end

Vagrant.configure(VAGRANTFILE_API_VERSION) do |config|
  config.vm.box = "ubuntu/trusty64"

  config.vm.define "fruity-vb" do |fruity|
    fruity.vm.hostname = "fruity-vb"
    fruity.vm.network "private_network", ip: "33.33.33.55" # VirtualBox version

    fruity.vm.synced_folder ".", "/vagrant", disabled: true
    fruity.vm.synced_folder "../data", "/vagrant", create: true, mount_options: ["dmode=777","fmode=777"]

    fruity.vm.provision "ansible" do |ansible|
      ansible.playbook = "playbook.yml"
      ansible.inventory_path = "inventory.ini"
      ansible.host_key_checking = false
      ansible.sudo = true
    end

    fruity.vm.provider "virtualbox" do |provider|
      provider.name = "fruity-vb"
      provider.customize ["modifyvm", :id, "--memory", "1024"]

      provider.customize ['modifyvm', :id, '--usb', 'on']
      # provider.customize ['usbfilter', 'add', '0', '--target', :id, '--name', 'edimax7718un', '--vendorid', '0x7392']
      # provider.customize ["controlvm", :id, "usbattach", "p=0x1015;v=0x1366;s=0x000068b16d280353;l=0x14100000"]
    end

  end
end
