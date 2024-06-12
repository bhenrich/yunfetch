# YunFetch

YunFetch is a simple system information fetching tool written in C#. I made this just for myself, but maybe I'll make it configurable and modular in the near future.
**Due to this being a project solely for myself, this might not work as expected for anyone else.**

Until that day comes I also will not do any technical support to help you get yunfetch working on your machine.

## Features

- Displays detailed system information in the terminal.
- Supports customization of output colors based on the Catppuccin Latte color scheme.
- Includes CPU, GPU, RAM, storage, and monitor information.
- Written in C# for cross-platform compatibility.

## Prerequisites
- An arch-based distro (btw)
- .NET ^8.0
- Nerdfonts for the cool icons.

## Usage
Clone the repository: `git clone https://github.com/bhenrich/yunfetch.git`

Do whatever you want.

## Congifuration
yunfetch creates a config at `~/.config/yunfetch/conf.yaml` on first startup. The "default configuration" is coincidentally exactly what I want and need, but it IS customisable. It should be pretty straight forward to realise how it works.

### Colors
- **textColor**: the ANSI code of the text color
- **highlightColor**: the ANSI code for your username's text color
- **accentColor**: the ANSI code for the icons' color
- **resetColor**: you really shouldn't edit this. it's the ANSI code to reset the term's color to default. editing this might temporarily break your terminal.

### Commands
I don't need to explain these case-by-case. It's the command that gets run to fetch the info about that module.
If you're using arch linux you shouldn't need to edit these, or if you do at least clone the project and adjust my parsing to work with your fancy custom commands.

### Other
- **defaultUsername**: Your user account's name. Not needed as of now, but nice to have.
- **defaultErrorMessage**: The message that get's displayed if a module fails to fetch.
- **clearTermOnRun**: Clear the terminal once yunfetch runs. This one's a boolean.

## LICENSE
yunfetch is licensed under the Mozilla Public License 2.0. Please see the LICENSE file of this repository for more information.

## Contributing
Feel free to PR.